// ShowBPM.Setting
using System;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using UnityModManagerNet;

namespace ShowBPM
{
    public class Setting : UnityModManager.ModSettings
    {
        public bool onTileBpm = true;

        public bool onCurBpm = true;

        public bool onRecommandKPS = true;

        public bool useShadow = true;

        public bool ignoreMultipress = false;

        public bool useBold = false;

        public bool showSpeedText = false;

        public int showSpeedTextMode = 0;

        public bool showRealKPS = false;
        
        public float x = 0.96f;

        public float y = 0.98f;

        public int size = 35;

        public int align = 2;

        public int showDecimal = 0;

        public bool zero = true;

        public string text1 = "타일 BPM - {value}";

        public string text2 = "체감 BPM - {value}";

        public string text3 = "초당 클릭 수 - {value}";
        
        public string text4 = "Real KPS - {value}";
        
        public Vector2 realKPSPosition = Vector2.zero;

        public override void Save(UnityModManager.ModEntry modEntry)
        {
            string path = GetPath(modEntry);
            try
            {
                StreamWriter textWriter = new StreamWriter(path);
                XmlSerializer xmlSerializer = new XmlSerializer(((object)this).GetType());
                xmlSerializer.Serialize(textWriter, this);
            }
            catch (Exception ex)
            {
                modEntry.Logger.Error("Can't save " + path + ".");
                modEntry.Logger.LogException(ex);
            }
        }

        public override string GetPath(UnityModManager.ModEntry modEntry)
        {
            return Path.Combine(modEntry.Path, GetType().Name + ".xml");
        }

    }
}