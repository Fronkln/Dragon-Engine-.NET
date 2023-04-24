using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public static class SyncManager
    {

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_FIGHTER_SYNC_MANAGER_REQUEST", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_FighterSyncManager_Request(ushort set, ushort attack, uint attacker, uint victim, bool colCheck = false);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_FIGHTER_SYNC_MANAGER_GET_PLAYING_DATA", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr DELib_FighterSyncManager_GetPlayingData(int serial);

        public static void Request(FighterCommandID id, FighterID attacker, FighterID victim, bool colCheck = false)
        {
            DELib_FighterSyncManager_Request(id.set_, id.cmd, attacker.Handle, victim.Handle, colCheck);
        }

        public static IntPtr GetPlayingData(int serial)
        {
            return DELib_FighterSyncManager_GetPlayingData(serial);
        }
    }
}
