using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    [StructLayout(LayoutKind.Explicit, Pack = 16)]
    public struct BattleFighterInfo
    {
        [FieldOffset(0x0)] public DynamicsPoseInfo pose_;
        [FieldOffset(0x50)] public MotionTimingStatus timing_;
        [FieldOffset(0x1A0)] public MotionID gmt_;
        [FieldOffset(0x1A4)] public uint frame_;
        [FieldOffset(0x1A8)] public bool is_ragdoll_;
        [FieldOffset(0x1A9)] public bool is_ragdoll_dead_;
        [FieldOffset(0x1AA)] public bool is_damage_;
        [FieldOffset(0x1AB)] public bool is_down_;
        [FieldOffset(0x1AC)] public bool in_inertial_motion;
        [FieldOffset(0x1AD)] public bool is_dead_;
        [FieldOffset(0x1AE)] public bool is_sway_;
        [FieldOffset(0x1B0)] public bool is_fly_damage_;
        [FieldOffset(0x1B1)] public bool is_stand_up_;
        [FieldOffset(0x1B2)] public bool is_ready_;
        [FieldOffset(0x1B3)] public bool is_run_end_;
        [FieldOffset(0x1B4)] public bool is_guard_;
        [FieldOffset(0x1B5)] public bool is_guard_reaction_;
        [FieldOffset(0x1B6)] public bool is_sync_;
        [FieldOffset(0x1B7)] public bool is_break_action_;
        [FieldOffset(0x1B8)] public bool is_hide_seek_;
    }
}
