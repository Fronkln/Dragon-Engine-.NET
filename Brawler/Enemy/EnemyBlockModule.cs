using System;
using DragonEngineLibrary;

namespace Brawler
{
    //Experimenting this with bosses right now.
    //Move to generic guys if good enough
    internal class EnemyBlockModule : EnemyModule
    {
        public float BlockChance = 15;
        public int GuaranteedBlocks = 0;

        public int RecentlyBlockedHits = 0;

        /// <summary>
        /// Enemies will guard on JustGuard event as long as this is true. We have to manage this.
        /// </summary>
        public bool BlockProcedure = false;

        //Prevent blocking for X seconds, only ever used in common thugs
        public float BlockPenalty = 0;

        public override void Update()
        {
            base.Update();

            if (BlockProcedure)
                GuardUpdate();

            if (BlockPenalty > 0)
                BlockPenalty -= DragonEngine.deltaTime;
        }

        private void GuardUpdate()
        {
            float cancelTime = GuaranteedBlocks <= 0 ? 1.5f : 3.5f;

            if(AI.LastHitTime > cancelTime)
            {
                BlockProcedure = false;
                GuaranteedBlocks = 0;
            }
        }

        public bool CanBlockAttack(BattleDamageInfoSafe dmgInf)
        {
            if (BlockPenalty > 0)
                return false;
            if (AI.EvasionModule.IsCounterAttacking)
                return false;
            if (AI.IsBeingJuggled())
                return false;

            return true;
        }

        public bool ShouldBlockAttack()
        {
            if (AI.Character.IsInvincible())
                return false;

            if (AI.RecentHitsWithoutDefensiveMove > 5)
                return true;

            if (new Random().Next(0, 101) <= BlockChance)
                return true;

            return false;
        }

        public void OnBlocked()
        {
            AI.LastGuardTime = 0;
            AI.RecentHitsWithoutDefensiveMove = 0;
            AI.RecentDefensiveAttacks++;
            RecentlyBlockedHits++;

            if (GuaranteedBlocks > 0)
                GuaranteedBlocks--;
        }

        public bool OnBlockedAnimEvent()
        {
            if (AI.EvasionModule.ShouldDoCounterAttack())
            {
                AI.EvasionModule.DoCounterAttack(true);
                return true;
            }

            return false;
        }
    }
}
