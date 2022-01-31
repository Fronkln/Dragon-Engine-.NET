using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    //Its normally not a static class
    //But only one of it can exist once
    //Therefore, it makes sense that we make it static
    public static class FighterManager
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_FIGHTER_MANAGER_GENERATEENEMYFIGHTER", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint DELib_FighterManager_GenerateEnemyFighter(ref PoseInfo poseAddr, uint soldier_id, CharacterID appearance);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_FIGHTER_MANAGER_GET_PLAYER", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr DELib_FighterManager_GetPlayer();

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_FIGHTER_MANAGER_GET_FIGHTER", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr DELib_FighterManager_GetFighter(uint index);

        /// <summary>
        /// Create an "enemy" fighter, appearance may only work after transformation is enabled, needs testing.
        /// </summary>
        public static EntityHandle<Character> GenerateEnemyFighter(PoseInfo pose, uint soldier_id, CharacterID appearance)
        {
            EntityHandle<Character> generatedEnemy = DELib_FighterManager_GenerateEnemyFighter(ref pose, soldier_id, appearance);
            return generatedEnemy;
        }

        public static Fighter GetFighter(uint index)
        {
            return new Fighter(DELib_FighterManager_GetFighter(index));
        }

        public static Fighter GetPlayer()
        {
            return GetFighter(0);
        }
    }
}
