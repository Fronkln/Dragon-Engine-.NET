using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    //Its normally not a static class
    //But only one of it can exist once
    //Therefore, it makes sense that we make it static
    public static class FighterManager
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_FIGHTER_MANAGER_GET_FIGHTER_INFO", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr DELib_FighterManager_GetFighterInfo(uint fighterIndex);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_FIGHTER_MANAGER_REQUESTREGISTRATIONFIGHTER", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_FighterManager_RequestRegistrationFighter(uint charaUID, BattleGroupID id);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_FIGHTER_MANAGER_REGISTRATIONFIGHTER", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_FighterManager_RegistrationFighter(uint charaUID, BattleGroupID id);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_FIGHTER_MANAGER_GENERATEENEMYFIGHTER", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint DELib_FighterManager_GenerateEnemyFighter(ref PoseInfo poseAddr, uint soldier_id, CharacterID appearance);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_FIGHTER_MANAGER_GET_PLAYER", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr DELib_FighterManager_GetPlayer();

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_FIGHTER_MANAGER_GET_FIGHTER", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr DELib_FighterManager_GetFighter(uint index);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_FIGHTER_MANAGER_GETNEARESTENEMY", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr DELib_FighterManager_GetNearestEnemy(IntPtr fighterID);


        [DllImport("Y7Internal.dll", EntryPoint = "LIB_Y7_EXCLUSIVE_FIGHTER_MANAGER_FORCEBRAWLERMODE", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_FighterManager_Y7_ForceBrawlerMode(bool brawler);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_Y7_EXCLUSIVE_FIGHTER_MANAGER_IS_BRAWLER", CallingConvention = CallingConvention.Cdecl)]
        [return:MarshalAs(UnmanagedType.U1)]
        internal static extern bool DELib_FighterManager_Y7_IsBrawlerMode();

        /// <summary>
        /// Create an "enemy" fighter, appearance may only work after transformation is enabled, needs testing.
        /// </summary>
        public static EntityHandle<Character> GenerateEnemyFighter(PoseInfo pose, uint soldier_id, CharacterID appearance)
        {
            EntityHandle<Character> generatedEnemy = DELib_FighterManager_GenerateEnemyFighter(ref pose, soldier_id, appearance);
            return generatedEnemy;
        }

        /// <summary>
        /// Return the fighter from specified index.
        /// </summary>
        public static Fighter GetFighter(uint index)
        {
            return new Fighter(DELib_FighterManager_GetFighter(index));
        }

        /// <summary>
        /// Get the player fighter.
        /// </summary>
        public static Fighter GetPlayer()
        {
            return GetFighter(0);
        }

        public static Fighter[] GetAllFighters()
        {
            List<Fighter> enemyFighters = new List<Fighter>();

            //heuteristic, may not be accurate on extreme cases
            for (uint i = 0; i < 48; i++)
            {
                Fighter fighter = GetFighter(i);

                if (fighter.Character.IsValid())
                        enemyFighters.Add(fighter);
            }

            return enemyFighters.ToArray();
        }

        public static int GetCurrentFighterIdx()
        {
            int highestIdx = 0;

            for(uint i = 0; i < 48; i++)
            {
                if (GetFighter(i).IsValid())
                    highestIdx = (int)i;
            }

            return highestIdx;
        }

        /// <summary>
        /// Get all enemies.
        /// </summary>
        public static Fighter[] GetAllEnemies()
        {
            List<Fighter> enemyFighters = new List<Fighter>();

            //heuteristic, may not be accurate on extreme cases
            for (uint i = 3; i < 48; i++)
            {
                Fighter fighter = GetFighter(i);

                if (fighter.Character.IsValid())
                    if (fighter.IsEnemy())
                        enemyFighters.Add(fighter);
            }

            return enemyFighters.ToArray();
        }

        public static BattleFighterInfo GetFighterInfo(uint fighterIndex)
        {
            IntPtr inf = DELib_FighterManager_GetFighterInfo(fighterIndex);

            if (inf == IntPtr.Zero)
                return new BattleFighterInfo();
            else
                return Marshal.PtrToStructure<BattleFighterInfo>(inf);
        }

        /// <summary>
        /// Request a character to be registered as a fighter.
        /// </summary>
        public static void RequestRegistrationFighter(EntityHandle<Character> chara, BattleGroupID group_id)
        {
            DELib_FighterManager_RequestRegistrationFighter(chara.UID, group_id);
        }

        /// <summary>
        /// Use RequestRegistrationFighter instead
        /// </summary>
        public static void RegistrationFighter(EntityHandle<Character> chara, BattleGroupID group_id)
        {
            DELib_FighterManager_RegistrationFighter(chara.UID, group_id);
        }

        public static void ForceBrawlerMode(bool brawler)
        {
            DELib_FighterManager_Y7_ForceBrawlerMode(brawler);
        }

        public static bool IsBrawlerMode()
        {
            return DELib_FighterManager_Y7_IsBrawlerMode();
        }

        /*
        public static Fighter GetNearestEnemy(FighterID id)
        {
            GCHandle idHandle = id.ToGcHandle();

            IntPtr nearest = DELib_FighterManager_GetNearestEnemy((IntPtr)idHandle);
            idHandle.Free();
            DragonEngine.Log(nearest.ToString() + " da near");

            return new Fighter(nearest);
        }
        */
    }
}
