﻿namespace DragonEngineLibrary
{
#if IW_AND_UP
    public enum HActRangeType
    {
        invalid,         // constant 0x0
        mukou,
        hit_wall,
        hit_wall_safety,
        hit_wall_force,
        dropped_throw,
        pole,
        guardrail,
        corner,
        stand,
        high_range,
        low_range,
        stairs_up,
        stairs_down,
        oven,
        stairs,
        plain,
        train_throw,
        special,
        warp_safety,
        warp_safety_slope,
        warp_force,
        water_side,
        water_high_side,
        door_left,
        door_right,
        battle_space,
        battle_result,
    }
#else
    public enum HActRangeType
    {
        invalid,         // constant 0x0
        hit_wall,        // constant 0x1
        dropped_throw,       // constant 0x2
        Invalid,         // constant 0x3
        mukou,       // constant 0x4
        pole,        // constant 0x5
        guardrail,       // constant 0x6
        corner,      // constant 0x7
        stand,       // constant 0x8
        high_range,      // constant 0x9
        low_range,       // constant 0xA
        dropped_down,        // constant 0xB
        oven,        // constant 0xC
        stairs,      // constant 0xD
        plain,       // constant 0xE
        train_throw,         // constant 0xF
        special,         // constant 0x10
        warp_safety,         // constant 0x11
        warp_force,      // constant 0x12
        stairs_up,       // constant 0x13
        stairs_down,         // constant 0x14
        water_side,      // constant 0x15
        water_high_side,         // constant 0x16
        door_left,       // constant 0x17
        door_right,      // constant 0x18
        warp_safety_slope,       // constant 0x19
        hit_wall_safety,         // constant 0x1A
        battle_space,        // constant 0x1B
        battle_result,       // constant 0x1C
    }
#endif
}