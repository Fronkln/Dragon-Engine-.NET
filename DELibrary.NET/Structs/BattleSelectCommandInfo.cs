using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    [StructLayout(LayoutKind.Explicit, Pack = 8, Size = 0x10)]
    public struct FighterCommandRef
    {
        [FieldOffset(0x0)] public FighterRPGCommand.CommandType command_type;
        [FieldOffset(0x4)] public RPGSkillID command;
        [FieldOffset(0x8)] public ItemID use_item_id;
    };

    [StructLayout(LayoutKind.Explicit, Pack = 4, Size = 0x20)]
    public struct BattleSelectCommandInfo
    {
        [FieldOffset(0x0)] public FighterCommandRef command;
        [FieldOffset(0x10)] public uint target_fighter;
        [FieldOffset(0x14)] public Player.ID target_player_id;
        /// <summary>
        /// EntityHandle AssetUnit
        /// </summary>
        [FieldOffset(0x18)] public uint target_asset;
        [FieldOffset(0x1C)] public bool is_skip;
        [FieldOffset(0x1D)] public bool is_wait;
    };
}
