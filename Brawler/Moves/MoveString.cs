using System;
using System.Timers;
using DragonEngineLibrary;
using DragonEngineLibrary.Service;

namespace Brawler
{
    /// <summary>
    /// Move strings are meant to use fighter command.
    /// </summary>
    public class MoveString : MoveBase
    {
        public override MoveType MoveType => MoveType.MoveComboString;

        public struct AttackFrame
        {
            public FighterCommandID Attack;
            public FighterCommandID Finisher;

            //Obsoleted by BEP followup
            //public float ActivationMinimumTime;

            public bool Buffered;

            public float AttackTime; // <--- Next on the chopping block to make combat more dynamic
            public float EndTime; // <--- Next on the chopping block to make combat more dynamic

            public AttackFrame(FighterCommandID attack, bool buffered, float attackTime, float endTime = 0)
            {
                Attack = attack;
                Finisher = new FighterCommandID();
                Buffered = buffered;
                //   ActivationMinimumTime = activMinTime;
                AttackTime = attackTime;

                if (endTime == 0)
                    endTime = AttackTime;

                EndTime = endTime;
            }

            public AttackFrame(FighterCommandID attack, FighterCommandID heavyAttack, bool buffered, float attackTime, float endTime = 0)
            {
                Attack = attack;
                Finisher = heavyAttack;
                Buffered = buffered;
                //ActivationMinimumTime = activMinTime;
                AttackTime = attackTime;

                if (endTime == 0)
                    endTime = AttackTime;

                EndTime = endTime;
            }
        }

        public AttackFrame[] Attacks;


        private float m_curAttackTime = 0;
        private int m_curAttack = 0;
        private MotionService.TimingResult m_curFollowupWindow;

        bool isFinishing = false;

        bool comboOver = false;
        bool nextAttackGuaranteed = false;
        bool nextAttackIsFinisher = false;

        //prevent input from doing shit
        bool inputWaitNextAttack = false;

        public AttackFrame GetCurrentAttack()
        {
            return Attacks[m_curAttack];
        }

        public void ExecuteCurrentAttack()
        {
            m_curAttackTime = 0;

            if (!nextAttackIsFinisher)
                BrawlerBattleManager.KasugaChara.HumanModeManager.ToAttackMode(GetCurrentAttack().Attack);
            else
            {;
                BrawlerBattleManager.KasugaChara.HumanModeManager.ToAttackMode(GetCurrentAttack().Finisher);
                isFinishing = true;
            }
        }

        private bool CanBufferAttack(MotionPlayInfo motInf)
        {
            return GetCurrentAttack().Buffered && motInf.tick_gmt_now_ < m_curFollowupWindow.Start;
        }

        private void UpdateFollowUpWindow(MotionPlayInfo motInf)
        {

#if DEBUG_FOLLOWUPS
            Console.WriteLine(m_curAttack+"|" + motInf.tick_gmt_now_ + " " + m_curFollowupWindow.Start + " " + m_curFollowupWindow.End);
#endif

            //We are in the range of followup window
            if (CanBufferAttack(motInf) || motInf.tick_gmt_now_ >= m_curFollowupWindow.Start && motInf.tick_gmt_now_ < m_curFollowupWindow.End)
            {
                if (Mod.DebugAutomaticCombo == true)
                {
                    nextAttackGuaranteed = true;
                    return;
                }

                bool wasGuaranteedBefore = nextAttackGuaranteed;


                if (DragonEngine.IsKeyDown(VirtualKey.LeftButton))
                    nextAttackGuaranteed = true;
                else
                {
                    bool finisherExists = Attacks[m_curAttack].Finisher.set_ != 0;

                    if (finisherExists)
                        if (DragonEngine.IsKeyDown(VirtualKey.RightButton))
                        {
                            nextAttackGuaranteed = true;
                            nextAttackIsFinisher = true;
                        }
                }

#if DEBUG_FOLLOWUPS
                if (!wasGuaranteedBefore)
                    if (nextAttackGuaranteed)
                        Console.WriteLine("Next attack guaranteed.");
#endif
            }

        }

        //its important that attacks should get interrupted
        //if the player flinches.
        //which isnt a thing right now
        //it will most certainly mess up string moves
        public override void Update()
        {
            inputWaitNextAttack = false;

            bool endOfCombo = m_curAttack >= Attacks.Length;

            if (endOfCombo)
                return;


            ECMotion motion = DragonEngine.GetHumanPlayer().GetMotion();
            MotionPlayInfo playInf = motion.PlayInfo;

            AttackFrame curMove = GetCurrentAttack();
            m_curAttackTime += DragonEngine.deltaTime;

            if (m_curAttackTime >= curMove.EndTime)
            {
                OnComboOver();
                return;
            }

            if (m_curFollowupWindow.IsValid())
            {
                if (!curMove.Buffered || (curMove.Buffered && playInf.tick_now_ >= m_curFollowupWindow.Start))
                    if (nextAttackGuaranteed)
                    {
#if DEBUG_FOLLOWUPS
                        Console.WriteLine("Executing followup.");
#endif

                        inputWaitNextAttack = true;

                        if (m_curAttack + 1 < Attacks.Length)
                        {
                            //Hacky, i dont care.
                            if (nextAttackIsFinisher)
                            {
                                ExecMoveAndGoNext();
                                m_curAttack++;
                            }
                            else
                            {

                                m_curAttack++;
                                ExecMoveAndGoNext();
                            }
                        }

                        //replace this with a motion end check instead
                        if (m_curAttack >= Attacks.Length)
                            new SimpleTimer(curMove.EndTime, delegate { OnComboOver(); });
                    }
            }

            bool getNextFollowup = playInf.tick_old_ == 0 || !m_curFollowupWindow.IsValid() || m_curFollowupWindow.End < playInf.tick_now_;

            if (getNextFollowup)
            {
                m_curFollowupWindow = MotionService.SearchTimingDetail(playInf.tick_now_, motion.BepID, 71);
            }
        }


        void ExecMoveAndGoNext()
        {
            ExecuteCurrentAttack();
            m_curAttackTime = 0;
            nextAttackGuaranteed = false;
            nextAttackIsFinisher = false;
          //  m_curAttack++;
        }

        public override void Execute(Fighter attacker, Fighter[] target)
        {
            comboOver = false;
            m_curAttack = 0;
            m_curAttackTime = 0;
            nextAttackGuaranteed = false;
            nextAttackIsFinisher = false;
            isFinishing = false;

            //ExecMoveAndGoNext();
            ExecuteCurrentAttack();
        }

        public override void InputUpdate()
        {
            if (inputWaitNextAttack)
                return;

            if (isFinishing || m_curAttack >= Attacks.Length || nextAttackGuaranteed)
                return;

            if (m_curFollowupWindow.IsValid())
                UpdateFollowUpWindow(BrawlerBattleManager.KasugaChara.GetMotion().PlayInfo);
#if DEBUG_FOLLOWUPS
            else
                Console.WriteLine("No followup window on " + GetCurrentAttack().Attack.cmd + " " + GetCurrentAttack().Attack.set_ + " bep ID " + BrawlerBattleManager.KasugaChara.GetMotion().BepID);
#endif
               

        }

        void OnComboOver()
        {
            comboOver = true;
            BrawlerPlayer.m_attackCooldown = 0.5f;
            BrawlerBattleManager.KasugaChara.HumanModeManager.ToEndReady();
        }


        public override bool MoveExecuting()
        {
            if (isFinishing)
                return m_curAttackTime < GetCurrentAttack().EndTime;

            if (Attacks.Length > 1)
                return comboOver == false;
            else
                return m_curAttackTime >= Attacks[0].EndTime;
        }

        public MoveString(AttackFrame[] Attacks, MoveInput[] input, MoveSimpleConditions cond) : base(999999, input, cond)
        {
            this.Attacks = Attacks;
        }
    }
}
