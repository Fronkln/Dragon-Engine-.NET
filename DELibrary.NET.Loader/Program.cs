using System;
using System.IO;

namespace OOELibrary.Loader
{
    public class Program
    {
        //The main purpose of this assembly is to have the library *not* be the entrypoint.
        //This way all loaded mods can access the statics from the library instead of having a copy of the lib be automatically loaded as a dependency.
        public static void Main(string[] args)
        {
            try
            {
                AppDomain domain = AppDomain.CreateDomain("OOELibBase");           
                domain.ExecuteAssembly(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "mods/DE Library/DELibrary.NET.dll"));
            }
            catch(Exception ex)
            {
                Console.WriteLine("Loader Error: " + ex.InnerException + "     " + ex.Message);
            }
        }
    }
}
