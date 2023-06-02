using System;
using System.Threading;
using System.IO;

namespace DragonEngineLibrary
{
    public class MyClass
    {
        //Do whatever you want here
        static void ThreadTest()
        {
            try
            {

                DragonEngine.Log("Dragon Engine Library .NET Thread Start", Logger.Event.INFORMATION);

                while(!DragonEngine.IsEngineInitialized())
                {
                    DragonEngine.RefreshOffsets();
                    continue;
                }

                DragonEngine.RegisterJob(DragonEngine.LibUpdate, DEJob.Update);
                DragonEngine.Log("Dragon Engine initialized, initializing the library.", Logger.Event.INFORMATION);

                StartEngine();
            }
            catch(Exception ex)
            {
                DragonEngine.Log("Failed to initialize\nError:" + ex.Message + "\n\nStacktrace:\n" + ex.StackTrace, Logger.Event.FATAL);
            }


        }

        private static void StartEngine()
        {
            DragonEngine.Log("Starting initializaton of all mods.");
            DragonEngine.Log("Path: " + AppDomain.CurrentDomain.BaseDirectory + "\n");


            Thread modsThread = new Thread(LibThread);
            modsThread.Start();

            Diagnostics.LibraryAssembly.Init();
            StartInterface();
        }

        public static void LibThread()
        {
            if (Directory.Exists("mods"))
            {
                foreach (string directory in Directory.GetDirectories(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "mods")))
                {
                    foreach (string dllFile in Directory.GetFiles(directory, "*.dll"))
                    {
                        bool loadRes = DragonEngine.InitializeModLibrary(dllFile);

                        if (loadRes)
                            DragonEngine.Log("Successfully loaded DLL library in " + new DirectoryInfo(directory).Name);
                    }
                }
            }

            DragonEngine.Log("All mods have been initialized.");
            DragonEngine.Log("This is a test information log", Logger.Event.INFORMATION);
            DragonEngine.Log("This is a test debug log", Logger.Event.DEBUG);
            DragonEngine.Log("This is a test warning log", Logger.Event.WARNING);
            DragonEngine.Log("This is a test error log", Logger.Event.ERROR);
            DragonEngine.Log("This is a test fatal log", Logger.Event.FATAL);

            //dont let the thread die
            while (true) { }
        }


        public static void StartInterface()
        {
            Advanced.ImGui.Init();
            Advanced.ImGui.RegisterUIUpdate(Interface.DevInterface.Draw);
            Thread inputThread = new Thread(Interface.DevInterface.InputThread);
            inputThread.Start();
        }


        // This method will be called by native code inside the target process…
        public static void Main(string[] args)
        {
            //Create seperate thread for our C# library
            DragonEngine.Log("DragonEngine Library .Net Main Start");
            DragonEngine.Initialize();


            Thread thread1 = new Thread(ThreadTest);
            thread1.Start();
        }
    }
}
