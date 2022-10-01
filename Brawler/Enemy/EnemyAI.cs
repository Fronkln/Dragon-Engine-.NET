using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DragonEngineLibrary;

namespace Brawler
{
    //Callback based Enemy AI
    internal class EnemyAI
    {
        public struct BrawlerAIFlags
        {
            public bool ShouldGuardBreakFlag;
        }

        public Fighter Character;
        public EntityHandle<Character> Chara;

        public BrawlerFighterInfo Info;
        public BrawlerAIFlags Flags;

        public float LastHitTime = 10000;
        public float LastGuardTime = 10000;

        private bool m_gettingUp = false;
        private bool m_getupHyperArmorDoOnce = false;

        //Use cautiously
        protected bool m_forceGuard = false;
        private int m_numGuardedHits = 0;

        //Player is spamming same attacks 0 iq
        private int m_spammedHits = 0;

        private const float Y7B_BTL_AI_SPM_TIME = 1.8f;

        //Amount of spam hits that makes the AI realize its being spam attacked
        private const int Y7B_BTL_AI_SPM_COUNT = 5;

        public virtual void Start()
        {

        }

        public virtual void InitResources()
        {

        }

        //Decide on a strategy based on player input
        private void ReadPlayerInput()
        {
            if (m_gettingUp)
                return;

            if (BrawlerPlayer.m_lastMove == null)
            {
                m_forceGuard = false;
                m_numGuardedHits = 0;
                return;
            }

            MoveBase playerMove = BrawlerPlayer.m_lastMove;

            if(playerMove.MoveType == MoveType.MoveComboString)
            {
                //We are being combod
                //We treat this seperately because it depends on last guard time which is RNG
                //Gonna be different from sidesteps etc
                if (LastHitTime < Y7B_BTL_AI_SPM_TIME && LastGuardTime < Y7B_BTL_AI_SPM_TIME)
                    m_forceGuard = true;
            }

        }

        public void Update()
        {
            Info.Update(Character);
            ReadPlayerInput();

            m_gettingUp = Info.IsGettingUp;

            CombatUpdate();
            HActProcedure();

            float delta = DragonEngine.deltaTime;

            LastGuardTime += delta;

            //Helps with timings even if it doesnt look like it makes sense.
            if(!Info.IsDown && !Info.IsRagdoll)
                LastHitTime += delta;

            if (LastHitTime > Y7B_BTL_AI_SPM_TIME)
                m_spammedHits = 0;
        }

        public virtual void CombatUpdate()
        {
            if (m_getupHyperArmorDoOnce && !m_gettingUp)
            {
                m_getupHyperArmorDoOnce = false;
                Character.GetStatus().SetSuperArmor(false);
            }

            if (m_gettingUp && !m_getupHyperArmorDoOnce)
            {
                m_getupHyperArmorDoOnce = true;
                Character.GetStatus().SetSuperArmor(true);
            }
        }

        public virtual void HActProcedure()
        {

        }

        public void TurnUpdate(BattleTurnManager.ActionStep phase)
        {
            CheckChange();
        }


        public virtual bool ShouldDoBlockCounter()
        {
            return false;
        }

        //Check if we are too far. If we are, give turn to nearest enemy instead.
        private void CheckChange()
        {

            if (BrawlerBattleManager.Enemies.Length <= 1)
                return;

            //We are already the closest (this array is auto filtered by BrawlerBattleManager based on distance)
            if (BrawlerBattleManager.Enemies[0].Character.UID == Chara.UID)
                return;


            Vector3 playerPos = (Vector3)BrawlerBattleManager.Kasuga.Character.Transform.Position;
            float distToPlayer = Vector3.Distance((Vector3)Chara.Get().Transform.Position, playerPos);

            if(distToPlayer > 13 || Info.IsDown)
            {
                EnemyManager._OverrideNextAttackerOnce = BrawlerBattleManager.Enemies[0];
                BattleTurnManager.ChangeActionStep(BattleTurnManager.ActionStep.ActionEnd);
            }
        }


        public void OnBlocked()
        {
            LastGuardTime = 0;
            m_numGuardedHits++;

            if (m_numGuardedHits >= 5)
            {
                LastGuardTime = 999;
                m_forceGuard = false;
                m_spammedHits = 0;
            }
        }

        public void OnGuardBroke()
        {
           // Chara.Get().GetMotion().RequestGMT((MotionID)5542);
            SoundManager.PlayCue(SoundCuesheetID.battle_common, 12, 0);

            //Processed by SetMotionID guardbreak reaction
            Flags.ShouldGuardBreakFlag = true;
        }


        public void Sway()
        {
            Chara.Get().HumanModeManager.ToSway();
        }

        public virtual bool DoSpecial(BattleDamageInfo inf)
        {
            return false;
        }
        public virtual bool ShouldBlockAttack(BattleDamageInfo dmgInf)
        {
            //It doesnt matter when they have super armor
            if (Character.GetStatus().IsSuperArmor())
                return false;

            if (m_forceGuard)
                return true;

            if (m_gettingUp)
                return false;

            if (Info.IsDown)
                return false;

            Random rnd = new Random();
            return rnd.Next(0, 101) <= 10;
        }

        public virtual bool IsBoss()
        {
            return false;
        }

        //Returns the amount of damage that the AI has agreed to take
        //This is important to reduce heat action damage when its spammed.
        public virtual long ProcessHActDamage(TalkParamID hact, long dmg)
        {
            return dmg;
        }

        public void OnAttacked()
        {
            LastHitTime = 0;
            m_spammedHits++;
        }

        /// <summary>
        /// Player is doing non-stop combos 0 IQ
        /// </summary>
        public bool IsBeingSpammed()
        {
            return m_spammedHits >= Y7B_BTL_AI_SPM_COUNT && LastHitTime <= Y7B_BTL_AI_SPM_TIME;
        }

        //Action, Action
        public virtual void OnStartAttack()
        {
     
        }

        public virtual void OnPlayerGettingUp()
        {

        }

        public virtual void OnPlayerDown()
        {

        }

    }
}
