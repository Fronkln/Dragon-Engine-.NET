using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public class PadInputInfo
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_PADINPUTINFO_IS_TIMING_PUSH", CallingConvention = CallingConvention.Cdecl)]
        [return:MarshalAs(UnmanagedType.U1)]
        internal static extern bool DELib_PadInputInfo_IsTimingPush(IntPtr pad, uint battleButton, uint time);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_PADINPUTINFO_IS_JUST_PUSH", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        internal static extern bool DELib_PadInputInfo_IsJustPush(IntPtr pad, uint battleButton);


        [DllImport("Y7Internal.dll", EntryPoint = "LIB_PADINPUTINFO_CHECK_BUTTON", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        internal static extern bool DELib_PadInputInfo_CheckButton(IntPtr pad, uint battleButton, uint kind, uint coma, int clear_history);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_PADINPUTINFO_GETTER_LEVER_WORLD_ANG", CallingConvention = CallingConvention.Cdecl)]
        internal static extern float DELib_PadInputInfo_Getter_Lever_World_Ang(IntPtr pad);

        public IntPtr Pointer;


        public float LeverWorldAng
        {
            get
            {
                return DELib_PadInputInfo_Getter_Lever_World_Ang(Pointer);
            }
        }

        public bool IsHold(BattleButtonID battleButton)
        {
            return CheckCommand(battleButton, 1, 1000, 0)
                && !CheckCommand(battleButton, 2, 1000, 0);
        }

        public bool IsTimingPush(BattleButtonID battleButton, float time)
        {
            return DELib_PadInputInfo_IsTimingPush(Pointer, (uint)battleButton, new GameTick(time).Tick);
        }

        public bool IsTimingPush(BattleButtonID battleButton, uint time_tick)
        {
            return DELib_PadInputInfo_IsTimingPush(Pointer,(uint)battleButton, time_tick);
        }

        public bool IsJustPush(BattleButtonID battleButton)
        {
            return DELib_PadInputInfo_IsJustPush(Pointer, (uint)battleButton);
        }

        public bool CheckCommand(BattleButtonID id, uint kind, uint coma = 0, int clear_history = 0)
        {
            return DELib_PadInputInfo_CheckButton(Pointer, (uint)id, kind, coma, clear_history);
        }
    }
}
