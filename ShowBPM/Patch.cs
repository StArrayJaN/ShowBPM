
// ShowBPM.Patch
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using HarmonyLib;
using ShowBPM;
using UnityEngine;

internal static class Patch
{
	[HarmonyPatch(typeof(scrCalibrationPlanet), "Start")]
	internal static class scrCalibrationPlanet_Start
	{
		private static void Postfix()
		{
			if (Main.IsEnabled)
			{
				Main.gui.TextObject.SetActive(value: false);
			}
		}
	}

	[HarmonyPatch(typeof(scrUIController), "WipeToBlack")]
	internal static class scrUIController_WipeToBlack_Patch
	{
		private static void Postfix()
		{
			if (Main.IsEnabled)
			{
				Main.gui.TextObject.SetActive(value: false);
			}
		}
	}

	[HarmonyPatch(typeof(scnEditor), "ResetScene")]
	internal static class scnEditor_ResetScene_Patch
	{
		private static void Postfix()
		{
			if (Main.IsEnabled)
			{
				Main.gui.TextObject.SetActive(value: false);
			}
		}
	}

	[HarmonyPatch(typeof(scrController), "StartLoadingScene")]
	internal static class scrController_StartLoadingScene_Patch
	{
		private static void Postfix()
		{
			if (Main.IsEnabled)
			{
				Main.gui.TextObject.SetActive(value: false);
			}
		}
	}

	[HarmonyPatch(typeof(scrController), "Awake")]
	internal static class scrControllerAwake
	{
		public static void Prefix()
		{
			if (Main.IsEnabled)
			{
				Main.language = (Main.languages.ContainsKey(RDString.language.ToString()) ? Main.languages[RDString.language.ToString()] : Main.languages["English"]);
			}
		}
	}

	[HarmonyPatch(typeof(scnGame), "Play")]
	internal static class CustomLevelStart
	{
		private static void Postfix(scnGame __instance)
		{
			if (Main.IsEnabled && scrController.instance.gameworld && !((UnityEngine.Object)(object)scnGame.instance == null))
			{
				LevelStart(scrController.instance);
			}
		}
	}

	[HarmonyPatch(typeof(scrPressToStart), "ShowText")]
	internal static class BossLevelStart
	{
		private static void Postfix(scrPressToStart __instance)
		{
			if (Main.IsEnabled && scrController.instance.gameworld && !((UnityEngine.Object)(object)scnGame.instance != null))
			{
				LevelStart(scrController.instance);
			}
		}
	}

	private static string realKPSText = "";
 	public static CallRateTracker callRateTracker = new CallRateTracker();
	[HarmonyPatch(typeof(scrPlanet), "MoveToNextFloor")]
	internal static class MoveToNextFloor
	{
		public static void Postfix(scrPlanet __instance, scrFloor floor)
		{
			if (Main.IsEnabled && __instance.controller.gameworld && !((UnityEngine.Object)(object)floor.nextfloor == null))
			{
				List<string> list = new List<string>();
				bool isCW = scrController.instance.isCW;
				double a = scrMisc.GetAngleMoved(floor.entryangle, floor.exitangle, isCW) / 3.1415927410125732 * 180.0;
				double num = Math.Round(a);
				double num2 = 0.0;
				double num3 = 0.0;
				double num4 = 0.0;
				double num5 = 0.0;
				num2 = __instance.controller.speed;
				num4 = GetRealBpm(floor, bpm) * (double)playbackSpeed * (double)pitch;
				num5 = GetRealBpm(((UnityEngine.Object)(object)floor.nextfloor == null) ? floor : floor.nextfloor, bpm) * (double)playbackSpeed * (double)pitch;
				bool flag = false;
				if (Main.setting.ignoreMultipress)
				{
					bool flag3 = true;
					double angleMoved = scrMisc.GetAngleMoved(floor.entryangle, floor.exitangle, !floor.isCCW);
					double num6 = scrMisc.AngleToTime(angleMoved, (double)__instance.conductor.bpm * __instance.controller.speed);
					double num7 = 1.56905098538846;
					bool flag4 = flag3 && angleMoved > num7;
					double num8 = (double)__instance.controller.averageFrameTime * 2.5;
					bool flag5 = flag4 && num6 > num8;
					double num9 = 0.0299999993294477;
					flag = !(flag5 && num6 > num9) && !DoubleEqual(num5, num4);
				}
				if (Main.setting.onTileBpm)
				{
					list.Add(Main.setting.text1.Replace("{value}", format((float)((double)bpm * num2))));
				}
				if (flag || beforedt)
				{
					num4 = beforebpm;
				}
				if (Main.setting.onCurBpm)
				{
					list.Add(Main.setting.text2.Replace("{value}", format((float)num4)));
				}
				if (Main.setting.onRecommandKPS)
				{
					num3 = num4 / 60.0;
					list.Add(Main.setting.text3.Replace("{value}", Math.Round(num3).ToString()));
				}
				if (scnGame.instance.levelData.angleData[scrController.instance.currentSeqID] != 999)
				{
					callRateTracker.TrackCall();
				}
				Main.gui.setText(string.Join("\n", list));
				beforedt = flag;
				beforebpm = num4;
			}
		}
	}

	[HarmonyPatch(typeof(RDString), "ChangeLanguage")]
	internal static class ChangeLanguage
	{
		public static void Prefix(SystemLanguage language)
		{
			Main.language = (Main.languages.ContainsKey(language.ToString()) ? Main.languages[language.ToString()] : Main.languages["English"]);
			Main.gui.text.font = RDString.GetFontDataForLanguage(language).font;
		}
	}

	[HarmonyPatch(typeof(scrController), "Awake")]
	internal static class Awake
	{
		public static void Prefix()
		{
			if (first)
			{
				first = false;
				Main.gui.text.font = RDString.GetFontDataForLanguage(RDString.language).font;
			}
		}
	}
	
	[HarmonyPatch(typeof(scnEditor), "OnGUI")]
	internal static class scnEditor_Update
	{
		public static void Prefix(scnEditor __instance)
		{
			if (Main.setting.showRealKPS && __instance.playMode)
			{
				realKPSText = Main.setting.text4.Replace("{value}", callRateTracker.GetCallsPerSecond().ToString());
				GUIStyle style = new GUIStyle();
				style.fontSize = Main.setting.size;
				style.normal.textColor = Color.white;
				style.font = RDString.GetFontDataForLanguage(Persistence.language).font;
				GUI.Label(new Rect(Main.setting.realKPSPosition, new Vector2(300f, 30f)), realKPSText, style);
			}
		}
	}
	[HarmonyPatch(typeof(scnEditor), "DrawFloorNums")]
	internal static class DrawFloorNums
	{
		public static bool Prefix(scnEditor __instance)
		{
			if (!Main.setting.showSpeedText)
			{
				return true;
			}
			foreach (scrFloor floor in __instance.floors)
			{
				if (floor.editorNumText.letterText.text[0] == 'x' || floor.editorNumText.letterText.text == "t")
				{
					floor.editorNumText.letterText.text = floor.seqID.ToString();
					floor.editorNumText.letterText.color = basicColor;
				}
				floor.editorNumText.gameObject.SetActive(__instance.showFloorNums && !__instance.playMode);
			}
			return false;
		}
	}

	[HarmonyPatch(typeof(scrFloor), "LateUpdate")]
	internal static class LateUpdate
	{
		public static void Postfix(scrFloor __instance)
		{
			if (Main.setting.showSpeedText && (scrController.instance.gameworld || !(UnityEngine.Object)(object)scrController.instance.currFloor || scrController.instance.currFloor.freeroamGenerated) && !(__instance.editorNumText == null) && __instance.editorNumText.letterText.text[0] == 'x' && __instance.isFading)
			{
				Color color = __instance.editorNumText.letterText.color;
				__instance.editorNumText.letterText.color = new Color(color.r, color.g, color.b, __instance.opacity);
			}
		}
	}

	[HarmonyPatch(typeof(scrFloor), "OnBecameVisible")]
	internal static class OnBecameVisible
	{
		public static bool Prefix(scrFloor __instance)
		{
			if (!Main.setting.showSpeedText)
			{
				return true;
			}
			((Behaviour)(object)__instance).enabled = true;
			if ((UnityEngine.Object)(object)scrController.instance != null && scrController.instance.gameworld)
			{
				if ((UnityEngine.Object)(object)__instance.nextfloor != null && __instance.holdLength > -1)
				{
					((Behaviour)(object)__instance.nextfloor).enabled = true;
				}
				if ((UnityEngine.Object)(object)__instance.prevfloor != null && __instance.prevfloor.holdLength > -1)
				{
					((Behaviour)(object)__instance.prevfloor).enabled = true;
				}
			}
			if (__instance.editorNumText != null)
			{
				if (ADOBase.editor != null && ADOBase.editor.showFloorNums && ! ADOBase.editor.playMode && ADOBase.isLevelEditor)
				{
					__instance.editorNumText.letterText.text = __instance.seqID.ToString();
					bool active = ADOBase.editor.showFloorNums && !ADOBase.editor.playMode;
					__instance.editorNumText.gameObject.SetActive(active);
				}
				if (__instance.editorNumText.letterText.text[0] == 'x')
				{
					__instance.editorNumText.gameObject.SetActive(value: true);
				}
			}
			return false;
		}
	}

	private static float bpm = 0f;

	private static float pitch = 0f;

	private static float playbackSpeed = 1f;

	private static bool first = true;

	private static bool beforedt = false;

	private static double beforebpm = 0.0;

	private static Color basicColor = new Color(1f, 0f, 1f, 1f);

	private static bool DoubleEqual(double f1, double f2)
	{
		return Math.Abs(f1 - f2) < 0.001;
	}
	[HarmonyPatch(typeof(RDString), "ChangeLanguage")]
	private static class Patch_RDString_ChangeLanguage
	{
		public static void Postfix()
		{
			Main.InitLanguage();
		}
	}
        
	[HarmonyPatch(typeof(RDString), "Setup")]
	private static class Patch_RDString_Setup
	{
		public static void Postfix()
		{
			Main.InitLanguage();
		}
	}
	private static double GetRealBpm(scrFloor floor, float bpm)
	{
		if ((UnityEngine.Object)(object)floor == null)
		{
			return bpm;
		}
		if (floor.seqID == 0)
		{
			return bpm;
		}
		if ((UnityEngine.Object)(object)floor.nextfloor == null)
		{
			return floor.speed * bpm;
		}
		return 60.0 / (floor.nextfloor.entryTime - floor.entryTime);
	}

	public static string Repeat(string value, int count)
	{
		return new StringBuilder(value.Length * count).Insert(0, value, count).ToString();
	}

	public static string format(float v)
	{
		return string.Format("{0:0." + Repeat(Main.setting.zero ? "0" : "#", Main.setting.showDecimal) + "}", v);
	}

	public static void LevelStart(scrController __instance)
	{
		Main.gui.TextObject.SetActive(value: true);
		List<string> list = new List<string>();
		float num = 0f;
		if ((UnityEngine.Object)(object)scnGame.instance != null)
		{
			pitch = scnGame.instance.levelData.pitch / 100f;
			if (ADOBase.isScnGame)
			{
				pitch *= GCS.currentSpeedTrial;
			}
			playbackSpeed = (((UnityEngine.Object)(object)scnEditor.instance == null) ? 1f : scnEditor.instance.playbackSpeed);
			bpm = scnGame.instance.levelData.bpm * playbackSpeed * pitch;
		}
		else
		{
			pitch = scrConductor.instance.song.pitch;
			playbackSpeed = 1f;
			bpm = scrConductor.instance.bpm * pitch;
		}
		if (Main.setting.showSpeedText)
		{
			foreach (scrFloor listFloor in scrLevelMaker.instance.listFloors)
			{
				scrFloor nextfloor = listFloor.nextfloor;
				if (nextfloor != null && !(nextfloor.editorNumText == null) && !DoubleEqual(listFloor.speed, nextfloor.speed) && ((Main.setting.showSpeedTextMode == 1 && !DoubleEqual(GetRealBpm(listFloor, bpm), GetRealBpm(listFloor.nextfloor, bpm))) || Main.setting.showSpeedTextMode == 0))
				{
					double num2 = ((Main.setting.showSpeedTextMode == 0) ? ((double)(nextfloor.speed / listFloor.speed)) : (GetRealBpm(listFloor.nextfloor, bpm) / Math.Abs(GetRealBpm(listFloor, bpm))));
					listFloor.nextfloor.editorNumText.letterText.text = $"x{num2:0.##}";
					if (num2 > 1.0)
					{
						listFloor.nextfloor.editorNumText.letterText.color = new Color32((byte)Math.Max(240.0 - num2 * 7.0, 110.0), (byte)Math.Max(96.0 - num2 * 5.0, 0.0), (byte)Math.Max(96.0 - num2 * 5.0, 0.0), byte.MaxValue);
					}
					else
					{
						listFloor.nextfloor.editorNumText.letterText.color = new Color32((byte)Math.Max(96.0 - num2 * 5.0, 0.0), (byte)Math.Max(96.0 - num2 * 5.0, 0.0), (byte)Math.Max(240.0 - num2 * 7.0, 110.0), byte.MaxValue);
					}
					if (listFloor.floorRenderer != null && listFloor.floorRenderer.renderer.isVisible)
					{
						listFloor.nextfloor.editorNumText.gameObject.SetActive(value: true);
					}
					if (listFloor.legacyFloorSpriteRenderer != null && listFloor.legacyFloorSpriteRenderer.isVisible)
					{
						listFloor.nextfloor.editorNumText.gameObject.SetActive(value: true);
					}
				}
			}
		}
		float num3 = bpm;
		if (__instance.currentSeqID != 0)
		{
			double speed = scrController.instance.speed;
			num3 = (float)(bpm * speed);
		}
		if (Main.setting.onTileBpm)
		{
			list.Add(Main.setting.text1.Replace("{value}", format(num3)));
		}
		if (Main.setting.onCurBpm)
		{
			list.Add(Main.setting.text2.Replace("{value}", format(num3)));
		}
		if (Main.setting.onRecommandKPS)
		{
			num = num3 / 60f;
			list.Add(Main.setting.text3.Replace("{value}", Math.Round(num).ToString()));
		}
		Main.gui.setText(string.Join("\n", list));
		Main.gui.setSize(Main.setting.size);
	}
}
