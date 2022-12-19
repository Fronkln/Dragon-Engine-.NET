using DragonEngineLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brawler
{
    internal enum KiryuStyle
    {
        None,
        Brawler,
        Rush,
        Crash,
        Legend
    }

    internal class EnemyAIKiryu : EnemyAIBoss
    {
        //0 = Brawler (99)
        //1 = Rush (89)
        //2 = Crash (98)
        //3 = Legend (91)
        private KiryuStyle m_curStyle = KiryuStyle.None;

        private TalkParamID m_brawlerAntiGuard = (TalkParamID)12908;

        private TalkParamID[] m_playerFinishers = new TalkParamID[4]
        {
            (TalkParamID)12909,
            (TalkParamID)12910,
            (TalkParamID)12911,
            (TalkParamID)12912,
        };

        public override void CombatUpdate()
        {
            base.CombatUpdate();

            StyleUpdate();
        }

        private void StyleUpdate()
        {
            KiryuStyle style = GetCurrentStyle();

            if (m_curStyle != style)
                OnStyleSwitch(style);

            switch (m_curStyle)
            {
                case KiryuStyle.Brawler:
                    BrawlerUpdate();
                    break;
                case KiryuStyle.Rush:
                    RushUpdate();
                    break;
                case KiryuStyle.Crash:
                    CrashUpdate();
                    break;
                case KiryuStyle.Legend:
                    LegendUpdate();
                    break;
            }
        }

        private void BrawlerUpdate()
        {
            Character chara = Chara.Get();

            if (BrawlerPlayer.GuardTime > 4.5f)
                if (!chara.GetMotion().RequestedAnimPlaying())
                    if (DistanceToPlayer <= 3)
                        if (!m_performedHacts.Contains(m_brawlerAntiGuard))
                            DoHAct(m_brawlerAntiGuard, chara.Transform.Position);
        }

        protected override void OnStartGettingUp()
        {
            if (m_curStyle == KiryuStyle.Rush)
                BattleTurnManager.ForceCounterCommand(Character, BrawlerBattleManager.Kasuga, (RPGSkillID)1761);
        }

        public override bool DamageTransitCounter(BattleDamageInfoSafe dmg)
        {
            if (m_curStyle != KiryuStyle.Legend)
                return false;

            if(IsLegendCounterReady())
            {
                if (dmg.IsDirect)
                {

                    if (dmg.IsSyncStartDmg)
                        BattleTurnManager.ForceCounterCommand(Character, dmg.Attacker.Get().GetFighter(), (RPGSkillID)1228);
                    else
                    {
                      //  bool shouldGuard = BlockModule.ShouldBlockAttack(dmg);
                        BattleTurnManager.ForceCounterCommand(Character, dmg.Attacker.Get().GetFighter(), (RPGSkillID)1063);
                    }
                    return true;
                }
            }
            else
            {
                if (BlockModule.BlockProcedure && BlockModule.RecentlyBlockedHits >= 3)
                {
                    BattleTurnManager.ForceCounterCommand(Character, BrawlerBattleManager.Kasuga, RPGSkillID.boss_kiryu_legend_atk_d);
                    return true;
                }

            }

            return base.DamageTransitCounter(dmg);
        }

        private bool IsLegendCounterReady()
        {
            MotionID counterGmt = Chara.Get().GetMotion().GmtID;

            //Dragon's Gaze
            return 
                counterGmt == MotionID.E_KRL_BTL_SUD_komaki_st || 
                counterGmt == MotionID.E_KRL_BTL_SUD_komaki_lp;
        }

        private void RushUpdate()
        {

        }

        private void CrashUpdate()
        {

        }

        private void LegendUpdate()
        {

        }

        public void OnStyleSwitch(KiryuStyle newStyle)
        {
            m_curStyle = newStyle;
        }

        private KiryuStyle GetCurrentStyle()
        {
            switch (Character.GetStatus().GetArts())
            {
                default:
                    return KiryuStyle.Brawler;
                case 99:
                    return KiryuStyle.Brawler;
                case 89:
                    return KiryuStyle.Rush;
                case 98:
                    return KiryuStyle.Crash;
                case 91:
                    return KiryuStyle.Legend;
            }
        }

        public override bool DoFinisher(BattleDamageInfoSafe dmgInf)
        {
            DoHAct(m_playerFinishers[(int)m_curStyle], Chara.Get().Transform.Position);
            return true;
        }
    }
}
