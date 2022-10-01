using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonEngineLibrary
{
    public unsafe struct CalcDamageEventArgs
    {
        public BattleDamageInfo* info;        // this+0x0
        public void* damage_fighter_;        // this+0x8
        public bool exist_damage;       // this+0x10
    }
}
