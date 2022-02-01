using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public static class BattleTurnManager
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_BATTLETURNMANAGER_REQUESTRUNAWAY", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_BattleTurnManager_RequestRunAway(IntPtr fighterPtr, bool success);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_BATTLETURNMANAGER_FORCECOUNTERCOMMAND", CallingConvention = CallingConvention.Cdecl)]
        [return:MarshalAs(UnmanagedType.U1)]
        internal static extern bool DELib_BattleTurnManager_ForceCounterCommand(IntPtr counterFighter, IntPtr attacker, RPGSkillID skillID);

        public static void RequestRunAway(Fighter fighter, bool success)
        {
            DELib_BattleTurnManager_RequestRunAway(fighter._ptr, success);
        }

        public static bool ForceCounterCommand(Fighter counterFighter, Fighter attacker, RPGSkillID skillID)
        {
            return DELib_BattleTurnManager_ForceCounterCommand(counterFighter._ptr, attacker._ptr, skillID);
        }
    }
}
