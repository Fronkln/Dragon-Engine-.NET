using System;
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

        public delegate void RegisterJobDelegate();
        internal static List<RegisterJobDelegate> _jobDelegates = new List<RegisterJobDelegate>();

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_INIT", CallingConvention = CallingConvention.Cdecl)]
        private static extern uint DELib_Init();

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_DRAGONENGINE_IS_ENGINE_INITIALIZED", CallingConvention = CallingConvention.Cdecl)]
        private static extern bool DELib_IsEngineInitialized();

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_DRAGONENGINE_GET_DELTATIME", CallingConvention = CallingConvention.Cdecl)]
        private static extern float DELib_GetDeltaTime();

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_GET_HUMAN_PLAYER", CallingConvention = CallingConvention.Cdecl)]
        private static extern uint DELib_GetHumanPlayer();

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_REGISTER_DE_JOB", CallingConvention = CallingConvention.Cdecl)]
        private static extern uint DELib_RegisterJob(IntPtr deleg, DEJob type);

        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(int vKey);

        //Couldnt figure out creating a console despite calling AllocConsole
        //I'll just print to the existing console i have through a very depressing PInvoke
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_TEMP_CPP_COUT", CallingConvention = CallingConvention.Cdecl)]
        private static extern void DELib_TEMP_CPP_COUT(string text);

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
            File.AppendAllText("dotnetlog.txt", valueStr + "\n");
        }

        public static void RegisterJob(Action action, DEJob jobID)
        {
            RegisterJobDelegate del = new RegisterJobDelegate(action);
            _jobDelegates.Add(del);

            DELib_RegisterJob(Marshal.GetFunctionPointerForDelegate(del), jobID);


            Log("Job for phase " + jobID.ToString() + " registered.");
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

        //Same as GetSceneEntity(SceneEntity.human_player)
        public static EntityHandle<Character> GetHumanPlayer()
        {
            return DELib_GetHumanPlayer();
        }

        public static void LibraryRenderUpdate()
        {
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
