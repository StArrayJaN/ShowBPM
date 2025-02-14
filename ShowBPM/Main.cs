using System;
using System.Collections.Generic;
using HarmonyLib;
using System.Reflection;
using UnityModManagerNet;
using UnityEngine;
using GDMiniJSON;


namespace ShowBPM
{
    public static class Main
    {
        public static bool IsEnabled { get; private set; }

        internal static TextBehaviour gui;
        public static UnityModManager.ModEntry.ModLogger Logger { get; private set; }

        public static Harmony harmony;
        public static Setting setting;

        public static Dictionary<string, Language> languages = new Dictionary<string, Language>()
        {
            {"Korean", new Korean()},
            {"English", new English()},
            {"中文", new Chinese()}
        };

        public static Language language = new English();
        
        internal static void Setup(UnityModManager.ModEntry modEntry)
        {
            Logger = modEntry.Logger;
            setting = new Setting();
            setting = UnityModManager.ModSettings.Load<Setting>(modEntry);
            modEntry.OnToggle = OnToggle;
        }

        private static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            IsEnabled = value;
            if(value)
            {
                Start(modEntry);
                gui = new GameObject().AddComponent<TextBehaviour>();
                UnityEngine.Object.DontDestroyOnLoad(gui);
                modEntry.OnGUI = OnGUI;
                modEntry.OnSaveGUI = OnSaveGUI;
                gui.TextObject.SetActive(false);
                
            } else
            {
                gui.TextObject.SetActive(false);
                UnityEngine.Object.DestroyImmediate(gui);
                gui = null;
                Stop(modEntry);
            }
            return true;
        }
        public static void InitLanguage()
        {
            switch (RDString.language)
            {
                case SystemLanguage.ChineseSimplified:
                    language = languages["中文"];
                    break;
                case SystemLanguage.ChineseTraditional:
                    language = languages["中文"];
                    break;
                case SystemLanguage.Korean:
                    language = languages["Korean"];
                    break;
                case SystemLanguage.English:
                    language = languages["English"];
                    break;
            }
        }
        private static void OnGUI(UnityModManager.ModEntry modEntry)
	{
		setting.onTileBpm = GUILayout.Toggle(setting.onTileBpm, language.showTileBPM);
		if (setting.onTileBpm)
		{
			GUILayout.BeginHorizontal();
			GUILayout.Space(30f);
			setting.text1 = MoreGUILayout.NamedTextField(language.setTileBPM, setting.text1, 300f);
			GUILayout.EndHorizontal();
		}
		setting.onCurBpm = GUILayout.Toggle(setting.onCurBpm, language.showRealBPM);
		if (setting.onCurBpm)
		{
			GUILayout.BeginHorizontal();
			GUILayout.Space(30f);
			setting.text2 = MoreGUILayout.NamedTextField(language.setRealBPM, setting.text2, 300f);
			GUILayout.EndHorizontal();
		}
		setting.onRecommandKPS = GUILayout.Toggle(setting.onRecommandKPS, language.showKPS);
		if (setting.onRecommandKPS)
		{
			GUILayout.BeginHorizontal();
			GUILayout.Space(30f);
			setting.text3 = MoreGUILayout.NamedTextField(language.setKPS, setting.text3, 300f);
			GUILayout.EndHorizontal();
		}
		setting.showSpeedText = GUILayout.Toggle(setting.showSpeedText, language.showSpeedText);
		if (setting.showSpeedText)
		{
			GUILayout.BeginHorizontal();
			GUILayout.Space(30f);
			setting.showSpeedTextMode = ((!GUILayout.Toggle(setting.showSpeedTextMode == 0, language.setTileBPM)) ? 1 : 0);
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			GUILayout.Space(30f);
			setting.showSpeedTextMode = (GUILayout.Toggle(setting.showSpeedTextMode == 1, language.setRealBPM) ? 1 : 0);
			GUILayout.EndHorizontal();
		}
		setting.showRealKPS = GUILayout.Toggle(setting.showRealKPS, language.showRealKPS);
		if (setting.showRealKPS)
		{
	        GUILayout.BeginHorizontal();
	        GUILayout.Space(30f);
	        setting.text4 = MoreGUILayout.NamedTextField(language.setRealKPS, setting.text4, 300f);
	        GUILayout.EndHorizontal();
	        GUILayout.BeginHorizontal();
	        GUILayout.Space(30f);
	        Main.setting.realKPSPosition.x =
		        MoreGUILayout.NamedSlider(language.setX, setting.realKPSPosition.x, 0f, Screen.width, 300f, 1f, 0f, "{0:0.##}");
	        GUILayout.EndHorizontal();
	        GUILayout.BeginHorizontal();
	        GUILayout.Space(30f);
	        Main.setting.realKPSPosition.y =
		        MoreGUILayout.NamedSlider(language.setY, setting.realKPSPosition.y, 0f, Screen.height, 300f, 1f, 0f, "{0:0.##}");
	        GUILayout.EndHorizontal();
		}
		if (!setting.onTileBpm && !setting.onCurBpm && !setting.onRecommandKPS)
		{
			return;
		}
		GUILayout.Label("   ");
		setting.useShadow = GUILayout.Toggle(setting.useShadow, language.setShadow);
		gui.shadowText.enabled = setting.useShadow;
		setting.useBold = GUILayout.Toggle(setting.useBold, language.setBold);
		gui.text.fontStyle = (setting.useBold ? FontStyle.Bold : FontStyle.Normal);
		setting.zero = GUILayout.Toggle(setting.zero, language.setZeroPlaceHolder);
		setting.ignoreMultipress = GUILayout.Toggle(setting.ignoreMultipress, language.ignoreMultipress);
		GUILayout.BeginHorizontal();
		GUILayout.Space(20f);
		int num = (int)MoreGUILayout.NamedSlider(language.showDecimal, setting.showDecimal, 0f, 6f, 300f, 1f, 0f, "{0:0.##}");
		if (num != setting.showDecimal)
		{
			setting.showDecimal = num;
		}
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Space(20f);
		float num2 = MoreGUILayout.NamedSlider(language.setX, setting.x, -0.1f, 1.1f, 300f, 0.01f, 0f, "{0:0.##}");
		if (num2 != setting.x)
		{
			setting.x = num2;
			gui.setPosition(setting.x, setting.y);
		}
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Space(20f);
		float num3 = MoreGUILayout.NamedSlider(language.setY, setting.y, -0.1f, 1.1f, 300f, 0.01f, 0f, "{0:0.##}");
		if (num3 != setting.y)
		{
			setting.y = num3;
			gui.setPosition(setting.x, setting.y);
		}
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Space(20f);
		float num4 = MoreGUILayout.NamedSlider(language.setSize, setting.size, 1f, 100f, 300f, 1f, 0f, "{0:0.##}");
		if ((int)num4 != setting.size)
		{
			setting.size = (int)num4;
			gui.setSize(setting.size);
		}
		GUILayout.EndHorizontal();
		string[] array = new string[3] { language.alignLeft, language.alignCenter, language.alignRight };
		GUILayout.BeginHorizontal();
		GUILayout.Space(20f);
		GUILayout.Label(language.setAlign);
		GUIStyle gUIStyle = new GUIStyle(GUI.skin.button);
		string[] array2 = array;
		foreach (string text in array2)
		{
			if (setting.align == Array.IndexOf(array, text))
			{
				gUIStyle.fontStyle = FontStyle.Bold;
			}
			if (GUILayout.Button(text, gUIStyle))
			{
				setting.align = Array.IndexOf(array, text);
			}
			gUIStyle.fontStyle = FontStyle.Normal;
		}
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		gui.text.alignment = gui.toAlign(setting.align);
	}
	

        private static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
            setting.Save(modEntry);
        }

        private static void Start(UnityModManager.ModEntry modEntry)
        {
            harmony = new Harmony(modEntry.Info.Id);
            harmony.PatchAll(Assembly.GetExecutingAssembly());

        }

        private static void Stop(UnityModManager.ModEntry modEntry)
        {
            harmony.UnpatchAll(modEntry.Info.Id);
        }
    }
}
