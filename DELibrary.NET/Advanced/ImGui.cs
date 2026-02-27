using System;
using System.Threading;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.IO;


//Advanced because the mere use of this can introduce decent amount of bugs
namespace DragonEngineLibrary.Advanced
{
    public static class ImGui
    {
        internal delegate void DX11Present();
        private static List<DX11Present> _dx11Delegates = new List<DX11Present>();

        public static bool toInit = false;

        public static void RegisterUIUpdate(Action func)
        {
            DX11Present del = new DX11Present(func);
            _dx11Delegates.Add(del);

            DXHook.DELibrary_DXHook_RegisterPresentFunc(Marshal.GetFunctionPointerForDelegate(del));
        }

        /// <summary>
        /// Register a callback that fires after ImGui context creation but before the first NewFrame.
        /// Font atlas is unlocked at this point - add custom fonts here.
        /// </summary>
        public static void RegisterPreFirstFrame(Action func)
        {
            DX11Present del = new DX11Present(func);
            _dx11Delegates.Add(del);

            DXHook.DELibrary_DXHook_RegisterPreFirstFrameFunc(Marshal.GetFunctionPointerForDelegate(del));
        }

        internal delegate void WndProcDelegate(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam);
        private static List<WndProcDelegate> _wndProcDelegates = new List<WndProcDelegate>();

        /// <summary>
        /// Register a WndProc callback through cimgui's window subclass.
        /// The DX11 hook subclasses the game window and forwards messages here.
        /// ImGui_ImplWin32_WndProcHandler is called automatically before these callbacks.
        /// </summary>
        public static void RegisterWndProc(Action<IntPtr, int, IntPtr, IntPtr> func)
        {
            WndProcDelegate del = new WndProcDelegate(func);
            _wndProcDelegates.Add(del);

            DXHook.DELibrary_DXHook_RegisterWndProcFunc(Marshal.GetFunctionPointerForDelegate(del));
        }

        public static void Init()
        {
            string libPath = Path.Combine(Library.Root, "Y7Internal.dll");
            string cimguiPath = Path.Combine(new FileInfo(libPath).Directory.FullName, "cimgui.dll");

            if (File.Exists(cimguiPath))
                DragonEngine.LoadLibrary(cimguiPath);

#if !IW_AND_UP
            DXHook.Init();
#endif
        }
    }
}
