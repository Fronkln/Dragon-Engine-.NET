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

        [DllImport("cimgui.dll", EntryPoint = "Register_PreFirstFrame_Function", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELibrary_DXHook_RegisterPreFirstFrameFunc(IntPtr addr);

        [DllImport("cimgui.dll", EntryPoint = "Register_WndProc_Function", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELibrary_DXHook_RegisterWndProcFunc(IntPtr addr);

        [DllImport("cimgui.dll", EntryPoint = "GetGameHwnd", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr GetGameHwnd();

        [DllImport("cimgui.dll", EntryPoint = "GetFontAtlas", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr GetFontAtlas();

        [DllImport("cimgui.dll", EntryPoint = "AddFontFromMemoryTTF", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr AddFontFromMemoryTTF(IntPtr font_data, int font_size, float size_pixels, IntPtr font_cfg, IntPtr glyph_ranges);
    }
}
