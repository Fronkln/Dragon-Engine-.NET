using System.Collections.Generic;
using System.Reflection;

namespace DragonEngineLibrary.Diagnostics
{
    //PLACEHOLDER
    internal class ModAssembly
    {
        Assembly assembly;
        AssemblyName[] referencedAssemblyNames;
        public static string Name { get; private set; }
        public static string AssemblyName { get; private set; }


        public ModAssembly(Assembly assembly, string name)
        {
            this.assembly = assembly;
            referencedAssemblyNames = assembly.GetReferencedAssemblies();
        }


        public int GetDependencyCount()
        {
            return referencedAssemblyNames.Length;
        }


        public List<string> GetDependencyNames()
        {
            List<string> returnList = new List<string>();
            foreach (var refassemblyname in referencedAssemblyNames)
            {
                returnList.Add(refassemblyname.FullName);
            }
            return returnList;
        }
    }
}
