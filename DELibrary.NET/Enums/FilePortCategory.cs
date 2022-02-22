using System;

namespace DragonEngineLibrary
{
    public enum FilePortCategory : uint
    {
        invalid,         // constant 0x0
        high_auth,       // constant 0x1
        high_auth_preload,       // constant 0x2
        auth_div_resource,       // constant 0x3
        lua,         // constant 0x4
        db,      // constant 0x5
        ui,      // constant 0x6
        entity,      // constant 0x7
        high_character,      // constant 0x8
        high_sound,      // constant 0x9
        high_effect,         // constant 0xA
        high_motion,         // constant 0xB
        equip_asset,         // constant 0xC
        lodtx_ast_new,       // constant 0xD
        lodtx_stg_new,       // constant 0xE
        stage_lod_low,       // constant 0xF
        lodtx_stg_down_d,        // constant 0x10
        lodtx_stg_org_d,         // constant 0x11
        lodtx_stg_cv2_d,         // constant 0x12
        motion,      // constant 0x13
        stage,       // constant 0x14
        character,       // constant 0x15
        character_dispose,       // constant 0x16
        asset,       // constant 0x17
        effect,      // constant 0x18
        stage_lod_high,      // constant 0x19
        lodtx_stg_down_e,        // constant 0x1A
        lodtx_stg_org_e,         // constant 0x1B
        lodtx_stg_cv2_e,         // constant 0x1C
        sound,       // constant 0x1D
        auth,        // constant 0x1E
        auth_preload,        // constant 0x1F
        character_npc,       // constant 0x20
        train,       // constant 0x21
        lodtex_invalid,      // constant 0x22
        scene_wait,      // constant 0x23
        num,		 // constant 0x24
    }
}
