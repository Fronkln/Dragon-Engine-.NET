using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    [StructLayout(LayoutKind.Explicit)]
    public struct CDisposeMaterial
    {
        [FieldOffset(0x0)]
        public Vector4 l_offset_v;
        [FieldOffset(0x10)]
        public Quaternion l_offset_orient;
        [FieldOffset(0x20)]
        public EntityUID uid;
        [FieldOffset(0x30)]
        public AssetID asset_id;
        [FieldOffset(0x34)]
        public CharacterID character_id;
        [FieldOffset(0x38)]
        public AttachmentNodeID parent_node_id;
        [FieldOffset(0x3C)]
        public AttachmentNodeID child_node_id;
        [FieldOffset(0x40)]
        public AttachmentCombinationID combination_id;
        [FieldOffset(0x44)]
        public AttachmentEquipTemplateID template_id;
        [FieldOffset(0x48)]
        public bool is_use_manual_offset;
        [FieldOffset(0x49)]
        public bool is_adv_mode;
    }

    [StructLayout(LayoutKind.Sequential, Size = 0x1F0)]
    public class CharacterCDispose
    {
        //byte array until i figure how to marshal this ungodly class
        public byte[] padding = new byte[0x1F0];
    }

    [StructLayout(LayoutKind.Explicit)]
    public unsafe struct CharacterAttributes
    {
        [FieldOffset(0x0)] public Vector4 InitPos;
        [FieldOffset(0x10)] [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x1F0)] public byte[] dispose_data;
        [FieldOffset(0x200)] public CharacterID chara_id;  
        [FieldOffset(0x204)] public CharacterModelID model_id;
        [FieldOffset(0x208)] public CharacterBoneType bone_type;
        [FieldOffset(0x20C)] public CharacterAnimalKind animal_kind;
        [FieldOffset(0x210)] public CharacterBagType bag_type;
        [FieldOffset(0x214)] public BattleControlType ctrl_type;
        [FieldOffset(0x218)] public BattleRPGEnemyID enemy_id;
        [FieldOffset(0x21C)] public BattleCommandSetID command_set_id;
        [FieldOffset(0x220)] public CharacterNPCSetup npc_setup;
        [FieldOffset(0x224)] public BehaviorSetID behavior_set;
        [FieldOffset(0x228)] public SujimonID sujimon_id;
        [FieldOffset(0x22C)] public BehaviorGroupID behavior_group;
        [FieldOffset(0x230)] public BehaviorActionID behavior_action;
        [FieldOffset(0x234)] public CharacterHeightID chara_scale_id;
        [FieldOffset(0x238)] public MotionFacePattern base_face_pattern;
        [FieldOffset(0x23C)] public int flags;
        [FieldOffset(0x240)] public float scale_rate;
        [FieldOffset(0x244)] public float scale_rate_face;
        [FieldOffset(0x248)] public float auto_wrinkle_scale;
        
        [FieldOffset(0x24C)] public CharacterNPCListID npc_list_id;
        [FieldOffset(0x250)] public CharacterNPCSoldierPersonalDataID soldier_data_id;
        [FieldOffset(0x254)] public CharacterVoicerID voicer_id;
        /// <summary> https://youtu.be/OQYb-3n0tBk?t=10 </summary>
        [FieldOffset(0x258)] public SoundShoeKind shoes_kind;
        [FieldOffset(0x25C)] public MissionKindID first_entry_mission_id;
        [FieldOffset(0x260)] public CharacterNPCTagID tag_id;
        [FieldOffset(0x264)] public CharacterNPCSpecialTypeID special_type;
        [FieldOffset(0x268)] public CabaretIslandCastID caba_cast_id;
        [FieldOffset(0x26C)] public CharacterClassID model_class_id;
        [FieldOffset(0x270)] [MarshalAs(UnmanagedType.U1)] public bool fix_model_lod;
        [FieldOffset(0x271)] public byte commander;
        [FieldOffset(0x272)] public byte exist_level;
        [FieldOffset(0x273)] public byte model_category_id;
        [FieldOffset(0x274)] public byte model_age_id;
        [FieldOffset(0x275)] public byte model_body_type_id;
        [FieldOffset(0x276)] public byte proxy_type;
        [FieldOffset(0x277)] [MarshalAs(UnmanagedType.U1)] public bool is_player;
        [FieldOffset(0x278)] [MarshalAs(UnmanagedType.U1)] public bool is_supporter;
        [FieldOffset(0x279)] [MarshalAs(UnmanagedType.U1)] public bool is_npc;
        [FieldOffset(0x27A)] [MarshalAs(UnmanagedType.U1)] public bool is_soldier;
        [FieldOffset(0x27B)] [MarshalAs(UnmanagedType.U1)] public bool is_encounter;
        [FieldOffset(0x27C)] [MarshalAs(UnmanagedType.U1)] public bool is_exec_physics_ragdoll;
        [FieldOffset(0x27D)] [MarshalAs(UnmanagedType.U1)] public bool is_mob;
        [FieldOffset(0x27E)] [MarshalAs(UnmanagedType.U1)] public bool is_process_in_pause;
        [FieldOffset(0x27F)] [MarshalAs(UnmanagedType.U1)] public bool is_enable_chara_proxy;
        [FieldOffset(0x280)] [MarshalAs(UnmanagedType.U1)] public bool is_ever_fix_chara_proxy;
        [FieldOffset(0x281)] [MarshalAs(UnmanagedType.U1)] public bool is_disable_scale;
        [FieldOffset(0x282)] [MarshalAs(UnmanagedType.U1)] public bool is_adv_model;
        [FieldOffset(0x283)] [MarshalAs(UnmanagedType.U1)] public bool is_encount_btl_type;
        [FieldOffset(0x284)] [MarshalAs(UnmanagedType.U1)] public bool is_force_update_motion;
        [FieldOffset(0x285)] [MarshalAs(UnmanagedType.U1)] public bool is_shoes_off;
        [FieldOffset(0x286)] [MarshalAs(UnmanagedType.U1)] public bool is_force_visible;
        [FieldOffset(0x287)] [MarshalAs(UnmanagedType.U1)] public bool is_minimum_mode;
        [FieldOffset(0x288)] [MarshalAs(UnmanagedType.U1)] public bool is_use_human_sensor;
        [FieldOffset(0x289)] [MarshalAs(UnmanagedType.U1)] public bool is_keep_dead;
        [FieldOffset(0x28A)] [MarshalAs(UnmanagedType.U1)] public bool is_wish_auto_invisible;
        [FieldOffset(0x28C)] public Player.ID player_id;
        [FieldOffset(0x290)] public Player.ID dmy_player_id;
        [FieldOffset(0x294)] public FilePortCategory file_port;
        
    }
}
