using System;
using System.Runtime.InteropServices;

#if TURN_BASED_GAME

namespace DragonEngineLibrary
{
    public static class Party
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_PARTY_GET_EQUIP_ITEM_ID", CallingConvention = CallingConvention.Cdecl)]
        public static extern ItemID GetEquipItemID(Player.ID player, PartyEquipSlotID slot);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_PARTY_GET_MAIN_MEMBER_NUM", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetMainMemberCount();
    }
}

#endif