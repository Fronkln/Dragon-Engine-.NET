using System;
using System.Runtime.InteropServices;
#if YLAD

namespace DragonEngineLibrary
{
    public static class BattleTurnManager
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_BATTLETURNMANAGER_TEST", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_BattleTurnManager_Test();

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_BATTLETURNMANAGER_SKIP_WAIT_TIME", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr DELib_BattleTurnManager_SkipWaitTime(bool readOnly, bool getNextFighter);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_BATTLETURNMANAGER_CHANGE_ACTION_STEP", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_BattleTurnManager_ChangeActionStep(ActionStep step);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_BATTLETURNMANAGER_CHANGE_PHASE", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_BattleTurnManager_ChangePhase(TurnPhase phase);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_BATTLETURNMANAGER_SWITCH_ACTIVE_FIGHTER", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_BattleTurnManager_SwitchActiveFighter(uint uid, bool no_ui_change);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_BATTLETURNMANAGER_SWITCH_TURN", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_BattleTurnManager_SwitchTurn();

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_BATTLETURNMANAGER_REQUESTRUNAWAY", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_BattleTurnManager_RequestRunAway(IntPtr fighterPtr, bool success);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_BATTLETURNMANAGER_WARPFIGHTER", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_BattleTurnManager_WarpFighter(IntPtr fighterPtr, ref PoseInfo inf);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_BATTLETURNMANAGER_RELEASE_MENU", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_BattleTurnManager_ReleaseMenu();

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_BATTLETURNMANAGER_FORCECOUNTERCOMMAND", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        internal static extern bool DELib_BattleTurnManager_ForceCounterCommand(IntPtr counterFighter, IntPtr attacker, RPGSkillID skillID);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_BATTLETURNMANAGER_GETTER_CURRENTPHASE", CallingConvention = CallingConvention.Cdecl)]
        internal static extern TurnPhase DELib_BattleTurnManager_Getter_CurrentPhase();

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_BATTLETURNMANAGER_GETTER_CURRENTACTIONSTEP", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ActionStep DELib_BattleTurnManager_Getter_CurrentActionStep();

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_BATTLETURNMANAGER_GETTER_ACTIONTYPE", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ActionType DELib_BattleTurnManager_Getter_ActionType();


        [DllImport("Y7Internal.dll", EntryPoint = "LIB_BATTLETURNMANAGER_REQ_PLAY_START_HACT", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        internal static extern bool DELib_BattleTurnManager_RequestPlayStartHact();

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_BATTLETURNMANAGER_REQUESTHACTEVENT", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        internal static extern bool DELib_BattleTurnManager_RequestHactEvent(ref HActRequestOptions option);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_BATTLETURNMANAGER_DOEXECTURNAICOMMAND", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        internal static extern bool DELib_BattleTurnManager_DoExecTurnAICommand(IntPtr fighter, IntPtr inf);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_BATTLETURNMANAGER_EXECTURNAICOMMANDDECIDE", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        internal static extern bool DELib_BattleTurnManager_ExecTurnAICommandDecide(IntPtr fighter);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_BATTLETURNMANAGER_GETTER_RPG_CAMERA", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint DELib_BattleTurnManager_Getter_RPGCamera();

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_BATTLETURNMANAGER_GETTER_UI_ROOT", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint DELib_BattleTurnManager_Getter_UI_Root();

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_BATTLETURNMANAGER_GETTER_HACT_READY_UI", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint DELib_BattleTurnManager_Getter_HAct_Ready_UI_Root();

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_BATTLETURNMANAGER_GETTER_TARGET_FIGHTER", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint DELib_BattleTurnManager_Getter_Target_Fighter();

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_BATTLETURNMANAGER_GETTER_SELECTED_FIGHTER", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint DELib_BattleTurnManager_Getter_Selected_Fighter();

        public enum TurnPhase : uint
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

        public static EntityHandle<CameraBase> RPGCamera { get { return DELib_BattleTurnManager_Getter_RPGCamera(); } }

        public static TurnPhase CurrentPhase { get { return DELib_BattleTurnManager_Getter_CurrentPhase(); } }
        public static ActionStep CurrentActionStep { get { return DELib_BattleTurnManager_Getter_CurrentActionStep(); } }
        public static ActionType CurrentActionType { get { return DELib_BattleTurnManager_Getter_ActionType(); } }

        internal delegate IntPtr OverrideAttackerSelectionDelegate(IntPtr battleTurnManager, bool readOnly, bool getNextFighter);


        public static UIHandleBase UIRoot
        {
            get
            {
                return new UIHandleBase() { Handle = DELib_BattleTurnManager_Getter_UI_Root() };
            }
        }

        public static UIHandleBase HActReadyUI
        {
            get
            {
                return new UIHandleBase() { Handle = DELib_BattleTurnManager_Getter_HAct_Ready_UI_Root() };
            }
        }

        public static EntityHandle<Character> TargetFighter
        {
            get
            {
                return new EntityHandle<Character>(DELib_BattleTurnManager_Getter_Target_Fighter());
            }
        }

        public static EntityHandle<Character> SelectedFighter
        {
            get
            {
                return new EntityHandle<Character>(DELib_BattleTurnManager_Getter_Selected_Fighter());
            }
        }

        internal static class OverrideAttackerSelectionInfo
        {
            internal static Func<bool, bool, Fighter> overrideFunc;
            internal static OverrideAttackerSelectionDelegate deleg;
            internal static IntPtr delegPtr;
        }


        internal static IntPtr ReturnManualAttackerSelectionResult(IntPtr battleTurnManager, bool readOnly, bool getNextFighter)
        {
            if (OverrideAttackerSelectionInfo.overrideFunc == null)
            {
                //no overrides, CPP library should jump to trampoline after getting 0
                return IntPtr.Zero;
            }
            else
            {
                //CPP library will return what we returned
                Fighter result = OverrideAttackerSelectionInfo.overrideFunc(readOnly, getNextFighter);
                IntPtr ptr;

                if (result == null)
                    ptr = IntPtr.Zero;
                else
                    ptr = result._ptr;

                return ptr;
            }
        }

        /// <summary>
        /// Manually decide who attacks when the game wants to pick an attacker.
        /// </summary>
        public static void OverrideAttackerSelection(Func<bool, bool, Fighter> function)
        {
            OverrideAttackerSelectionInfo.overrideFunc = function;
        }

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

        /// <summary>Removes the RPG UI.</summary>
        public static void ReleaseMenu()
        {
            DELib_BattleTurnManager_ReleaseMenu();
        }

        public static void SwitchTurn()
        {
            DELib_BattleTurnManager_SwitchTurn();
        }

        public static void SwitchActiveFighter(FighterID id, bool noUIChange)
        {
            DELib_BattleTurnManager_SwitchActiveFighter(id.Handle, noUIChange);
        }

        public static void ChangePhase(TurnPhase phase)
        {
            DELib_BattleTurnManager_ChangePhase(phase);
        }

        public static void ChangeActionStep(ActionStep step)
        {
            DELib_BattleTurnManager_ChangeActionStep(step);
        }

        public static Fighter SkipWaitTime(bool readOnly, bool getNextFighter)
        {
            IntPtr next = DELib_BattleTurnManager_SkipWaitTime(readOnly, getNextFighter);
            Fighter fighter = new Fighter(next);

            return fighter;
        }

        /// <summary>
        /// Starts a hact in queue
        /// </summary>
        /// <returns></returns>
        public static bool RequestPlayStartHact()
        {
            return DELib_BattleTurnManager_RequestPlayStartHact();
        }

        /// <summary>
        /// Request a hact to be played, barely functional? 
        /// </summary>
        public static bool RequestHActEvent(HActRequestOptions hact)
        {
            return DELib_BattleTurnManager_RequestHactEvent(ref hact);
        }


        public static bool DoExecTurnAICommand(Fighter fighter, BattleSelectCommandInfo inf)
        {
            //memory leak!
            return DELib_BattleTurnManager_DoExecTurnAICommand(fighter._ptr, inf.ToIntPtr());
        }

        public static bool ExecTurnAICommandDecide(Fighter fighter)
        {
            return DELib_BattleTurnManager_ExecTurnAICommandDecide(fighter._ptr);
        }
    }
}
#endif