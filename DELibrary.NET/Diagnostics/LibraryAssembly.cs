using System.Diagnostics;
using System.Reflection;

namespace DragonEngineLibrary.Diagnostics
{
    internal class LibraryAssembly
    {
        static Assembly assembly;
        static Process process;
        public static string CommitHash { get; private set; }
        public static string Version { get; private set; }
        public static string CLRVersion { get; private set; }


        internal static void Init()
        {
            assembly = Assembly.GetExecutingAssembly();
            process = Process.GetCurrentProcess();
            CommitHash = GetCommitHash();
            Version = GetVersion();
            CLRVersion = System.Runtime.InteropServices.RuntimeEnvironment.GetSystemVersion();
        }


        private static string GetCommitHash()
        {
            var attr = (AssemblyMetadataAttribute)assembly.GetCustomAttribute(typeof(AssemblyMetadataAttribute));
            return attr.Value;
        }


        private static string GetVersion()
        {
            return assembly.GetName().Version.ToString();
        }


        public static long GetProcessWorkingSet64()
        {
            process.Refresh();
            return process.WorkingSet64;
        }


        public static long GetProcessVirtualMemorySize64()
        {
            process.Refresh();
            return process.VirtualMemorySize64;
        }
    }
}
