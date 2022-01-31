using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public static class Player
    {
        public enum ID
        {
            invalid,         // constant 0x0
            kiryu,       // constant 0x1
#if !(Y6)
            yagami,      // constant 0x2
            majima,      // constant 0x3
#endif
#if YLAD
            kasuga,      // constant 0x4
            sakaida,         // constant 0x5
            adachi,      // constant 0x6
            hoshino,         // constant 0x7
            test_player,         // constant 0x8
            saeko,       // constant 0x9
            nanba,       // constant 0xA
            ayaka,       // constant 0xB
            chou,        // constant 0xC
            jyungi,      // constant 0xD
            woman_a,		 // constant 0xE
#endif
        }

        //[DllImport("Y7Internal.dll", EntryPoint = "PLAYER_SET_LEVEL", CallingConvention = CallingConvention.Cdecl)]
        //public extern static void SetLevel(uint level, ID playerID);
    }
}
