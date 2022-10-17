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

        //counter attacking

        private bool m_gettingUp = false;
        private bool m_getupHyperArmorDoOnce = false;

        public int RecentDefensiveAttacks = 0;
        //We can determine if RNG has failed us and force things as needed
        //Primarily meant for bosses
        public int RecentHitsWithoutDefensiveMove = 0; 

        //Player is spamming same attacks 0 iq
        private int RecentHits = 0;

        private const float Y7B_BTL_AI_SPM_TIME = 1.8f;

        //Amount of spam hits that makes the AI realize its being spam attacked
        private const int Y7B_BTL_AI_SPM_COUNT = 5;

        public List<RPGSkillID> CounterAttacks = new List<RPGSkillID>();

        public EnemyEvasionModule EvasionModule = null;
        public EnemyBlockModule BlockModule = null;
        public EnemySyncHActModule SyncHActModule = null;

        /// <summary>
        /// List of heat actions we have done.
        /// </summary>
        protected HashSet<TalkParamID> m_performedHacts = new HashSet<TalkParamID>();

        public virtual void Start()
        {
            BlockModule = new EnemyBlockModule() { AI = this };
            EvasionModule = new EnemyEvasionModule() { AI = this };
            SyncHActModule = new EnemySyncHActModule() { AI = this };
#if DEBUG
            Console.WriteLine(Chara.Get().Attributes.soldier_data_id + " " + Chara.Get().Attributes.enemy_id + " " + Chara.Get().Attributes.ctrl_type);
#endif
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
                BlockModule.RecentlyBlockedHits = 0;
                return;
            }

            MoveBase playerMove = BrawlerPlayer.m_lastMove;

            /*
            if(playerMove.MoveType == MoveType.MoveComboString)
            {
                //We are being combod
                //We treat this seperately because it depends on last guard time which is RNG
                //Gonna be different from sidesteps etc
                if (LastHitTime < Y7B_BTL_AI_SPM_TIME && LastGuardTime < Y7B_BTL_AI_SPM_TIME)
                    m_forceGuard = true;
            }
            */

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
            {
                RecentHits = 0;
                RecentHitsWithoutDefensiveMove = 0;
                RecentDefensiveAttacks = 0;
            }
        }

        public virtual void CombatUpdate()
        {
            BlockModule.Update();
            EvasionModule.Update();
            SyncHActModule.Update();

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


        public virtual bool AllowDamage()
        {
            return TutorialManager.AllowEnemyDamage();
        }


         //Not applicable to generic enemy
        public virtual bool ShouldDoCounterAttack()
        {
            return false;
        }

        public virtual bool IsAttacking()
        {
            return BrawlerBattleManager.CurrentAttacker.Character.UID == Chara.UID &&
                BattleTurnManager.CurrentPhase == BattleTurnManager.TurnPhase.Action &&
                BattleTurnManager.CurrentActionStep == BattleTurnManager.ActionStep.Action;
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


        public virtual void OnBlocked()
        {
            BlockModule.OnBlocked();
        }

        public virtual void OnGuardBroke()
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

        public void DoGrabHActSync(EnemyMoveSync sync)
        {

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

            if (Character.IsSync())
                return false;

            if (m_gettingUp)
                return false;

            if (Info.IsDown)
                return false;

            Random rnd = new Random();
            return rnd.Next(0, 101) <= BlockModule.BlockChance;
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

        //Counts for dodges and counter attacks too
        //Maybe dont?
        public virtual void OnHit()
        {
            LastHitTime = 0;
            RecentHits++;
        }

        /// <summary>
        /// Player is doing non-stop combos 0 IQ
        /// </summary>
        public bool IsBeingSpammed()
        {
            return RecentHits >= Y7B_BTL_AI_SPM_COUNT && LastHitTime <= Y7B_BTL_AI_SPM_TIME;
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

        public void DoHAct(TalkParamID hact, params Fighter[] allies)
        {
            HActRequestOptions opts = new HActRequestOptions();
            opts.base_mtx.matrix = Chara.Get().GetPosture().GetRootMatrix();
            opts.id = hact;
            opts.is_force_play = true;

            opts.Register(HActReplaceID.hu_enemy_00, Chara.UID);
            opts.Register(HActReplaceID.hu_player, BrawlerBattleManager.KasugaChara.UID);

            opts.RegisterWeapon(AuthAssetReplaceID.we_enemy_00_r, Character.GetWeapon(AttachmentCombinationID.right_weapon));
            opts.RegisterWeapon(AuthAssetReplaceID.we_enemy_00_l, Character.GetWeapon(AttachmentCombinationID.left_weapon));
            opts.RegisterWeapon(AuthAssetReplaceID.we_player_r, BrawlerBattleManager.Kasuga.GetWeapon(AttachmentCombinationID.right_weapon));
            opts.RegisterWeapon(AuthAssetReplaceID.we_player_l, BrawlerBattleManager.Kasuga.GetWeapon(AttachmentCombinationID.left_weapon));

            int curReplace = (int)HActReplaceID.hu_enemy_01;

            foreach (Fighter fighter in allies)
            {
                if (fighter.IsValid())
                    opts.Register((HActReplaceID)curReplace, fighter.Character.UID);

                curReplace++;
            }

            m_performedHacts.Add(hact);
            BattleTurnManager.RequestHActEvent(opts);
        }
    }
}
