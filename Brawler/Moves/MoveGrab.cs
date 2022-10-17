using System;
using DragonEngineLibrary;

namespace Brawler
{
    internal class MoveGrab : MoveBase
    {
        public enum GrabPhase
        {
            AnimStart,
            Grabbing,
            Grabbed,
            BreakOut,
            AttackLight,
            AttackHeavy,
            Throw,
        }

        public override MoveType MoveType => MoveType.MoveGrab;

        public RPGSkillID Grab;
        public RPGSkillID GrabSync;
        public RPGSkillID ShakeOff;
        /// <summary>
        /// We will check if this anim is playing to successfully determine a grab
        /// </summary>
        public MotionID GrabAnim;
        public RPGSkillID[] HitLight;
        public RPGSkillID HitHeavy;
        public RPGSkillID HitThrow;

        private float m_phaseTime = 0;

        private bool m_phaseDoOnceFlag;
        private bool m_execing = false;

        private int m_curLight = 0;
        private bool m_justAttacked;

        private bool m_toAttackLight;
        private bool m_toAttackHeavy;

        public GrabPhase Phase;

        public Fighter Victim;

        public MoveGrab(MoveInput[] input, MoveSimpleConditions cond) : base(0.01f, input, cond)
        {

        }

        public override void Execute(Fighter attacker, Fighter[] target)
        {
            m_execing = true;
            m_phaseDoOnceFlag = false;
            m_curLight = 0;
            ChangePhase(GrabPhase.AnimStart);

            BattleTurnManager.ForceCounterCommand(attacker, target[0], Grab);
            new DETask
            (
                delegate { return attacker.Character.GetMotion().RequestedAnimPlaying() || m_phaseTime > 2; },
                delegate
                {
                    if (attacker.Character.GetMotion().RequestedAnimPlaying())
                    {
                        ChangePhase(GrabPhase.Grabbing);
                        m_phaseDoOnceFlag = true;
                    }
                    else
                        m_execing = false;
                }
            );
        }

        public override bool AllowHActWhileExecuting()
        {
            return true;
        }

        public override bool IsSyncMove()
        {
            return true;
        }

        public override void Update()
        {
            m_phaseTime += DragonEngine.deltaTime;

            switch(Phase)
            {
                case GrabPhase.Grabbing:
                    if (m_phaseDoOnceFlag)
                    {
                        //Executes twice, investigate
                        new DETaskList
                        (
                           new DETaskWaitRequestedAnim(BrawlerBattleManager.Kasuga.Character.GetMotion(), null),
                           new DETaskNextFrame(
                               delegate 
                               {
                                   bool grabbed = BrawlerPlayer.MotionInfo.gmt_id_ == GrabAnim;

                                   if (grabbed)
                                   {
                                       //TODO: get the victim from sync smartly through DE functions
                                       Victim = BrawlerBattleManager.EnemiesNearest[0];

                                       ChangePhase(GrabPhase.Grabbed);
                                       Console.WriteLine("Grabbed");
                                   }
                                   else
                                       m_execing = false;
                               })
                        );

                        m_phaseDoOnceFlag = false;
                        return;
                    }
                    break;

                case GrabPhase.Grabbed:
                    GrabbedUpdate();
                    break;
                case GrabPhase.Throw:
                    ThrowUpdate();
                    break;
                case GrabPhase.AttackLight:
                    if (m_phaseDoOnceFlag)
                    {
                        AttackProcedure();
                        m_phaseDoOnceFlag = false;
                    }
                    else
                        AttackUpdate();
                    break;

            }
        }

        public override void InputUpdate()
        {
            base.InputUpdate();

            if(Phase == GrabPhase.Grabbed)
            {
                if (DragonEngine.IsKeyHeld(VirtualKey.F))
                {

                    BrawlerBattleManager.KasugaChara.HumanModeManager.ToEndReady();
                    Victim.Character.HumanModeManager.ToEndReady();
                    ChangePhase(GrabPhase.Throw);
                    m_phaseDoOnceFlag = true;

                    new DETask(delegate { return !BrawlerBattleManager.KasugaChara.GetMotion().RequestedAnimPlaying(); }, delegate
                    {
                        BattleTurnManager.ForceCounterCommand(BrawlerBattleManager.Kasuga, Victim, HitThrow);
                        ChangePhase(GrabPhase.Throw);

                        new DETaskList
                       (
                            new DETaskNextFrame(),
                            new DETaskWaitRequestedAnim(BrawlerBattleManager.KasugaChara.GetMotion(),
                                delegate
                                {
                                    m_execing = false;
                                })
                       );
                    });

                    m_phaseDoOnceFlag = true;
                }

                if(DragonEngine.IsKeyHeld(VirtualKey.G))
                {
                    ChangePhase(GrabPhase.AttackLight);
                    m_phaseDoOnceFlag = true;
                }
            }
        }

        private void GrabbedUpdate()
        {
            bool animPlaying = BrawlerPlayer.MotionInfo.gmt_id_ == GrabAnim;

            if (animPlaying && m_justAttacked)
                m_justAttacked = false;

            //Play escape grab anim
            if (!animPlaying && !m_justAttacked)
            {
                BattleTurnManager.ForceCounterCommand(BrawlerBattleManager.Kasuga, Victim, ShakeOff);
                m_execing = false;
                return;
            }              
        }

        private void AttackProcedure()
        {
            BrawlerBattleManager.KasugaChara.HumanModeManager.ToEndReady();
            Victim.Character.HumanModeManager.ToEndReady();

            new DETaskList
            (
                new DETask(delegate { return !BrawlerPlayer.Info.IsSync; }, 
                    delegate 
                    {
                        BattleTurnManager.ForceCounterCommand(BrawlerBattleManager.Kasuga, Victim, HitHeavy);
                        m_curLight++;

                        if (m_curLight >= HitLight.Length)
                            m_curLight = 0;
                    }, false)
            );

            new DETaskList
            (
                new DETask(delegate { return BrawlerPlayer.Info.IsSync; }, null, false),
                new DETask(delegate { return !BrawlerPlayer.Info.IsSync; },
                    delegate 
                    {
                        m_execing = false;
                        //m_justAttacked = true; 
                        //GrabSyncProcedure(); 
                    }, false)
            );
        }

        private void GrabSyncProcedure()
        {
            BattleTurnManager.ForceCounterCommand(BrawlerBattleManager.Kasuga, Victim, GrabSync);
            ChangePhase(GrabPhase.Grabbed);
        }

        private void AttackUpdate()
        {
   
        }

        private void ThrowUpdate()
        {
            return;
            if (m_phaseDoOnceFlag)
            {
                BattleTurnManager.ForceCounterCommand(BrawlerBattleManager.Kasuga, Victim, HitThrow);

                new DETaskList
               (
                    new DETaskNextFrame(),
                    new DETaskWaitRequestedAnim(BrawlerBattleManager.KasugaChara.GetMotion(),
                        delegate
                        {
                            m_execing = false;
                        })
               );

                m_phaseDoOnceFlag = false;
            }
        }

        private void ChangePhase(GrabPhase phase)
        {
            m_phaseTime = 0;
            Phase = phase;
        }

        public override bool MoveExecuting()
        {
            return m_execing;
        }

        public override void OnMoveEnd()
        {
            base.OnMoveEnd();

            Console.WriteLine("move over");
        }

        public override bool CheckConditions(Fighter fighter, Fighter[] targets)
        {

            return base.CheckConditions(fighter, targets);
        }
    }
}
