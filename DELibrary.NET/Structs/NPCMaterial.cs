using System;
using System.Runtime.InteropServices;


namespace DragonEngineLibrary
{
    [StructLayout(LayoutKind.Explicit, Pack = 8, Size = 0xF0)]
    public class NPCMaterialBase
    {
        [FieldOffset(0x0)] public AIUtilCommand ai_command_;
        [FieldOffset(0x90)] public Vector4 pos_;
        [FieldOffset(0xA0)] public float rot_y_;
        [FieldOffset(0xA4)] public CharacterID character_id_;
        [FieldOffset(0xA8)] public BehaviorSetID behavior_set_id_;
        [FieldOffset(0xAC)] public uint parent_;
        [FieldOffset(0xB0)] public CharacterNPCSoldierPersonalDataID soldier_data_id_;
        [FieldOffset(0xB4)] public CharacterNPCSetup npc_setup_id_;
        [FieldOffset(0xB8)] public CharacterNPCLookParamID look_param_id_;
        [FieldOffset(0xBC)] public MissionKindID entry_mission_id_;
        [FieldOffset(0xC0)] public TalkParamID div_ai_id_;
        [FieldOffset(0xC4)] public CharacterNPCSpecialTypeID special_type_;
        [FieldOffset(0xC8)] public MapIconID map_icon_id_;
        [FieldOffset(0xCA)] public AIBtlReactionPatternID btl_reaction_pattern_id_;
        [FieldOffset(0xD0)] public uint h_life_parent_;
        [FieldOffset(0xD4)] public CharacterNPCFirstDisplayType first_display_type_id_;
        [FieldOffset(0xD8)] [MarshalAs(UnmanagedType.U1)] public byte commander;
        [FieldOffset(0xD9)] [MarshalAs(UnmanagedType.U1)] public byte exist_level_;
        [FieldOffset(0xDA)] [MarshalAs(UnmanagedType.U1)] public byte might_kind_;
        [FieldOffset(0xDB)] [MarshalAs(UnmanagedType.U1)] public byte collision_type_;
        [FieldOffset(0xDC)] [MarshalAs(UnmanagedType.U1)] public bool is_mob_;
        [FieldOffset(0xDD)] [MarshalAs(UnmanagedType.U1)] public bool is_eternal_life_;
        [FieldOffset(0xDE)] [MarshalAs(UnmanagedType.U1)] public bool is_short_life_;
        [FieldOffset(0xDF)] [MarshalAs(UnmanagedType.U1)] public bool is_encounter_;
        [FieldOffset(0xE0)] [MarshalAs(UnmanagedType.U1)] public bool is_canceler_npc_control_delete_;
        [FieldOffset(0xE1)] [MarshalAs(UnmanagedType.U1)] public bool is_wish_hide_map_icon_;
        [FieldOffset(0xE2)] [MarshalAs(UnmanagedType.U1)] public bool is_enable_proxy_;
        [FieldOffset(0xE3)] [MarshalAs(UnmanagedType.U1)] public bool is_minimum_mode_;
        [FieldOffset(0xE4)] [MarshalAs(UnmanagedType.U1)] public bool is_wish_fit_ground_;
        [FieldOffset(0xE5)] [MarshalAs(UnmanagedType.U1)] public bool is_bag_off_;
        [FieldOffset(0xE6)] [MarshalAs(UnmanagedType.U1)] public bool is_enable_replace_model_;
        [FieldOffset(0xE7)] [MarshalAs(UnmanagedType.U1)] public bool is_link_life_parent_;
        [FieldOffset(0xE8)] [MarshalAs(UnmanagedType.U1)] public bool is_external_sit_chair_;
    }


    [StructLayout(LayoutKind.Sequential)]
    public class NPCMaterial : NPCMaterialBase
    {
        public BattleRPGEnemyID enemy_id_;
        public CharacterNPCListID npc_list_id_;
        public CharacterHeightID height_scale_id_;
        public CharacterVoicerID voicer_id_;
        public DEHandleMarshal chair_asset_;
        public CharacterNPCTagID npc_tag_id_;
        public float no_damage_age_;
        public uint original_uid_serial_;
        public ushort original_uid_user_;
        [MarshalAs(UnmanagedType.U1)] public bool is_encount_btl_type_;
        [MarshalAs(UnmanagedType.U1)] public bool is_force_visible_;
        [MarshalAs(UnmanagedType.U1)] public bool is_force_create_;
        [MarshalAs(UnmanagedType.U1)] public bool is_wish_fix_uid_;
        [MarshalAs(UnmanagedType.U1)] public bool is_child_parts_;
        [MarshalAs(UnmanagedType.U1)] public bool is_transformed_entity_;
        public DEHandleMarshal h_spawn_from_;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x4)]
        private byte[] _PADDING1 = new byte[0x4];

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x10)]
        public byte[] reinforcements_;

        public CharacterNPCSoldierPersonalDataGroupID personal_data_group_id_;
        public uint nawabari_id_;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x8)]
        private byte[] _PADDING2 = new byte[0x8];
        // nawabari::e_id nawabari_id_;


        public NPCMaterial()
        {
            reinforcements_ = new byte[0x10];
        }
    }
}
