using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonEngineLibrary
{
    public enum ArmsType : uint
    {
        invalid = 0,
        single = 1,
        single_bullet = 2,
        single_live_bullet = 3,
        single_spray = 4,
        dual_main = 5,
        dual_main_nunchuck = 6,
        dual_main_bullet = 7,
        multi_sub = 8,
        multi_sub_bullet = 9
    }
}
