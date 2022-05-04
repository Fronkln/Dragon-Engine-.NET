using System;
using System.Timers;
using DragonEngineLibrary;

namespace Brawler
{
    /// <summary>
    /// Move strings are meant to use fighter command.
    /// </summary>
    public class MoveString : MoveBase
    {
        public struct AttackFrame
        {
            public FighterCommandID Attack;

            public float ActivationMinimumTime;
            public float AttackTime;
            public float EndTime;

            public AttackFrame(FighterCommandID attack, float activMinTime, float attackTime, float endTime = 0)
            {
                Attack = attack;
                ActivationMinimumTime = activMinTime;
                AttackTime = attackTime;

                if (endTime == 0)
                    endTime = AttackTime;

                EndTime = endTime;
            }
        }

        public AttackFrame[] Attacks;


        private float m_curAttackTime = 0;
        private int m_curAttack = 0;

        bool attackComplete = false;

        bool comboOver = false;
        bool nextAttackGuaranteed = false;

        public AttackFrame GetCurrentAttack()
        {
            return Attacks[m_curAttack];
        }

        public void ExecuteCurrentAttack()
        {
            m_curAttackTime = 0;
            FighterManager.GetFighter(0).Character.HumanModeManager.ToAttackMode(GetCurrentAttack().Attack);
        }

        public override void Update()
        {
            bool endOfCombo = m_curAttack >= Attacks.Length;

            if (endOfCombo)
                return;

            AttackFrame curMove = GetCurrentAttack();
            m_curAttackTime += DragonEngine.deltaTime;

            if(m_curAttackTime >= curMove.EndTime)
            {
                OnComboOver();
                return;
            }

            if (m_curAttackTime >= curMove.ActivationMinimumTime)
            {
                if(Mod.DebugAutomaticCombo || nextAttackGuaranteed)
                    ExecMoveAndGoNext();

                if (m_curAttack >= Attacks.Length)
                    new SimpleTimer(curMove.EndTime, delegate { OnComboOver(); });
            }
            
        }


        void ExecMoveAndGoNext()
        {
            ExecuteCurrentAttack();
            attackComplete = false;
            m_curAttackTime = 0;
            nextAttackGuaranteed = false;
            m_curAttack++;
        }

        public override void Execute(Fighter attacker, Fighter[] target)
        {
            comboOver = false;
            m_curAttack = 0;
            m_curAttackTime = 0;
            nextAttackGuaranteed = false;
            attackComplete = false;

            ExecMoveAndGoNext();
        }

        public override void InputUpdate()
        {
            if (m_curAttack >= Attacks.Length)
                return;

            AttackFrame curAttack = GetCurrentAttack();

            if (m_curAttackTime < GetCurrentAttack().ActivationMinimumTime)
            {
                if (DragonEngine.IsKeyDown(VirtualKey.LeftButton))
                    nextAttackGuaranteed = true;
            }

        }

        void OnComboOver()
        {
            comboOver = true;
            BrawlerPlayer.m_attackCooldown = 0.5f;
            DragonEngine.GetHumanPlayer().HumanModeManager.ToEndReady();
        }


        public override bool MoveExecuting()
        {
            return comboOver == false;
        }

        public MoveString(AttackFrame[] Attacks, MoveInput[] input, MoveSimpleConditions cond) : base(999999, input, cond)
        {
            this.Attacks = Attacks;
        }
    }
}
