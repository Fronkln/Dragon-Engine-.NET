using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public static class BattleTurnManager
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_BATTLETURNMANAGER_TEST", CallingConvention = CallingConvention.Cdecl)]
        public static extern void DELib_BattleTurnManager_Test();

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_BATTLETURNMANAGER_REQUESTRUNAWAY", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_BattleTurnManager_RequestRunAway(IntPtr fighterPtr, bool success);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_BATTLETURNMANAGER_WARPFIGHTER", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_BattleTurnManager_WarpFighter(IntPtr fighterPtr, ref PoseInfo inf);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_BATTLETURNMANAGER_FORCECOUNTERCOMMAND", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        internal static extern bool DELib_BattleTurnManager_ForceCounterCommand(IntPtr counterFighter, IntPtr attacker, RPGSkillID skillID);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_BATTLETURNMANAGER_GETTER_CURRENTPHASE", CallingConvention = CallingConvention.Cdecl)]
        internal static extern TurnPhase DELib_BattleTurnManager_Getter_CurrentPhase();

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_BATTLETURNMANAGER_GETTER_CURRENTACTIONSTEP", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ActionStep DELib_BattleTurnManager_Getter_CurrentActionStep();

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_BATTLETURNMANAGER_GETTER_ACTIONTYPE", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ActionType DELib_BattleTurnManager_Getter_ActionType();

        public enum TurnPhase
        {
            StartWait = 0x0,
            Start = 0x1,
            Action = 0x2,
            Event = 0x3,
            Cleanup = 0x4,
            End = 0x5,
            BattleResult = 0x6,
            NumPhases = 0x7,
        };

        public enum ActionStep
        {
            Init = 0x0,
            SelectCommand = 0x1,
            SelectTarget = 0x2,
            SelectCombinaiton = 0x3,
            TriggeredEvent = 0x4,
            ActionStart = 0x5,
            Ready = 0x6,
            Action = 0x7,
            ActionFinalize = 0x8,
            ActionEnd = 0x9,
            NumActionSteps = 0xA,
        };

        public enum ActionType
        {
            None = 0x0,
            Normal = 0x1,
            Interrupt = 0x2,
            Revenge = 0x3,
            Combination = 0x4,
            NumType = 0x5,
        };

        public static TurnPhase CurrentPhase { get { return DELib_BattleTurnManager_Getter_CurrentPhase(); } }
        public static ActionStep CurrentActionStep { get { return DELib_BattleTurnManager_Getter_CurrentActionStep(); } }
        public static ActionType CurrentActionType { get { return DELib_BattleTurnManager_Getter_ActionType(); } }

        /// <summary>
        /// Run away or not. Seems to end the battle either way? Weird function
        /// </summary>
        public static void RequestRunAway(Fighter fighter, bool success)
        {
            DELib_BattleTurnManager_RequestRunAway(fighter._ptr, success);
        }

        /// <summary>
        /// "Counter" an attacker with specified rpg skill.
        /// </summary>
        public static bool ForceCounterCommand(Fighter counterFighter, Fighter attacker, RPGSkillID skillID)
        {
            return DELib_BattleTurnManager_ForceCounterCommand(counterFighter._ptr, attacker._ptr, skillID);
        }

        /// <summary>
        /// Teleport the fighter to specified position.
        /// </summary>
        public static void WarpFighter(Fighter fighter, PoseInfo poseInf)
        {
            DELib_BattleTurnManager_WarpFighter(fighter._ptr, ref poseInf);
        }
    }
}
