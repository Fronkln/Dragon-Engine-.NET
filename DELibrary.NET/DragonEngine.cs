using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.IO;

namespace DragonEngineLibrary
{
    public static class DragonEngine
    {
        internal static StreamWriter _logWriter;
        internal static MemoryStream _logStream;

        public delegate void RegisterJobDelegate();
        internal static List<RegisterJobDelegate> _delegates = new List<RegisterJobDelegate>();

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_DRAGONENGINE_GET_DELTATIME", CallingConvention = CallingConvention.Cdecl)]
        private static extern float DELib_GetDeltaTime();

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_GET_HUMAN_PLAYER", CallingConvention = CallingConvention.Cdecl)]
        private static extern uint DELib_GetHumanPlayer();

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_INIT", CallingConvention = CallingConvention.Cdecl)]
        private static extern uint DELib_Init();

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_REGISTER_DE_JOB", CallingConvention = CallingConvention.Cdecl)]
        private static extern uint DELib_RegisterJob(IntPtr deleg, DEJob type);


        //Couldnt figure out creating a console despite calling AllocConsole
        //I'll just print to the existing console i have through a very depressing PInvoke
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_TEMP_CPP_COUT", CallingConvention = CallingConvention.Cdecl)]
        private static extern void DELib_TEMP_CPP_COUT(string text);

        //temp
        public static float deltaTime
        {
            get
            {
                return DELib_GetDeltaTime();
            }
        }

        public static EntityHandle<Character> GetHumanPlayer()
        {
            return DELib_GetHumanPlayer();
        }

        public static void Initialize()
        {
            DELib_Init();
        }


        public static void RegisterJob(Action action, DEJob jobID)
        {
            RegisterJobDelegate del = new RegisterJobDelegate(action);
            _delegates.Add(del);

            DELib_RegisterJob(Marshal.GetFunctionPointerForDelegate(del), jobID);


            Log("Job for phase " + jobID.ToString() + " registered.");
        }


        public static void Log(object value)
        {
            string valueStr = value.ToString();

            DELib_TEMP_CPP_COUT(valueStr + "\n");
            File.AppendAllText("dotnetlog.txt", valueStr + "\n");
        }
    }
}
