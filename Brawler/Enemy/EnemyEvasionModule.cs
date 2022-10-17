using System;
using DragonEngineLibrary;

namespace Brawler
{
    internal class EnemyEvasionModule : EnemyModule
    {
        public float LastEvasionTime = 9999;
        public int RecentlyEvadedAttacks = 0;

        public float CurrentCounterAttackCooldown = 0;
        public float CounterAttackCooldown = 4.5f;

        public bool IsCounterAttacking = false;

        public override void Update()
        {
            base.Update();

            if (LastEvasionTime > 1.8f)
                RecentlyEvadedAttacks = 0;

            float delta = DragonEngine.deltaTime;

            CounterAttackCooldown -= delta;
            LastEvasionTime += delta;
        }

        public void DoEvasion()
        {
            AI.Chara.Get().HumanModeManager.ToSway();
            LastEvasionTime = 0;
            RecentlyEvadedAttacks++;
            AI.RecentHitsWithoutDefensiveMove = 0;
            AI.RecentDefensiveAttacks++;

            if (AI.ShouldDoCounterAttack())
                DoCounterAttack();
        }

        public bool ShouldEvade(BattleDamageInfo inf)
        {
            //Maybe instead of blanket banning super armor evasions
            //Set a flag for when we dont want to dodge during certain parts
            //Example: Sawashiro 2 charged attack being cancelled out by evasion bug

            //replace with is counter attacking
            if (AI.IsAttacking())
                return false;

            if (AI.Character.IsSync())
                return false;

            if (AI.Character.GetStatus().IsSuperArmor())
                return false;

            if (AI.BlockModule.ShouldBlockAttack(inf))
                return false;

            bool firstEvasion = ShouldEvadeFirstAttack();

            if (firstEvasion)
                return firstEvasion;
            else
                //Make this a proper algorithm later
                return new Random().Next(0, 101) <= 10;
        }

        //First attack = Hasnt got hit since 2.5 seconds
        public bool ShouldEvadeFirstAttack()
        {
            const float h_firstEvasionChance = 40;

            if (!AI.IsBoss())
                return false;

            if (AI.LastHitTime < 2.5f)
                return false;
            else
                return new Random().Next(0, 101) <= h_firstEvasionChance;
        }

        public virtual bool ShouldDoCounterAttack()
        {
            return CurrentCounterAttackCooldown <= 0 && AI.CounterAttacks.Count > 0 && AI.RecentDefensiveAttacks >= 5;
        }

        public virtual void DoCounterAttack(bool immediate = false)
        {
            CurrentCounterAttackCooldown = CounterAttackCooldown;
            //This part is important because this variable only exists for defensive moves
            AI.RecentDefensiveAttacks = 0;
            AI.RecentHitsWithoutDefensiveMove = 0;

            bool wasSuper = AI.Character.GetStatus().IsSuperArmor();

            if (!wasSuper)
                AI.Character.GetStatus().SetSuperArmor(true);

            DETaskList list = new DETaskList(new DETask[]
            {
               new DETaskTime(immediate ? 0 : 0.45f, delegate
               {
                   Console.WriteLine("Time for a devastating counter attack!");
                   BattleTurnManager.ForceCounterCommand(AI.Character, BrawlerBattleManager.Kasuga, AI.CounterAttacks[ new Random().Next(0, AI.CounterAttacks.Count)]);

                   AI.Chara.Get().Components.EffectEvent.Get().PlayEvent((EffectEventCharaID)206);
                   SoundManager.PlayCue(SoundCuesheetID.battle_common, 5, 0);

                   //Grant superarmor while countering.
                   if(!wasSuper)
                   {
                       IsCounterAttacking = true;

                       DETaskList counterProcedure = new DETaskList(new DETask[]
                       {
                           new DETaskNextFrame(),
                           new DETaskNextFrame(),
                           new DETask(
                               delegate
                               {
                                   return AI.Chara.Get().GetMotion().GmtID == 0;
                               },
                               delegate
                               {
                                   IsCounterAttacking = false;
                                   AI.Character.GetStatus().SetSuperArmor(false);
                               }, false)
                       });
                   }
               }, false),
            });
        }
    }
}
