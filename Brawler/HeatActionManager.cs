using System;
using System.Linq;
using System.Collections.Generic;
using DragonEngineLibrary;

namespace Brawler
{
    public static class HeatActionManager
    {
        public static Dictionary<AssetArmsCategoryID, HeatAction[]> HeatActionsList = new Dictionary<AssetArmsCategoryID, HeatAction[]>();


        public static void Init()
        {
            HeatAction[] unarmedAttacks = new HeatAction[]
            {
                new HeatAction((TalkParamID)12884, HeatActionCondition.EnemyDown, 1, 0, 1, 2f) //kenzan slap
            };

            HeatAction[] wepAAttacks = new HeatAction[]
            {
                new HeatAction(TalkParamID.jh23760_buki_n, HeatActionCondition.None, 1, 0, 1, 2f) //slam weapon on face
            };

            HeatAction[] wepGAttacks = new HeatAction[]
{
                new HeatAction(TalkParamID.jh27320_buki_g_oi, HeatActionCondition.None, 1, 0, 1, 2f) //knocked down hammer attack
};

            HeatActionsList[AssetArmsCategoryID.invalid] = unarmedAttacks;
            HeatActionsList[AssetArmsCategoryID.A] = wepAAttacks;
            HeatActionsList[AssetArmsCategoryID.G] = wepGAttacks;
        }

        //Return true = heat action executed
        public static bool InputUpdate(AssetArmsCategoryID currentWep)
        {
            bool rightClickPressed = DragonEngine.IsKeyDown(VirtualKey.RightButton);

            if (!HeatActionsList.ContainsKey(currentWep))
                return false;

            if (!rightClickPressed)
                return false;

            if (HActManager.IsPlaying())
                return false;

            Fighter kasuga = FighterManager.GetFighter(0);

            Fighter[] validEnemies = FighterManager.GetAllEnemies().Where(x => !x.IsDead()).OrderBy(x => Vector3.Distance((Vector3)kasuga.Character.Transform.Position, (Vector3)x.Character.Transform.Position)).ToArray();

            if (validEnemies.Length <= 0)
                return false;


            float distToNearestEnemy = Vector3.Distance((Vector3)kasuga.Character.GetPosCenter(), (Vector3)validEnemies[0].Character.GetPosCenter());

            foreach(HeatAction act in HeatActionsList[currentWep])
            {
                if (distToNearestEnemy > act.dist || validEnemies.Length < act.numTargets)
                    continue;

                if (act.CheckConditions(kasuga, validEnemies))
                    ExecHeatAction(act, kasuga, validEnemies);
            }

            return false;
        }


        private static void ExecHeatAction(HeatAction act, Fighter attacker, Fighter[] enemies)
        {
            attacker.Character.GetBattleStatus().Heat = 0;
            BrawlerPlayer.ExecuteMove(act, attacker, enemies);
        }
    }
}
