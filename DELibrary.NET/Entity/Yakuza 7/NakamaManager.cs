using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public static class NakamaManager
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_NAKAMAMANAGER_CHANGE1", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_NakamaManager_Change(uint index, Player.ID playerID);
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_NAKAMAMANAGER_FINDINDEX1", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int DELib_NakamaManager_FindIndex1(Player.ID playerID);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_NAKAMAMANAGER_GET_CHARACTER_HANDLE", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint DELib_NakamaManager_GetCharacterHandle(uint index);

        /// <summary>
        /// Change party member at specified index to the player ID specified.
        /// </summary>
        public static void Change(int index, Player.ID playerID)
        {
            DELib_NakamaManager_Change((uint)index, playerID);
        }

        public static int FindIndex(Player.ID id)
        {
            return DELib_NakamaManager_FindIndex1(id);
        }

        public static EntityHandle<Character> GetCharacterHandle(uint index)
        {
            return DELib_NakamaManager_GetCharacterHandle(index);
        }

        public static void RemoveAllPartyMembers()
        {
            for (int i = 1; i < 4; i++)
                Change(i, Player.ID.invalid);
        }
    }
}
