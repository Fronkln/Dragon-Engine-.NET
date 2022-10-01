using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    [StructLayout(LayoutKind.Sequential, Size = 0x2C)]
    public unsafe struct TimingInfoDamage
    {
        public uint damage;
        public int force_dead;
        public int no_dead;
        public int vanish;
        public int fatal;
        public int recover;
        public int target_sync;
        public uint attaker;
        public int direct_damage;
        public int direct_damage_is_hp_ratio;
        public int attack_id;
    };

}
