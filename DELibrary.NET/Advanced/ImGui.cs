using System;
using System.Threading;
using System.Collections.Generic;
using System.Runtime.InteropServices;


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

        public static void Init()
        {
            DXHook.DELibrary_DXHook_InitImGui();
        }
    }
}
