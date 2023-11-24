using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary.Advanced
{
    public static class DXHook
    {
        [DllImport("mods/DE Library/cimgui.dll", EntryPoint = "OOELib_InitDX11Hook", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Init();

        [DllImport("mods/DE Library/cimgui.dll", EntryPoint = "OOELib_Register_Present_Function", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELibrary_DXHook_RegisterPresentFunc(IntPtr addr);
    }
}
