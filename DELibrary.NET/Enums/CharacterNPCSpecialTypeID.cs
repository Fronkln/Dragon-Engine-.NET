using System;

namespace DragonEngineLibrary
{
    public enum CharacterNPCSpecialTypeID : uint
    {
        invalid,         // constant 0x0
        encount_boss,        // constant 0x1
        hact_gal,        // constant 0x2
        hact_convenience_staff,      // constant 0x3
        stalking_target,         // constant 0x4
        chase_target,        // constant 0x5
        Friend,      // constant 0x6
        henchman_encounter,      // constant 0x7
        fan,         // constant 0x8
        police,      // constant 0x9
        supporter,       // constant 0xA
        stalking_target_supporter,       // constant 0xB
        popup_ok,        // constant 0xC
    };
}
