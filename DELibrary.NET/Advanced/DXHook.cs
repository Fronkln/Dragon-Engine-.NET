using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary.Advanced
{
    public static class DXHook
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_DXHOOK_DO_HOOK", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELibrary_DXHook_DoHook();

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_DXHOOK_GET_WINDOW", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr DELibrary_DXHook_GetWindow();

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_DXHOOK_GET_DEVICE", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr DELibrary_DXHook_GetDevice();

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_DXHOOK_GET_DEVICE_CONTEXT", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr DELibrary_DXHook_GetDeviceContext();

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_DXHOOK_INIT_IMGUI", CallingConvention = CallingConvention.Cdecl)]
        [return:MarshalAs(UnmanagedType.I1)]
        internal static extern bool DELibrary_DXHook_InitImGui();

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_DXHOOK_SET_WANT_HOOK", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELibrary_DXHook_SetWantHook();

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_DXHOOK_GET_WANT_HOOK", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool DELibrary_DXHook_GetWantHook();

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_DXHOOK_REGISTER_PRESENT_FUNC", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool DELibrary_DXHook_RegisterPresentFunc(IntPtr deleg);

        public static void DoHook()
        {
            DELibrary_DXHook_DoHook();
        }

        public static IntPtr GetWindow()
        {
            return DELibrary_DXHook_GetWindow();
        }

        public static IntPtr GetDevice()
        {
            return DELibrary_DXHook_GetDevice();
        }

        public static IntPtr GetDeviceContext()
        {
            return GetDeviceContext();
        }

        public static bool InitImGui()
        {
            return DELibrary_DXHook_InitImGui();
        }

        public static bool IsWantHook()
        {
            return DELibrary_DXHook_GetWantHook();
        }
    }
}
