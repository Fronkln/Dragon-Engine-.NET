using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    //This seems to be okay
    [StructLayout(LayoutKind.Explicit, Pack = 4, Size = 0x80)]
    public struct AIUtilCommandElement
    {
        [FieldOffset(0x0)] public Vector4 target_pos_;
        [FieldOffset(0x10)] public EntityUID target_uid_;
        [FieldOffset(0x20)] public EntityUID sub_target_uid_;
        [FieldOffset(0x30)] public CharacterNPCTagID target_tag_id_;
        [FieldOffset(0x34)] public CharacterNPCTagID sub_target_tag_id_;
        [FieldOffset(0x38)] public BehaviorGroupID behavior_group_;
        [FieldOffset(0x3C)] public BehaviorActionID behavior_action_;
        [FieldOffset(0x40)] public AIActGroupID ai_act_group_id_;
        [FieldOffset(0x44)] public AIActKindID ai_act_kind_id_;
        [FieldOffset(0x48)] public AIDisposeID ai_dispose_id_;
        [FieldOffset(0x4C)] public AIParamID ai_param_id_;
        [FieldOffset(0x50)] public float speed_;
        [FieldOffset(0x54)] public float behavior_speed_;
        /// <summary> EntityHandle cauth_play </summary>
        [FieldOffset(0x58)] public uint h_auth_play_;
        [FieldOffset(0x5C)] public uint util_u32_a_;
        [FieldOffset(0x60)] public uint util_u32_b_;
        [FieldOffset(0x64)] public float util_f32_a_;
        [FieldOffset(0x68)] public float util_f32_b_;
        [FieldOffset(0x6C)] public CharacterNPCBehaviorTurnActionID turn_action_id_;
        [FieldOffset(0x70)] public TriggerID trigger_id_;
        [FieldOffset(0x74)] [MarshalAs(UnmanagedType.U1)] public bool is_util_flag_a_;
        [FieldOffset(0x75)] [MarshalAs(UnmanagedType.U1)] public bool is_util_flag_b_;
        [FieldOffset(0x76)] [MarshalAs(UnmanagedType.U1)] public bool is_util_flag_c_;
        [FieldOffset(0x77)] [MarshalAs(UnmanagedType.U1)] public bool is_util_flag_d_;
        [FieldOffset(0x78)] [MarshalAs(UnmanagedType.U1)] public bool is_wish_loop_start_;
        [FieldOffset(0x79)] [MarshalAs(UnmanagedType.U1)] public bool is_wish_priority_looks_;
        [FieldOffset(0x7A)] [MarshalAs(UnmanagedType.U1)] public bool is_wish_player_mode_;
        [FieldOffset(0x7B)] [MarshalAs(UnmanagedType.U1)] public bool is_wish_avoid_;
        [FieldOffset(0x7C)] [MarshalAs(UnmanagedType.U1)] public bool is_wish_quick_reaction_;
        [FieldOffset(0x7D)] [MarshalAs(UnmanagedType.U1)] public bool is_repeat_behavior_request_;
    }


    [StructLayout(LayoutKind.Explicit, Size = 0x90)]
    public struct AIUtilCommand
    {
        public enum State : sbyte
        {
            invalid,      // constant 0x0
            request,         // constant 0x1
            receive,         // constant 0x2
            finish,      // constant 0x3
        }

        public enum Commander : sbyte
        {
            invalid,       // constant 0x0
            live,        // constant 0x1
            auth,        // constant 0x2
            dispose,         // constant 0x3
            generate,        // constant 0x4
            script,      // constant 0x5
            ai,      // constant 0x6
            scene,       // constant 0x7
            encount,         // constant 0x8
            player_control,      // constant 0x9
            debug,       // constant 0xA
        }

        [FieldOffset(0x0)] public AIUtilCommandElement element_;
        [FieldOffset(0x80)] public AIPackID pack_id_;
        [FieldOffset(0x84)] public AICommandID command_id_;
        /// <summary> EntityHandle EntityBase </summary>
        [FieldOffset(0x88)] public uint order_source_;
        [FieldOffset(0x8C)] public State state_;
        [FieldOffset(0x8D)] public Commander commander_;
        [FieldOffset(0x8E)] public sbyte priority_;
        [FieldOffset(0x8F)] [MarshalAs(UnmanagedType.U1)] public bool is_wish_save_request_;
    }
}
