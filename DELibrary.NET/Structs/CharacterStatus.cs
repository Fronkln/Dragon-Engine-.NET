using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    [StructLayout(LayoutKind.Sequential, Size = 0x10)]
    public struct CharacterStatusFlag
    {
        //RGGS: today i will use a 128 bit enum
        internal int i0;
        internal int i1;
        internal int i2;
        internal int i3; 
    }

    [Flags]
    public enum CharacterModeFlag
    {
        SystemPreparing = 1 << 1,
        Fighter = 1 << 2,
        Sync = 1 << 3,
        SyncOwner = 1 << 4
    }

    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct CharacterModeStatus
    {
        public CharacterModeFlag flags;
        public byte e_mode;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct CharacterStatus
    {
        IntPtr parent_;
        public float wait_ready_sec_;
        public Vector4 v_vel;
        public Vector4 pos_old;
        public Vector4 physics_displacement;
        public Vector4 ct_dynamics_tangent_velocity;
        private IntPtr ct_dynamics_handle;
        private IntPtr ct_dynamics_handle_old;
        public CharacterStatusFlag flags;
        public CharacterStatusFlag flags_old;
        public CharacterStatusFlag flags_old2;
        public CharacterStatusFlag forbid_action;
        public CharacterModeStatus mode_status_;
        public MotionBehaviorGroupActID base_behavior_;
        public MotionBehaviorGroupActID resume_behavior_;
        public float rot_y_old;
        public float spd;
        public float mot_speed;
        public float ang_spd_y;
        public float lean;
        public short last_move_ang_y;
        public short last_mot_move_ang_y;
        public short dull_ang_y;
        public short slope_ang;
        public float run_ratio;
        public float door_waiting;
        public float leap_delay_timer;
        public float crab_delay_timer;
        public float land_timer;
        public float fall_designated_wait;
        public float wall_push_timer;
        public float hit_npc_timer;
        public short hit_npc_dir;
        public short hit_wall_dir;
        public short last_wall_normal_dir;
        public short revise_ang;
        public float sleep_sec_;
        public uint fighter_index;
        public uint sametype_index;
        public MissionKindID entry_mission_id_;
        private uint lock_is_no_input_temporary_;
        public float fade_delete_interval_sec_;
        public float no_ground_sec_;
        public uint job_regist_frame_;
        public uint job_phase_;
        public CharacterDeleteFactorID delete_factor_;
        public CharacterExistLevelID exist_level_;
        public byte pressing_priority;
        public char is_no_input_temporary_;
        public char stop_npc_popup_count_;
        public bool is_need_check_equiped_hip_;
        public bool b_warp;
        public bool is_inside_stage_area_;
        public bool is_canceler_npc_control_delete_;
        public bool is_need_check_mission_area_;
        public bool is_encount_btl_type_;
        public bool is_wish_check_shoose_;
        public bool is_first_warped_;
        public bool is_wish_fade_in_;
        public bool is_fly_;
        public bool is_enable_minimum_update_invisible_;
        public bool is_minimum_update_mode_;
        public bool is_request_cancel_minimum_update_;
        public bool is_auth_game_control_mode_start_brain_;
        public bool is_manual_behavior_speed_;
        public bool is_reset_base_action_;
        public bool is_reset_base_parts_action_;
        public bool is_fake_mob_npc_control_;
        public bool is_delay_apply_battle_behavior_;
        public bool is_requested_to_transform_fighter_;
        public Vector4 base_pos_radian_;
    }
}
