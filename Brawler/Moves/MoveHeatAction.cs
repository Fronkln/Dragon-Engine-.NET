using System;
using System.Timers;
using System.Collections.Generic;
using System.Linq;
using DragonEngineLibrary;


namespace Brawler
{
    public class HeatAction : MoveBase
    {
        TalkParamID heatAction;
        HeatActionCondition heatAttackCond;
        public float dist;
        int damage;
        public int numTargets;
        float killTime = 0;

        /*
        public HeatAction(TalkParamID action, HeatActionCondition cond, MoveInput[] input, float dist, int damage, int numTargets = 1, float killTime = 0) : base(1, null, MoveSimpleConditions.None)
        {
            heatAction = action;
            heatAttackCond = cond;
            this.dist = dist;
            this.numTargets = numTargets;
            this.damage = damage;
            this.killTime = killTime;
            inputKeys = input;
        }
        */

        public HeatAction(TalkParamID action, HeatActionCondition cond, float dist, int damage, int numTargets = 1, float killTime = 0) : base(1, null, MoveSimpleConditions.None)
        {
            heatAction = action;
            heatAttackCond = cond;
            this.dist = dist;
            this.numTargets = numTargets;
            this.damage = damage;
            this.killTime = killTime;
        }

        public override void OnMoveEnd()
        {
            
        }

        public override bool CheckConditions(Fighter fighter, Fighter[] targets)
        {
            List<Fighter> fightersToCheck = new List<Fighter>();

            for (int i = 0; i < numTargets; i++)
                fightersToCheck.Add(targets[i]);

            Vector3[] positions = fightersToCheck.Select(x => (Vector3)x.Character.GetPosCenter()).ToArray();
            Vector3 posSum = Vector3.zero;

            foreach (Vector3 pos in positions)
                posSum += pos;

            Vector3 center = posSum / positions.Length;

            if (Vector3.Distance(positions[0], center) > dist)
                return false;

            if (heatAttackCond == HeatActionCondition.None)
                return true;

            foreach (Fighter enemy in fightersToCheck)
            {
                if (heatAttackCond.HasFlag(HeatActionCondition.EnemyCriticalHealth))
                {
                    long curHp = enemy.Character.GetBattleStatus().CurrentHP;
                    long criticalHP = (long)(curHp * Mod.CriticalHPRatio);

                    if (curHp > criticalHP)
                        return false;
                }

                if (heatAttackCond.HasFlag(HeatActionCondition.EnemyDown))
                {
                    bool down = enemy.IsDown();
   
                    if (!down)
                        return false;
                }

            }

            return true;
        }

        /// <summary>
        /// Assuming the conditions were met
        /// </summary>
        public override void Execute(Fighter attacker, Fighter[] target)
        {
            base.Execute(attacker, target);


            HActRequestOptions opts = new HActRequestOptions();
            opts.base_mtx.matrix = attacker.Character.GetPosture().GetRootMatrix();
            opts.id = heatAction;
            opts.is_force_play = true;


            opts.Register(HActReplaceID.hu_player1, attacker.Character);
            opts.RegisterWeapon(AuthAssetReplaceID.we_player_r, attacker.GetWeapon(AttachmentCombinationID.right_weapon));
            //opts.RegisterFighterAndWeapon(HActReplaceID.hu_player1, attacker, AuthAssetReplaceID.we_player_r);
           
            Fighter[] enemies = FighterManager.GetAllEnemies().Where(x => !x.IsDead()).OrderBy(x => Vector3.Distance((Vector3)attacker.Character.Transform.Position, (Vector3)x.Character.Transform.Position)).ToArray();
            HActReplaceID curReplace = HActReplaceID.hu_enemy_00;

            List<Fighter> affectedEnemies = new List<Fighter>();

            SoundManager.PlayCue(SoundCuesheetID.battle_common, 10, 0);

            for(int i = 0; i < numTargets; i++)
            {
                if (i >= enemies.Length)
                    break;

                opts.Register(curReplace, enemies[i].Character.UID);
                curReplace = (HActReplaceID)((int)curReplace + 1);
                affectedEnemies.Add(enemies[i]);
            }


            foreach(Fighter fighter in affectedEnemies)
            {
                ECBattleStatus status = fighter.Character.GetBattleStatus();

                if(status.CurrentHP < damage)
                {
                    //if its 0 they die at the end of hact
                    if (killTime != 0)
                    {
                        Timer killtimer = new Timer() { Enabled = true, Interval = TimeSpan.FromSeconds(killTime).TotalMilliseconds, AutoReset = false };
                        killtimer.Elapsed += delegate { fighter.Character.ToDead(); };
                    }
                }

                status.CurrentHP -= damage;
                
            }

         //   Player.SetHeatNow(Player.ID.kasuga, 0);

            HActManager.RequestHAct(opts);
            AuthManager.PlayingScene.Get().SetSpeed(0);
        }

        public override bool MoveExecuting()
        {
            return HActManager.IsPlaying();
        }
    }
}
