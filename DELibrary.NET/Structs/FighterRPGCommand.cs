using System;

namespace DragonEngineLibrary
{
    public class FighterRPGCommand
    {
        public enum CommandType
        {
            COMMAND_TYPE_NONE = 0x0,
            COMMAND_TYPE_BASIC_ATTACK = 0x1,
            COMMAND_TYPE_SKILL = 0x2,
            COMMAND_TYPE_GUARD = 0x3,
            COMMAND_TYPE_RUN_AWAY = 0x4,
            COMMAND_TYPE_NUM = 0x5,
        };
    }
}
