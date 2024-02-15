using System;

namespace DragonEngineLibrary
{
#if TURN_BASED_GAME
    public enum PartyEquipSlotID
    {
        weapon = 0x1,
        head = 0x2,
        body = 0x3,
        leg = 0x4,
        accessory0 = 0x5,
        accessory1 = 0x6,
    };
#endif
}
