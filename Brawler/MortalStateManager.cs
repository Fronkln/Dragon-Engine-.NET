using System;
using System.Collections.Generic;
using DragonEngineLibrary;

namespace Brawler
{
    public static class MortalStateManager
    {
        private const EffectEventCharaID m_mortalEntry = EffectEventCharaID.JudgeBossAuraOza_ed;
        private const EffectEventCharaID m_mortalLoop = EffectEventCharaID.JudgeBossAuraOza_lp;
        private const EffectEventCharaID m_mortalExit = EffectEventCharaID.JudgeBossAuraOza_st;

        private static List<EntityHandle<Character>> m_mortalEnemies = new List<EntityHandle<Character>>();

        private static bool ShouldEnterMortalState(Fighter fighter)
        {
            return fighter.GetStatus().CurrentHP > 0;
        }

        private static bool ShouldExitMortalState(Fighter fighter)
        {
            return fighter.GetStatus().CurrentHP <= 0;
        }

        private static void EnterMortalState(Fighter fighter)
        {
            m_mortalEnemies.Add(fighter.Character.UID);

            fighter.Character.Components.EffectEvent.Get().PlayEvent(m_mortalEntry, m_mortalLoop);
            fighter.GetStatus().SetSuperArmor(true);
            SoundManager.PlayCue(SoundCuesheetID.battle_common, 5, 0);
        }

        private static void ExitMortalState(Fighter fighter)
        {
            m_mortalEnemies.Remove(fighter.Character.UID);

            ECCharacterEffectEvent effect = fighter.Character.Components.EffectEvent.Get();

            effect.StopEvent(m_mortalLoop, false);
            effect.PlayEventOverride(m_mortalExit);
            SoundManager.PlayCue(SoundCuesheetID.battle_common, 5, 0);

            fighter.GetStatus().SetSuperArmor(false);
        }

        public static void Update()
        {
            Fighter[] enemies = FighterManager.GetAllEnemies();

            foreach (Fighter enemy in enemies)
                if (ShouldEnterMortalState(enemy))
                    if (!m_mortalEnemies.Contains(enemy.Character.UID))
                        EnterMortalState(enemy);

            EntityHandle<Character>[] curList = m_mortalEnemies.ToArray();

            foreach (EntityHandle<Character> enemy in curList)
            {
                Fighter enemyFighter = enemy.Get().GetFighter();

                if (enemy.IsValid() && ShouldExitMortalState(enemyFighter))
                    ExitMortalState(enemyFighter);
            }
        }
    }
}
