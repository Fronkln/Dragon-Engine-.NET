using System.Collections.Generic;
using System.Reflection;

namespace DragonEngineLibrary.Diagnostics
{
    //PLACEHOLDER
    internal static class AssemblyManager
    {
        static Dictionary<string, ModAssembly> mods = new Dictionary<string, ModAssembly>();
        internal static List<string> ModNames = new List<string>();
        internal static List<string> AssemblyNames = new List<string>();


        internal static void RegisterModAssembly(Assembly assembly, string modName)
        {
            ModAssembly modAssembly = new ModAssembly(assembly, modName);
            mods.Add(modName, modAssembly);
            ModNames.Add(modName);
            AssemblyNames.Add(assembly.GetName().Name);
        }


        internal static ModAssembly GetModAssembly(string modName)
        {
            return mods[modName];
        }


        internal static string[] GetModNames()
        {
            return ModNames.ToArray();
        }

        internal static string[] GetModAssemblyNames()
        {
            return AssemblyNames.ToArray();
        }
    }
}
