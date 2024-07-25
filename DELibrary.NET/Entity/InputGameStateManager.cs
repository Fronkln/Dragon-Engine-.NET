using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public static class InputGameStateManager
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_INPUT_GAME_STATE_MANAGER_GET_CURRENT", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint GetCurrent(); //broken gog
    }
}
