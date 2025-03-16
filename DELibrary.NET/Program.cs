using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.IO;
using System.Windows.Input;
using System.Security.AccessControl;
using System.Diagnostics;

namespace DragonEngineLibrary
{
    public class Library
    {
        [DllImport("kernel32")]
        static extern bool AllocConsole();

        protected static bool m_initOnce = false;
        private static HashSet<string> m_modsList = new HashSet<string>();

        public static string BaseDirectory;
        public static string Root;

        //Do whatever you want here
        static void ThreadTest()
        {
            try
            {
                DragonEngine.Log("\nDragon Engine Library .NET Thread Start");
                File.WriteAllText("de_log.txt", "");

                DragonEngine.RefreshOffsets();

                while (!DragonEngine.IsEngineInitialized())
                {
                    DragonEngine.RefreshOffsets();
                }

                DragonEngine.Log("Dragon Engine initialized, initializing the library.\n");
                StartEngine();
            }
            catch (Exception ex)
            {
                DragonEngine.Log("\n\n\nFailed to initialize\nError:" + ex.Message + "\n\nStacktrace:\n" + ex.StackTrace);
            }


        }

        private static void StartEngine()
        {
            DragonEngine.Log("Starting initializaton of all mods.");
            DragonEngine.Log("Path: " + AppDomain.CurrentDomain.BaseDirectory + "\n");


            Thread modsThread = new Thread(LibThread);
            modsThread.Start();
        }

        public static void LibThread()
        {
            AppDomain.CurrentDomain.AssemblyResolve += delegate (object sender, ResolveEventArgs args)
            {
                string assemblyName = args.Name.Split(',')[0];

                if (assemblyName == "DELibrary.NET")
                    return AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(x => x.GetName().Name == "DELibrary.NET");

                return null;
            };

            if (Directory.Exists("mods"))
            {
                int modCount = Parless.GetModCount();

                for (int i = 0; i < modCount; i++)
                {
                    IntPtr str = Parless.GetModName(i);
                    m_modsList.Add(Marshal.PtrToStringAnsi(str));
                }

                /*
                if (File.Exists("ModList.txt"))
                {
                    string[] mods = File.ReadAllText("ModList.txt").Split('|');
                    foreach (string mod in mods)
                        m_modsList.Add(mod.Substring(1));

                    foreach (var kv in m_modsList)
                        DragonEngine.Log(kv);
                }
                */

                foreach (string directory in Directory.GetDirectories(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "mods")))
                {
                    string dirName = new DirectoryInfo(directory).Name;

                    if (!m_modsList.Contains(dirName))
                        continue;

                    string configFile = Path.Combine(directory, "de_mod.ini");

                    if (!File.Exists(configFile))
                        continue;

                    Ini ini = new Ini(configFile);
                    string dllFile = ini.GetValue("InitDll");
    
                    bool loadRes = DragonEngine.InitializeModLibrary(Path.Combine(directory, dllFile));

                    if (loadRes)
                        DragonEngine.Log("Successfully loaded DLL library in " + new DirectoryInfo(directory).Name);
                    else
                        DragonEngine.Log("Failed to load DLL library in " + new DirectoryInfo(directory).Name);
                }
            }

            DragonEngine.Log("\n\nAll mods have been initialized.");
        }


        public static string GetModsDirectory()
        {
            return Path.Combine(BaseDirectory, "mods");
        }

        private static bool ShouldInitialize()
        {
            string modsDir = GetModsDirectory();

            if (!Directory.Exists(modsDir))
                return false;

            int modCount = Parless.GetModCount();

            if (modCount <= 0)
                return false;


            for(int i = 0; i < modCount; i++)
            {
                string modDir = Path.Combine(modsDir, Marshal.PtrToStringAnsi(Parless.GetModName(i)));

                if (!Directory.Exists(modDir))
                    continue;

                if (File.Exists(Path.Combine(modDir, "de_mod.ini")))
                    return true;
            }

            return false;
        }

        // This method will be called by native code inside the target process…
        public static void Main(string[] args)
        {
            //Initialize logging file
            //File.Create("dotnetlog.txt").Close();
            // DragonEngine._logStream = new MemoryStream();
            // DragonEngine._logWriter = new StreamWriter(DragonEngine._logStream);

            BaseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            Root = args[0];

            //Create seperate thread for our C# library
            DragonEngine.Log("DragonEngine Library .Net Main Start");
            DragonEngine.Log("BaseDirectory: " + BaseDirectory);
            DragonEngine.Log("RootDirectory: " + Root);

            if(!ShouldInitialize())
            {
                DragonEngine.Log("There is no need to initialize the library. No compatible mods detected. Aborting.");
                return;
            }

            // Environment.CurrentDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "mods", "DE Library");
            DragonEngine.Initialize();

            Environment.CurrentDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory);

            Thread thread1 = new Thread(ThreadTest);
            thread1.Start();
        }
    }
}
