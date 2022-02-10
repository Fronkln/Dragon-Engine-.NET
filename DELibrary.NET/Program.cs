using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.IO;
using System.Windows.Input;

namespace DragonEngineLibrary
{
    public class MyClass
    {
        //Do whatever you want here
        static void ThreadTest()
        {
            DragonEngine.Log("Dragon Engine Library .NET Thread Start");

            while (!DragonEngine.IsEngineInitialized())
            {
                continue;
            }

            DragonEngine.Log("Engine initialized");


            if (Directory.Exists("mods"))
            {
                foreach (string directory in Directory.GetDirectories("mods"))
                {
                    foreach (string dllFile in Directory.GetFiles(directory, "*.dll"))
                    {
                        bool loadRes = DragonEngine.InitializeModLibrary(dllFile);

                        if (loadRes)
                            DragonEngine.Log("Successfully loaded DLL library in " + new DirectoryInfo(directory).Name);
                    }
                }
            }

            DragonEngine.Log("\n\nAll mods have been initialized.");

            while (true) { }
        }

        // This method will be called by native code inside the target process…
        public static void Main(string[] args)
        {
            //Initialize logging file
            //File.Create("dotnetlog.txt").Close();
            // DragonEngine._logStream = new MemoryStream();
            // DragonEngine._logWriter = new StreamWriter(DragonEngine._logStream);

            //Create seperate thread for our C# library

            DragonEngine.Log("DragonEngine Library .Net Main Start");

            Thread thread1 = new Thread(ThreadTest);
            thread1.Start();

            DragonEngine.RegisterJob(DragonEngine.LibraryRenderUpdate, DEJob.Update);
        }
    }
}
