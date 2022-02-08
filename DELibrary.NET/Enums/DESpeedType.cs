using System;

namespace DragonEngineLibrary
{
    /// <summary>
    /// Used when setting the game's type (have characters be at 0.5 speed etc)
    /// </summary>
    public enum DESpeedType : sbyte
    {
        General,		 // constant 0x0
        Player,		 // constant 0x1
        Character,		 // constant 0x2
        Camera,		 // constant 0x3
        UI,		 // constant 0x4
        Unprocessed,		 // constant 0x5
        Count		 // constant 0x6
    };
}
