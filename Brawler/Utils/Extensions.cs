using System;
using System.Threading.Tasks;
using DragonEngineLibrary;
using DragonEngineLibrary.Service;

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

        public static bool IsPlayingAnim(this ECMotion motion, MotionID gmt)
        {
            return motion.GmtID == gmt;
        }

        public static bool InBepNodeRange(this ECMotion motion, uint nodeID)
        {
            MotionPlayInfo inf = motion.PlayInfo;
            MotionService.TimingResult res = MotionService.SearchTimingDetail(inf.tick_now_, motion.BepID, nodeID);

            if (res.Start == -1 || res.End == -1)
                return false;

            return inf.tick_gmt_now_>= res.Start && inf.tick_gmt_now_<= res.End;
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
