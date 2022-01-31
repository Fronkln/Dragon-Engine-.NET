using System;

namespace DragonEngineLibrary
{
    public enum CharacterNPCSetup : uint
    {
        invalid,         // constant 0x0
        Catch,       // constant 0x1
        catch_fix,       // constant 0x2
        cheer_layout,        // constant 0x3
        no_reaction,         // constant 0x4
        fix_soldier,         // constant 0x5
        event_encount_lynch,         // constant 0x6
        fix_talk,        // constant 0x7
        fix_talk_cheer,      // constant 0x8
        fix_talk_fear,       // constant 0x9
        fix_talk_chair,      // constant 0xA
        event_talk,      // constant 0xB
        chase_jammer,        // constant 0xC
        no_collision_cheer,      // constant 0xD
        no_collision_fear,       // constant 0xE
        air_gene_enemy,      // constant 0xF
        fix_talk_wide,       // constant 0x10
        fix_talk_cheer_wide,         // constant 0x11
        fix_talk_fear_wide,      // constant 0x12
        no_collision_chair,      // constant 0x13
        no_collision_seiza,      // constant 0x14
        no_collision_agura,      // constant 0x15
        idol_tal,        // constant 0x16
        no_collision_ever,       // constant 0x17
        no_collision,        // constant 0x18
        fix_talk_l,      // constant 0x19
        fix_simple,      // constant 0x1A
        reaction,        // constant 0x1B
        create_asset_special,        // constant 0x1C
        small_wrapping,      // constant 0x1D
        drone_race_jammer,       // constant 0x1E
        no_collision_reaction,       // constant 0x1F
        no_collision_ever_fix,       // constant 0x20
    };
}
