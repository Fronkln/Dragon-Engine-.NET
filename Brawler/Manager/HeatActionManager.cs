using System;
using System.Linq;
using System.Collections.Generic;
using DragonEngineLibrary;

namespace Brawler
{
    public static class HeatActionManager
    {
        public static Dictionary<AssetArmsCategoryID, Dictionary<uint, HeatAction[]>> HeatActionsList = new Dictionary<AssetArmsCategoryID, Dictionary<uint, HeatAction[]>>();
        public static List<HeatAction> PerformableHacts = new List<HeatAction>();

        private static float m_heatCd = 0;

        public static void Init()
        {
            HeatAction[] unarmedAttacks = new HeatAction[]
            {
                new HeatAction((TalkParamID)12885, HeatActionCondition.FighterDown, 5, 0, 1, 5f), //y0 brawler getup attack
                new HeatAction((TalkParamID)12892, HeatActionCondition.EnemyStandingUp, 5, 0, 1, 3.5f), //y5 enemy gets up heat attack
                new HeatAction((TalkParamID)12905, HeatActionCondition.EnemyGrabbed | HeatActionCondition.FighterHealthNotCritical, 5, 0, 1, 3.5f), //y3 shimano throw
                new HeatAction((TalkParamID)12906, HeatActionCondition.EnemyGrabbed | HeatActionCondition.FighterCriticalHealth, 5, 0, 1, 3.5f), //Custom HAct: Essence of Last Stand
               // new HeatAction((TalkParamID)12886, HeatActionCondition.EnemyDown, 2, 0, 1, 2f), //kaito tower bridge
              //  new HeatAction((TalkParamID)12895, HeatActionCondition.EnemyDown, 2, 0, 1, 2f) //wreckage
              new HeatAction((TalkParamID)12904, HeatActionCondition.EnemyDown, 2, 0, 1, 2f) //y5 downed combo
            };

            HeatAction[] wepAAttacks = new HeatAction[]
            {
                new HeatAction(TalkParamID.jh23760_buki_n, HeatActionCondition.EnemyNotDown, 1, 0, 1, 2f) //slam weapon on face
            };

            HeatAction[] wepABatonAttacks = new HeatAction[]
            {
                new HeatAction((TalkParamID)12891, HeatActionCondition.EnemyNotDown, 1, 0, 1, 2f) //bonk head with baton
            };

            HeatAction[] wepCAttacks = new HeatAction[]
{
                new HeatAction((TalkParamID)12893, HeatActionCondition.EnemyNotDown, 1, 0, 1, 2f) //slam weapon on face
};

            HeatAction[] wepDAttacks = new HeatAction[]
            {
                new HeatAction(TalkParamID.YH1440_ich_bat_atk, HeatActionCondition.EnemyNotDown | HeatActionCondition.FighterCriticalHealth, 3, 0, 1, 3.5f), //repurposed hero kiwami bat
                new HeatAction((TalkParamID)2530, HeatActionCondition.EnemyNotDown | HeatActionCondition.FighterHealthNotCritical, 1, 0, 1, 3.5f), //bat heat action
            };

            HeatAction[] wepGAttacks = new HeatAction[]
            {
                new HeatAction(TalkParamID.jh27320_buki_g_oi, HeatActionCondition.EnemyNotDown, 1, 0, 1, 2f) //knocked down hammer attack
            };

            HeatAction[] wepYAttacks = new HeatAction[]
            {
                new HeatAction((TalkParamID)12887, HeatActionCondition.EnemyNotDown, 1, 0, 1, 2f) //unload clip on enemy
            };

            //0 = default
            HeatActionsList.Add(AssetArmsCategoryID.invalid, new Dictionary<uint, HeatAction[]>() { [0] = unarmedAttacks });
            HeatActionsList.Add(AssetArmsCategoryID.A, new Dictionary<uint, HeatAction[]>() { [0] = wepAAttacks, [3] = wepABatonAttacks });
            HeatActionsList.Add(AssetArmsCategoryID.C, new Dictionary<uint, HeatAction[]>() { [0] = wepCAttacks});
            HeatActionsList.Add(AssetArmsCategoryID.D, new Dictionary<uint, HeatAction[]>() { [0] = wepDAttacks });
            HeatActionsList.Add(AssetArmsCategoryID.G, new Dictionary<uint, HeatAction[]>() { [0] = wepGAttacks });
            HeatActionsList.Add(AssetArmsCategoryID.Y, new Dictionary<uint, HeatAction[]>() { [0] = wepYAttacks });
        }

        //Return true = heat action executed
        public static bool InputUpdate(AssetArmsCategoryID currentWep, uint subCategory)
        {

            //can there be a more optimized way to check these? probably not
            //we are solely doing this for hact prompt.
            PerformableHacts = GetPerformableHacts(currentWep, subCategory);

            bool rightClickPressed = Mod.Input[BattleInput.RightMouse].LastTimeSincePressed < 0.05f;

            if (!rightClickPressed || !CanHAct())
                return false;

            ExecHeatAction(PerformableHacts[0], BrawlerBattleManager.Kasuga, BrawlerBattleManager.EnemiesNearest);
            return true; 
        }

        public static void Update()
        {
            if (m_heatCd > 0)
                m_heatCd -= DragonEngine.deltaTime;
        }

        public static bool CanHAct()
        {
            if (m_heatCd > 0)
                return false;

            if (DragonEngine.IsKeyHeld(VirtualKey.R))
                return false;

            if (!BrawlerBattleManager.Kasuga.IsValid())
                return false;

            if (BrawlerBattleManager.Kasuga.IsDead())
                return false;

            if (BrawlerBattleManager.HActIsPlaying)
                return false;

            if (PerformableHacts.Count <= 0)
                return false;

            return true;
        }

        private static List<HeatAction> GetPerformableHacts(AssetArmsCategoryID currentWep, uint subCategory)
        {
            Fighter kasuga = BrawlerBattleManager.Kasuga;
            Fighter[] validEnemies = BrawlerBattleManager.EnemiesNearest;

            List<HeatAction> hacts = new List<HeatAction>();

            if (!HeatActionsList.ContainsKey(currentWep))
                return hacts;

            if (validEnemies.Length <= 0)
                return hacts;

            float distToNearestEnemy = Vector3.Distance((Vector3)kasuga.Character.GetPosCenter(), (Vector3)validEnemies[0].Character.GetPosCenter());
            uint subCat = (HeatActionsList[currentWep].ContainsKey(subCategory) ? subCategory : 0u);

            //Possible optimization: stop if we have atleast one hact we can perform? (Due to priority of RMB)
            foreach (HeatAction act in HeatActionsList[currentWep][subCat])
            {
                if (distToNearestEnemy > act.dist || validEnemies.Length < act.numTargets)
                    continue;

                if (act.CheckConditions(kasuga, validEnemies))
                {
                    hacts.Add(act);
#if !DEBUG
                    break;
#endif
                }
            }

            return hacts;
        }

        private static void ExecHeatAction(HeatAction act, Fighter attacker, Fighter[] enemies)
        {
            Player.SetHeatNow(Player.ID.kasuga, 0);
           // attacker.Character.GetBattleStatus().Heat = 0;
            BrawlerPlayer.ExecuteMove(act, attacker, enemies);
            m_heatCd = 3.5f;
        }
    }
}
