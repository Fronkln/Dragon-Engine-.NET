using System;
using System.Collections.Generic;
using System.IO;

namespace DragonEngineLibrary
{
    /// <summary>
    /// Ryu Mod Manager functionality
    /// </summary>
    public static class RMM
    {
        public static string[] GetInstalledMods()
        {
            List<string> mods = new List<string>();

            if (File.Exists("ModLoadOrder.txt"))
            {
                foreach (string str in File.ReadAllLines("ModLoadOrder.txt"))
                {
                    if (str.Trim().StartsWith(";")) //commented/disabled mod
                        continue;

                    mods.Add(str);
                }
            }

            return mods.ToArray();
        }
    }
}
