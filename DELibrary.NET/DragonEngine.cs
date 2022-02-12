using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.IO;
using System.Reflection;

namespace DragonEngineLibrary
{
    public static class DragonEngine
    {
        internal static StreamWriter _logWriter;
        internal static MemoryStream _logStream;

        internal delegate void RegisterJobDelegate();

        internal class JobRegisterInfo
        {
            public Action funcRaw;
            public RegisterJobDelegate del;
            public IntPtr delPointer;
            public DEJob phase;
            public bool after;

            public JobRegisterInfo(Action func, RegisterJobDelegate del, IntPtr ptr, DEJob phase, bool after)
            {
                this.funcRaw = func;
                this.del = del;
                this.phase = phase;
                delPointer = ptr;
                this.after = after;
            }
        }
        internal static List<JobRegisterInfo> _jobDelegates = new List<JobRegisterInfo>();

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_INIT", CallingConvention = CallingConvention.Cdecl)]
        private static extern uint DELib_Init();

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_DRAGONENGINE_IS_ENGINE_INITIALIZED", CallingConvention = CallingConvention.Cdecl)]
        private static extern bool DELib_IsEngineInitialized();

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_DRAGONENGINE_REFRESH_OFFSETS", CallingConvention = CallingConvention.Cdecl)]
        private static extern void DELib_RefreshOffsets();

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_DRAGONENGINE_GET_DELTATIME", CallingConvention = CallingConvention.Cdecl)]
        private static extern float DELib_GetDeltaTime();

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_DRAGONENGINE_SET_SPEED", CallingConvention = CallingConvention.Cdecl)]
        private static extern void DELib_SetSpeed(DESpeedType speedType, float speed);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_GET_HUMAN_PLAYER", CallingConvention = CallingConvention.Cdecl)]
        private static extern uint DELib_GetHumanPlayer();

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_REGISTER_DE_JOB", CallingConvention = CallingConvention.Cdecl)]
        private static extern uint DELib_RegisterJob(IntPtr deleg, DEJob type, bool after);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_UNREGISTER_DE_JOB", CallingConvention = CallingConvention.Cdecl)]
        private static extern uint DELib_UnregisterJob(IntPtr deleg, DEJob type);

        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(int vKey);

        //Couldnt figure out creating a console despite calling AllocConsole
        //I'll just print to the existing console i have through a very depressing PInvoke
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_TEMP_CPP_COUT", CallingConvention = CallingConvention.Cdecl)]
        private static extern void DELib_TEMP_CPP_COUT(string text);

        /// <summary>
        /// Initialize Dragon Engine library. Important for it to properly function.
        /// </summary>
        public static void Initialize()
        {
            DELib_Init();
        }

        /// <summary>
        /// This is not related to DragonEngine.Initialize
        /// </summary>
        public static bool IsEngineInitialized()
        {
            return DELib_IsEngineInitialized();
        }

        public static void Log(object value)
        {
            string valueStr = value.ToString();

            DELib_TEMP_CPP_COUT(valueStr + "\n");
           // File.AppendAllText("dotnetlog.txt", valueStr + "\n");
        }

        /// <summary>
        /// Register a function that will be executed by Dragon Engine.
        /// </summary>
        /// <param name="after">If after is set to true, it will execute after main game functions.</param>
        public static void RegisterJob(Action action, DEJob jobID, bool after = false)
        {
            RegisterJobDelegate del = new RegisterJobDelegate(action);
            JobRegisterInfo inf = new JobRegisterInfo(action, del, Marshal.GetFunctionPointerForDelegate(del), jobID, after);
            _jobDelegates.Add(inf);

            DELib_RegisterJob(inf.delPointer, jobID, after);


            Log("Job for phase " + jobID.ToString() + " registered.");
        }

        /// <summary>
        /// Unregister a job that was registered.
        /// </summary>
        public static void UnregisterJob(Action func, DEJob phase)
        {
           
            foreach (JobRegisterInfo job in _jobDelegates.ToArray())
                if (job.phase == phase)
                    if (job.funcRaw == func)
                    {
                        DELib_UnregisterJob(job.delPointer, phase);
                    }
        }

        public static bool IsKeyDown(VirtualKey virtualKey)
        {
            return (GetAsyncKeyState((int)virtualKey)) == -32767;
        }

        public static bool IsKeyHeld(VirtualKey virtualKey)
        {
            return (GetAsyncKeyState((int)virtualKey) & 0x8000) == 0x8000;
        }


        public static float deltaTime
        {
            get
            {
                return DELib_GetDeltaTime();
            }
        }

        public static void SetSpeed(DESpeedType speedType, float speed)
        {
            DELib_SetSpeed(speedType, speed);
        }

        //Same as GetSceneEntity(SceneEntity.human_player)
        public static EntityHandle<Character> GetHumanPlayer()
        {
            return DELib_GetHumanPlayer();
        }

        public static void LibraryRenderUpdate()
        {
            DELib_RefreshOffsets();

            if (Advanced.DXHook.DELibrary_DXHook_GetWantHook())
                Advanced.ImGui.InitLib();
        }

        internal static bool InitializeModLibrary(string path)
        {
            if (string.IsNullOrEmpty(path))
                return false;

            if (!File.Exists(path))
            {
                Log(Directory.GetCurrentDirectory());
                Log(path + " does not exist.");
                return false;
            }

            try
            {
                //Ugly reflection type
                Assembly loadedAssembly = Assembly.LoadFrom(path);
                Type modInfoType = typeof(DEModInfo);

                foreach (CustomAttributeData dat in loadedAssembly.CustomAttributes)
                    if (dat.AttributeType.FullName == typeof(DEModInfo).FullName) // type comparing didnt work. so we compare names
                    {
                        ProcessDEMod(dat.ConstructorArguments);
                        return true;
                    }

                return false;
            }
            catch (Exception ex)
            {
                if(ex as BadImageFormatException == null)
                    Log("Failed to load library, error: " + ex.Message);

                return false;
            }
        }

        internal static void ProcessDEMod(IList<CustomAttributeTypedArgument> modInfo)
        {
            string modName = (string)modInfo[0].Value;
            Type modType = (Type)modInfo[1].Value;

            if (modType == null)
            {
                Log($"The mod {modName} does not have a valid mod initialization class");
                return;
            }


            if (modType.BaseType.FullName == "DragonEngineLibrary.DragonEngineMod")
            {
                Log("Initializing mod class");
                object createdObj = Activator.CreateInstance(modType);

                if (createdObj != null)
                    modType.GetMethod("OnModInit", BindingFlags.Public | BindingFlags.Instance).Invoke(createdObj, null);
                else
                    Log("Mod class initialization failed!");
            }
            else
                Log(modName + "'s initialization class does not derive from DragonEngineMod!");

        }
    }


    public class DragonEngineMod
    {
        public virtual void OnModInit()
        {

        }
    }
}
