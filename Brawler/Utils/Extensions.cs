using System;
using System.Threading.Tasks;
using DragonEngineLibrary;

namespace Brawler
{
    internal static class Extensions
    {
        //This is not ideal. But i havent been able to come up with any real solution
        public static bool IsAnimDamage(this Character chara)
        {
            return chara.GetMotion().GmtID.ToString().Contains("_dmg_");
        }

        /// <summary>
        /// Request GMT and wait for it to start. Finishes on anim start
        /// </summary>
        public async static Task RequestAndWaitGMT(this ECMotion motion, MotionID gmt)
        {
            motion.RequestGMT(gmt);

            await Task.Run(() =>
            {
                new DETaskAsync(delegate { return motion.GmtID == gmt; });
            });
        }

        public static bool RequestedAnimPlaying(this ECMotion motion)
        {
            return motion.GmtID != MotionID.invalid;
        }


        public static bool IsBrawlerCriticalHP(this Fighter fighter)
        {
            return IsHPBelowRatio(fighter, Mod.CriticalHPRatio);
        }

        public static bool IsHPBelowRatio(this Fighter fighter, float ratio)
        {
            ECBattleStatus status = fighter.GetStatus();

            return status.CurrentHP <= (status.MaxHP * ratio);
        }
    }
}
