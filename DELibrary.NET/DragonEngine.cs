﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.IO;
using System.Reflection;
using static System.Collections.Specialized.BitVector32;
using System.Security.Cryptography;

namespace DragonEngineLibrary
{
    namespace Unsafe
    {
        public static class CPP
        {
            /// <summary>
            /// Very dangerous to use, doesnt call constructors on deletion.
            /// </summary>
            /// <param name="text"></param>
            [DllImport("Y7Internal.dll", EntryPoint = "LIB_CPP_FREE_MEM", CallingConvention = CallingConvention.Cdecl)]
            public static extern void FreeUnmanagedMemory(IntPtr memory);

            [DllImport("Y7Internal.dll", EntryPoint = "LIB_UNSAFE_NOP", CallingConvention = CallingConvention.Cdecl)]
            public static extern void NopMemory(IntPtr memory, uint len);


            [DllImport("Y7Internal.dll", EntryPoint = "LIB_UNSAFE_PATCH", CallingConvention = CallingConvention.Cdecl)]
            private static extern void Unsafe_NopMemory(IntPtr memory, IntPtr buf, int len);

            public static void PatchMemory(IntPtr addr, params byte[] bytes)
            {
                IntPtr byteArr = Marshal.AllocHGlobal(bytes.Length);
                Marshal.Copy(bytes, 0, byteArr, bytes.Length);

                Unsafe_NopMemory(addr, byteArr, bytes.Length);

                Marshal.FreeHGlobal(byteArr);
            }
        }
    }

    public static class DragonEngine
    {
        internal delegate void RegisterJobDelegate();
        internal delegate void RegisterWndProcDelegate(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam);

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
        internal static List<RegisterWndProcDelegate> _wndprocDelegates = new List<RegisterWndProcDelegate>();


        [DllImport("Y7Internal.dll", EntryPoint = "LIB_GET_MODULE_ADDRESS", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr GetModuleHandle();

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_INIT", CallingConvention = CallingConvention.Cdecl)]
        private static extern uint DELib_Init();

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_REGISTER_ATTACKER_OVERRIDE_FUNCTION", CallingConvention = CallingConvention.Cdecl)]
        private static extern void DELib_RegisterAttackerOverrideFunc(IntPtr deleg);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_DRAGONENGINE_IS_ENGINE_INITIALIZED", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        private static extern bool DELib_IsEngineInitialized();

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_DRAGONENGINE_REFRESH_OFFSETS", CallingConvention = CallingConvention.Cdecl)]
        private static extern void DELib_RefreshOffsets();

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_DRAGONENGINE_GET_DELTATIME", CallingConvention = CallingConvention.Cdecl)]
        private static extern float DELib_GetDeltaTime();

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_DRAGONENGINE_GET_FRAMERATE", CallingConvention = CallingConvention.Cdecl)]
        private static extern float DELib_GetFrameRate();

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_DRAGONENGINE_SET_SPEED", CallingConvention = CallingConvention.Cdecl)]
        private static extern void DELib_SetSpeed(DESpeedType speedType, float speed);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_GET_HUMAN_PLAYER", CallingConvention = CallingConvention.Cdecl)]
        private static extern uint DELib_GetHumanPlayer();

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_DRAGONENGINE_ALLOW_ALT_TAB_PAUSE", CallingConvention = CallingConvention.Cdecl)]
        private static extern void DELib_AllowAltTabPause(bool allow);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_DRAGONENGINE_FORCE_SET_CURSOR_VISIBLE", CallingConvention = CallingConvention.Cdecl)]
        private static extern void DELib_ForceSetCursorVisible(bool allow);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_DRAGONENGINE_IS_CURSOR_FORCED_VISIBLE", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        private static extern bool DELib_IsCursorForcedVisible();

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_DRAGONENGINE_IS_ALT_TAB_PAUSE_ALLOWED", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        private static extern bool DELib_IsAltTabPauseAllowed();

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_REGISTER_DE_JOB", CallingConvention = CallingConvention.Cdecl)]
        private static extern uint DELib_RegisterJob(IntPtr deleg, DEJob type, bool after);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_UNREGISTER_DE_JOB", CallingConvention = CallingConvention.Cdecl)]
        private static extern uint DELib_UnregisterJob(IntPtr deleg, DEJob type);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_REGISTER_WNDPROC", CallingConvention = CallingConvention.Cdecl)]
        private static extern uint DELib_RegisterWndProc(IntPtr deleg);

        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(int vKey);

        //Couldnt figure out creating a console despite calling AllocConsole
        //I'll just print to the existing console i have through a very depressing PInvoke
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_TEMP_CPP_COUT", CallingConvention = CallingConvention.Cdecl)]
        private static extern void DELib_TEMP_CPP_COUT(string text);

        public static IntPtr BaseAddress { get { return GetModuleHandle(); } }

        /// <summary>
        /// Initialize Dragon Engine library. Important for it to properly function.
        /// </summary>
        /// 

        public static void Initialize()
        {
            DELib_Init();
#if YLAD
            BattleTurnManager.OverrideAttackerSelectionInfo.deleg = new BattleTurnManager.OverrideAttackerSelectionDelegate(BattleTurnManager.ReturnManualAttackerSelectionResult);
            BattleTurnManager.OverrideAttackerSelectionInfo.delegPtr = Marshal.GetFunctionPointerForDelegate(BattleTurnManager.OverrideAttackerSelectionInfo.deleg);
            DELib_RegisterAttackerOverrideFunc(BattleTurnManager.OverrideAttackerSelectionInfo.delegPtr);
#endif

            EngineHooks.Initialize();
        }


        internal static void LibUpdate()
        {
            RefreshOffsets();
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

            Console.WriteLine(valueStr);
            // DELib_TEMP_CPP_COUT(valueStr + "\n");
            // File.AppendAllText("dotnetlog.txt", valueStr + "\n");
        }

        public static bool IsCursorForcedVisible()
        {
            return DELib_IsCursorForcedVisible();
        }

        public static void ForceSetCursorVisible(bool visible)
        {
            DELib_ForceSetCursorVisible(visible);
        }

        public static void AllowAltTabPause(bool allow)
        {
            DELib_AllowAltTabPause(allow);
        }

        public static bool IsAltTabPauseAllowed()
        {
            return DELib_IsAltTabPauseAllowed();
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

        public static void RegisterWndProc(Action<IntPtr, int, IntPtr, IntPtr> func)
        {
            RegisterWndProcDelegate del = new RegisterWndProcDelegate(func);
            DELib_RegisterWndProc(Marshal.GetFunctionPointerForDelegate(del));

            _wndprocDelegates.Add(del);
        }


        public static void RefreshOffsets()
        {
            DELib_RefreshOffsets();
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

        /// <summary>
        /// Current FPS (Frames per second) of the engine.
        /// </summary>
        public static float frameRate
        {
            get
            {
                return DELib_GetFrameRate();
            }
        }

        public static void SetSpeed(DESpeedType speedType, float speed)
        {
            DELib_SetSpeed(speedType, speed);
        }

        //Same as GetSceneEntity(SceneEntity.human_player)
        public static Character GetHumanPlayer()
        {
            return new EntityHandle<Character>(DELib_GetHumanPlayer());
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
                //It was a valid C# Dragon Engine .NET library but there was an error
                if (ex as BadImageFormatException == null)
                    Log("Failed to load library, error: " + Environment.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);

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
        public virtual string ModPath
        {
            get
            {
                return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            }
        }

        public virtual void OnModInit()
        {

        }
    }
}
