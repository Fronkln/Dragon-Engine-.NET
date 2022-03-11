using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary.Advanced
{
    public static class DXHook
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_IMGUI_INIT", CallingConvention = CallingConvention.Cdecl)]
        [return:MarshalAs(UnmanagedType.I1)]
        internal static extern bool DELibrary_DXHook_InitImGui();


        [DllImport("Y7Internal.dll", EntryPoint = "LIB_DXHOOK_REGISTER_PRESENT_FUNC", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern void DELibrary_DXHook_RegisterPresentFunc(IntPtr addr);
    }
}
