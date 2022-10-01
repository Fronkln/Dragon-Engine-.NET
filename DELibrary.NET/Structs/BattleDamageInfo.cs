using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonEngineLibrary
{
    [StructLayout(LayoutKind.Sequential, Size = 0x7B0)]
    public unsafe struct BattleDamageInfo
    {

        public Matrix4x4 sync_mtx;
        public Vector4 hit_pos;
        public uint damage_parts;
        public uint attack_parts;
        public uint attacker;
        public uint asset;
        public uint type_;
        public int base_damage;
        public float fDamageRandomRatio;
        public float fHitOrderRatio;
        public float fElementDamRatio;
        public float fSkillPowRatio;
        public float fDefenceRatio;
        public float fSkillCriticalRatio;
        public float fSkillAddCriticalRatio;
        public float fItemAddCriticalRatio;
        public fixed int attr[2];
        public uint base_attack;
        public uint sp_attack;
        public uint sp_heal;
        public uint category;
        public uint base_defence;
        public bool is_guard;
        public bool is_just_guard;
        public bool is_critical;
        public bool is_LCA_input_success;
        public bool is_weak;
        public bool is_miss;
        public bool is_ex_effect;
        public bool is_use_skill_data;
        public uint skill_id;
        public uint attribute;
        public fixed int modify_param[13];
        public uint attack_id;
        public bool is_fix_critical_ratio;
        public bool is_force_death_by_skill;
        public bool is_force_vanish_by_skill;
        public bool is_invisible_ui_;
        public fixed byte gapF0[8];
        public fixed byte ex_state_list[0x18];
        public uint damage;
        public uint power;
        public uint item_id;
        public uint mot;
        public uint button_type_;
        public uint bomb_id;
        public uint bullet_id;
        public uint gun_asset;
        public fixed byte INFO_STR[0x680];
        public bool is_direct_set;
        public bool is_fix_set_attr;
        public bool is_authed;
        public bool is_hact;
        public bool is_friendly_fire;
        public bool is_tame_damage;
        public bool is_reroled_;
        public bool is_skill_last_attack_;

        public void SetAttr(uint attr)
        {

        }

        // [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x7B0)]
        // byte[] buf;
    };

}
