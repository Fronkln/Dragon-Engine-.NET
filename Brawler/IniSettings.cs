using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brawler
{
    internal static class IniSettings
    {
        public static bool PreferGreenUI = false;


        public static void Read()
        {
            Ini ini = new Ini(Path.Combine(Mod.ModPath, "likeabrawler.ini"));
            PreferGreenUI = ini.GetValue("GreenBar", "UI") == "1";
        }

        public static void WriteDefault()
        {
            Ini ini = new Ini(Path.Combine(Mod.ModPath, "likeabrawler.ini"));
            ini.WriteValue("GreenBar", "UI", "0");
            ini.Save();
        }
    }
}
