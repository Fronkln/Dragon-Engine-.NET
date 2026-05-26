using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public static class BattleResourceManager
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_BATTLERESOURCEMANAGER_CREATETEMPHACTCHARA", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint DELib_BattleResourceManager_CreateTempHActChara(CharacterID id, Player.ID playerID);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_BATTLERESOURCEMANAGER_FINDRPGCOMMAND", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr DELib_BattleResourceManager_FindRpgCommand(RPGSkillID skill);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_BATTLERESOURCEMANAGER_DOPREPAREREFALL", CallingConvention = CallingConvention.Cdecl)]
        [return:MarshalAs(UnmanagedType.U1)]
        public static extern bool DoPrepareRefAll(uint commandset, uint mode = 4);

        /// <summary>
        /// Create a temporary character to be used in hacts. Only one can exist at a time?
        /// </summary>
        public static EntityHandle<Character> CreateTempHActChara(CharacterID id, Player.ID playerID)
        {
            return DELib_BattleResourceManager_CreateTempHActChara(id, playerID);
        }

        public static IntPtr FindRpgCommand(RPGSkillID skill)
        {
            return DELib_BattleResourceManager_FindRpgCommand(skill);
        }
    }
}
