using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonEngineLibrary
{
    public static class EffectEventManager
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CEFFECT_EVENT_MANAGER_LOAD_SCREEN", CallingConvention = CallingConvention.Cdecl)]
        public static extern void LoadScreen(uint id);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CEFFECT_EVENT_MANAGER_PLAY_SCREEN", CallingConvention = CallingConvention.Cdecl)]
        public static extern void PlayScreen(uint id, bool loop = false, bool fade = false, float fadeSpeed = 1, bool ignoreSpeed = false);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CEFFECT_EVENT_MANAGER_STOP_SCREEN", CallingConvention = CallingConvention.Cdecl)]
        public static extern void StopScreen(uint id, bool immediate = true);
    }
}
