using System;
using System.Linq;
using System.Collections.Generic;
using DragonEngineLibrary;

namespace Brawler
{
    internal static class EnemyManager
    {
        public static Dictionary<uint, EnemyAI> EnemyAIs = new Dictionary<uint, EnemyAI>();

        internal static Fighter _OverrideNextAttackerOnce = null;


        //CALLBACK STUFF
        private static bool m_playerGettingUpDoOnce;
        private static bool m_playerDownDoOnce;

        private static bool m_attackStartDoOnce = false;

        private static void UpdateCallback()
        {

            if(BattleTurnManager.CurrentPhase == BattleTurnManager.TurnPhase.Action)
            {
                if(BattleTurnManager.CurrentActionStep != BattleTurnManager.ActionStep.Action)
                    m_attackStartDoOnce = false;
                else
                {
                    if(!m_attackStartDoOnce)
                    {
                        EnemyAIs[BrawlerBattleManager.CurrentAttacker.Character.UID].OnStartAttack();
                        m_attackStartDoOnce = true;
                    }
                }
            }


            if (BrawlerPlayer.Info.IsGettingUp)
            {
                if (!m_playerGettingUpDoOnce)
                {
                    foreach (var kv in EnemyAIs)
                        kv.Value.OnPlayerGettingUp();

                    m_playerGettingUpDoOnce = true;
                }
            }
            else
                if (m_playerGettingUpDoOnce)
                    m_playerGettingUpDoOnce = false;


            if (BrawlerPlayer.Info.IsDown)
            {
                if (!m_playerDownDoOnce)
                {
                    foreach (var kv in EnemyAIs)
                        kv.Value.OnPlayerDown();

                    m_playerDownDoOnce = true;
                }
            }
            else
                if (m_playerDownDoOnce)
                m_playerDownDoOnce = false;
        }

        public static void Update()
        {
            if (BrawlerBattleManager.Enemies.Length <= 0)
                return;

            UpdateCallback();


            foreach (Fighter fighter in BrawlerBattleManager.Enemies)
                if (!EnemyAIs.ContainsKey(fighter.Character.UID))
                {
                    EnemyAI createdAI = null;

                    if (!fighter.IsBoss())
                        createdAI = CreateEnemyAI(fighter);
                    else
                        createdAI = CreateBossAI(fighter);

                    EnemyAIs.Add(fighter.Character.UID, createdAI);

                    createdAI.Start();
                    createdAI.InitResources();
                }

            EnemyAIs = EnemyAIs.Where(x => x.Value.Chara.IsValid() && !x.Value.Character.IsDead()).ToDictionary(x => x.Key, x => x.Value);

            uint attacker = BattleTurnManager.SelectedFighter.UID;

            foreach (var kv in EnemyAIs)
            {
                kv.Value.Update();

                if (kv.Value.Character == BrawlerBattleManager.CurrentAttacker)
                {
                    BattleTurnManager.ActionStep phase = BattleTurnManager.CurrentActionStep;

                    if (phase == BattleTurnManager.ActionStep.Action || phase == BattleTurnManager.ActionStep.Ready)
                        if (!kv.Value.Character.IsDead())
                        {
                            if (kv.Key == attacker)
                                kv.Value.TurnUpdate(phase);
                        }
                }
            }
        }

        public static Fighter OnAttackerSelect(bool readOnly, bool getNextFighter)
        {
            if (Mod.DisableAttacksFromAI)
                return FighterManager.GetFighter(0);

            if (!FighterManager.IsBrawlerMode())
                return null;

            if(_OverrideNextAttackerOnce != null)
            {
                Fighter attacker = _OverrideNextAttackerOnce;
                _OverrideNextAttackerOnce = null;

                return attacker;
            }

            Fighter chosenEnemy = null;
            Fighter[] allEnemies = FighterManager.GetAllEnemies().Where(x => !x.IsDead()).ToArray();

            if (allEnemies.Length == 1)
                chosenEnemy = allEnemies[0];

            Random rnd = new Random();

            if (allEnemies.Length > 1)
            {
                allEnemies = allEnemies.OrderBy(x => Vector3.Distance((Vector3)FighterManager.GetFighter(0).Character.Transform.Position, (Vector3)x.Character.Transform.Position)).ToArray();
                chosenEnemy = allEnemies[rnd.Next(0, 2)]; //focus on the nearest three enemies.
            }
            else
                chosenEnemy = allEnemies[rnd.Next(0, allEnemies.Length)];

            BrawlerBattleManager.CurrentAttacker = chosenEnemy;

            return chosenEnemy;
        }

        private static EnemyAI CreateEnemyAI(Fighter enemy)
        {
            EnemyAI ai = null;

            switch(enemy.Character.Attributes.enemy_id)
            {
                case BattleRPGEnemyID.yazawa_boss_ushio_c01:
                    ai = new EnemyAIUshio();
                    break;
            }

            switch(enemy.Character.Attributes.soldier_data_id)
            {
                case CharacterNPCSoldierPersonalDataID.yazawa_btl03_0010_000_2:
                    ai = new EnemyAIHu();
                    break;
            }

            if (ai == null)
                ai = new EnemyAI();

            ai.Character = enemy;
            ai.Chara = enemy.Character;

            return ai;
        }

        private static EnemyAIBoss CreateBossAI(Fighter enemy)
        {
            EnemyAIBoss ai = null;

            switch(enemy.Character.Attributes.ctrl_type)
            {
                default:
                    ai = new EnemyAIBoss();
                    break;
                case BattleControlType.boss_majima_b:
                    ai = new EnemyAIMajima();
                    break;
                case BattleControlType.boss_saejima:
                    ai = new EnemyAISaejima();
                    break;
                case BattleControlType.boss_sawashiro_e:
                    ai = new EnemyAISawashiro2();
                    break;
            }

            ai.Character = enemy;
            ai.Chara = enemy.Character;

            return ai;
        }
    }
}
