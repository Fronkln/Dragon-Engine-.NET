using System;
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

        /// <summary>
        /// List of heat actions we have done.
        /// </summary>
        protected HashSet<TalkParamID> m_performedHacts = new HashSet<TalkParamID>();

        //Last hact we took damage from.
        private TalkParamID m_lastDamagedHact = TalkParamID.invalid;

        //public virtual MotionID TauntMotion => MotionID.invalid;

        public virtual MotionID TauntMotion => MotionID.test_dance;
        private const float Y7B_BOSS_TAUNT_COOLDOWN = 7.5f;
        private float m_tauntCD;

        //Amount of attacks to sidestep until we go back to blocking
        //Can be overrided for people like Majima who will never stop
        public virtual int EvadeAmount => 6;
        private int m_evadedHits = 0;

        protected List<RPGSkillID> m_counterAttacks = new List<RPGSkillID>();
        protected virtual float m_counterAttackCooldown => 4.5f;
        private float m_counterAttackCD;

        public override bool IsBoss()
        {
            return true;
        }

        public override void CombatUpdate()
        {
            base.CombatUpdate();

            if (!IsBeingSpammed())
                m_evadedHits = 0;

            if (m_tauntCD > 0)
                m_tauntCD -= DragonEngine.deltaTime;

            if (m_counterAttackCD > 0)
                m_counterAttackCD -= DragonEngine.deltaTime;

            if (m_tauntCD <= 0 && BrawlerPlayer.Info.IsGettingUp || BrawlerPlayer.Info.IsDown)
                TauntProcedure();
        }

        public void TauntProcedure()
        {
            if (!Chara.Get().GetMotion().RequestedAnimPlaying())
                if (TauntMotion != MotionID.invalid)
                    Chara.Get().GetMotion().RequestGMT(TauntMotion);
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

        public override bool ShouldDoBlockCounter()
        {
            return false;
        }

        //Evade attack + strike back
        //Executed right before we go back to blocking in 90% of bosses.
        public virtual bool ShouldDoCounterAttack()
        {   
            return m_counterAttackCD <= 0 && m_counterAttacks.Count > 0 && m_evadedHits >= 6;
        }

        public virtual void DoCounterAttack()
        {
            m_counterAttackCD = m_counterAttackCooldown;

            Console.WriteLine("pre!");

            DETaskList list = new DETaskList(new DETask[]
            {
               new DETaskTime(0.45f, delegate
               {
                   Console.WriteLine("Time for a devastating counter attack!");

                   BattleTurnManager.ForceCounterCommand(Character, BrawlerBattleManager.Kasuga, m_counterAttacks[ new Random().Next(0, m_counterAttacks.Count)]);
                 
                   Chara.Get().Components.EffectEvent.Get().PlayEvent((EffectEventCharaID)206);
                   SoundManager.PlayCue(SoundCuesheetID.battle_common, 5, 0);

                   //Grant superarmor while countering.
                   if(!Character.GetStatus().IsSuperArmor())
                   {
                       Character.GetStatus().SetSuperArmor(true);

                       DETaskList armorProcedure = new DETaskList(new DETask[]
                       {
                           new DETaskNextFrame(),
                           new DETaskNextFrame(),
                           new DETask(
                               delegate
                               { 
                                   return Chara.Get().GetMotion().GmtID == 0; 
                               }, 
                               delegate
                               {
                                   Character.GetStatus().SetSuperArmor(false);
                               }, false)
                       });
                   }
               }, false),
            });
        }

        public override bool DoSpecial(BattleDamageInfo inf)
        {
            // Character.Character.GetMotion().RequestGMT(11);

            if (Character.GetStatus().IsSuperArmor())
                return false;

            if (IsBeingSpammed() && LastGuardTime > 1.3f)
            {
                //TODO: probably make this a overridable function.
                Character.Character.HumanModeManager.ToSway();
                m_evadedHits++;

                if (ShouldDoCounterAttack())
                    DoCounterAttack();

                if (m_evadedHits >= EvadeAmount)
                {
                    //Start blocking instead we dodged enough
                    m_evadedHits = 0;
                    LastGuardTime = 0;
                    m_forceGuard = true;
                }

                return true;
            }

            return false;
        }

        public void DoHAct(TalkParamID hact)
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

            m_performedHacts.Add(hact);
            BattleTurnManager.RequestHActEvent(opts);
        }
    }
}
