using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary.Advanced
{
    public static class DXHook
    {
        [DllImport("cimgui.dll", EntryPoint = "InitDX11Hook", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Init();

        [DllImport("cimgui.dll", EntryPoint = "Register_Present_Function", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELibrary_DXHook_RegisterPresentFunc(IntPtr addr);
    }
}
