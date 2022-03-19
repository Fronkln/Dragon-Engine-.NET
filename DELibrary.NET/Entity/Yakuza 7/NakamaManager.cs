using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public static class NakamaManager
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_NAKAMAMANAGER_CHANGE1", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_NakamaManager_Change(uint index, Player.ID playerID);

        /// <summary>
        /// Change party member at specified index to the player ID specified.
        /// </summary>
        public static void Change(uint index, Player.ID playerID)
        {
            DELib_NakamaManager_Change(index, playerID);
        }

        public static void RemoveAllPartyMembers()
        {
            for (uint i = 1; i < 4; i++)
                Change(i, Player.ID.invalid);
        }
    }
}
