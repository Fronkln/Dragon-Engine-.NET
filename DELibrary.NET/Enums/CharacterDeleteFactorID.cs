using System;

namespace DragonEngineLibrary
{
    public enum CharacterDeleteFactorID : uint
    {
        invalid,         // constant 0x0
        unkown,      // constant 0x1
        debug,       // constant 0x2
        no_wish_live,        // constant 0x3
        npc_control,         // constant 0x4
        over_dead_body,      // constant 0x5
        over_npc_num,        // constant 0x6
        request_delete_live_npc,         // constant 0x7
        request_delete_dead_body,        // constant 0x8
        request_delete_all_fighter,      // constant 0x9
        generate_request,        // constant 0xA
        no_encount_btl,      // constant 0xB
        inside_obstacle,         // constant 0xC
        ai_escape_finish,        // constant 0xD
        ai_encounter_stay,       // constant 0xE
        no_stay_chair,       // constant 0xF
        auth_element_battle,         // constant 0x10
        obstacle_car,        // constant 0x11
        obstacle_door,       // constant 0x12
        keep_touch_collision,        // constant 0x13
        police_manager,      // constant 0x14
        long_wait_ready_near,        // constant 0x15
        no_chair,        // constant 0x16
        attachment,      // constant 0x17
        fall,        // constant 0x18
        ragdoll_needs_elimination,       // constant 0x19
        interact_other_encounter_team,       // constant 0x1A
        request_delete_all_enemy,        // constant 0x1B
    }
}
