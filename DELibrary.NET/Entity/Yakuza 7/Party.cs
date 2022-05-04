using System;
using System.Runtime.InteropServices;

#if YLAD

namespace DragonEngineLibrary
{
    public static class Party
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_PARTY_GET_EQUIP_ITEM_ID", CallingConvention = CallingConvention.Cdecl)]
        public static extern ItemID GetEquipItemID(Player.ID player, PartyEquipSlotID slot);
    }
}

#endif