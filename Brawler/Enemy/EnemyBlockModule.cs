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

        public bool ShouldBlockAttack(BattleDamageInfo dmgInf)
        {
            if (AI.LastGuardTime > 2f)
                GuaranteedBlocks = 0;

            if (GuaranteedBlocks > 0)
                return true;
            else
                return false;
        }

        public void OnBlocked()
        {
            AI.LastGuardTime = 0;
            AI.RecentHitsWithoutDefensiveMove = 0;
            AI.RecentDefensiveAttacks++;
            RecentlyBlockedHits++;

            if(GuaranteedBlocks > 0)
                GuaranteedBlocks--;
            else
                GuaranteedBlocks = 2;
        }

        public bool OnBlockedAnimEvent()
        {
            if (AI.EvasionModule.ShouldDoCounterAttack())
            {
                AI.EvasionModule.DoCounterAttack();
                return true;
            }

            return false;
        }
    }
}
