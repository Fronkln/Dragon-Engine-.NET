using System;
using DragonEngineLibrary;
using Newtonsoft.Json;

namespace Brawler
{
    public enum BattleInput
    {
        LeftMouse,
        RightMouse,
        MiddleMouse,
        LeftShift,
        Space,
        E,
        Q,
        F,
        T
    }

    public struct MoveInput
    {
        [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public BattleInput Key;
        public bool Hold;

        public MoveInput(BattleInput key, bool hold)
        {
            Key = key;
            Hold = hold;
        }
    }
}
