using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public static class BattleManager
    {
        [DllImport("Y7Internal.dll", EntryPoint = "CBATTLE_MANAGER_IS_TIMING_PUSH", CallingConvention = CallingConvention.Cdecl)]
        [return:MarshalAs(UnmanagedType.U1)]
        internal static extern bool DELib_BattleManager_IsTimingPush(uint battleButton, uint tick);

        public static bool IsTimingPush(uint battleButton, GameTick tick)
        {
            return DELib_BattleManager_IsTimingPush(battleButton, tick.Tick);
        }
    }
}
