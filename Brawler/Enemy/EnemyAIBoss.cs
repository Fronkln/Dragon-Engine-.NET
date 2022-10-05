﻿using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using DragonEngineLibrary;

namespace Brawler
{
    //Unlike standard AI, these guys shouldnt get stunlocked if they get rapidly hit by Ichiban.
    //Its also important that they probably sidestep attacks as well.
    //Should probably blacklist certain control types. (a tiger sidestepping sounds fucked up)
    internal class EnemyAIBoss : EnemyAI
    {
        /// <summary>
        /// List of heat actions we took damage from. We will reduce 25-30% of DMG for subsequent uses.
        /// </summary>
        protected HashSet<TalkParamID> m_damagedHacts = new HashSet<TalkParamID>();

        //Last hact we took damage from.
        private TalkParamID m_lastDamagedHact = TalkParamID.invalid;
        public virtual MotionID TauntMotion => MotionID.invalid;
        protected virtual float BOSS_TAUNT_COOLDOWN => 7.5f;
        private float m_tauntCD;

        public override void Start()
        {
            base.Start();

            BlockModule.BlockChance = 20;
        }

        public override bool IsBoss()
        {
            return true;
        }

        public override bool ShouldBlockAttack(BattleDamageInfo dmgInf)
        {
            //Wow! We really took a lot of hits and the RNG hasnt gotten our back!
            if(RecentHitsWithoutDefensiveMove >= 6)
            {
                Console.WriteLine("Ate 6 whole hits without defensive move recently. Auto blocking");
                RecentHitsWithoutDefensiveMove = 0;
                return true;
            }

            if (BlockModule.ShouldBlockAttack(dmgInf))
                return true;
            else
                return base.ShouldBlockAttack(dmgInf);
        }

        public override void CombatUpdate()
        {
            base.CombatUpdate();

            if (m_tauntCD > 0)
                m_tauntCD -= DragonEngine.deltaTime;

            if(!IsAttacking()) 
            {
                if (BattleTurnManager.CurrentActionStep == BattleTurnManager.ActionStep.Ready)
                    if (m_tauntCD <= 0 && TauntMotion != MotionID.invalid)
                    if (ShouldTaunt())
                        TauntProcedure();
            }
        }

        public void TauntProcedure()
        {
            if (!Chara.Get().GetMotion().RequestedAnimPlaying())
            {
                m_tauntCD = BOSS_TAUNT_COOLDOWN;

                DETaskList list = new DETaskList(new DETask[]
                {
                    new DETaskNextFrame(),
                    new DETaskNextFrame(delegate
                    {
                        Chara.Get().GetMotion().RequestGMT(TauntMotion);
                        m_tauntCD =  BOSS_TAUNT_COOLDOWN;
                    }),
                });
            }
        }

        public override void HActProcedure()
        {
            base.HActProcedure();

            if (!HActManager.IsPlaying())
                if (m_lastDamagedHact != TalkParamID.invalid)
                {
                    m_damagedHacts.Add(m_lastDamagedHact);
                    m_lastDamagedHact = TalkParamID.invalid;
                }
        }

        //Returns the amount of damage that the AI has agreed to take
        //This is important to reduce heat action damage when its spammed.
        public override long ProcessHActDamage(TalkParamID hact, long dmg)
        {
            m_lastDamagedHact = hact;

            if (dmg > 1 && m_damagedHacts.Contains(hact))
            {
                dmg -= (long)(dmg * 0.3f);
                Console.WriteLine("Reduced damage for spammed hact " + hact);
            }

            return dmg;
        }

        public virtual bool ShouldTaunt()
        {
            return BrawlerPlayer.Info.IsGettingUp || BrawlerPlayer.Info.IsDown;
        }

        //Evade attack + strike back
        //Executed right before we go back to blocking in 90% of bosses.
        public override bool ShouldDoCounterAttack()
        {
            return EvasionModule.ShouldDoCounterAttack();
        }

        public override bool DoSpecial(BattleDamageInfo inf)
        {
            // Character.Character.GetMotion().RequestGMT(11);

            if (Character.GetStatus().IsSuperArmor())
                return false;

            //Respect the boundaries of guaranteed blocks and dont ignore it
            if (BlockModule.ShouldBlockAttack(inf))
                return false;

            bool shouldEvade = EvasionModule.ShouldEvade(inf);       

            if(shouldEvade)
            {
                EvasionModule.DoEvasion();

                /*
                if (m_evadedHits >= EvadeAmount)
                {
                    //Start blocking instead we dodged enough
                    m_evadedHits = 0;
                    LastGuardTime = 0;
                    m_forceGuard = true;
                }
                */

                return true;
            }

            return false;
        }
    }
}
