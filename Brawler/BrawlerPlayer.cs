using System.Collections.Generic;
using DragonEngineLibrary;

namespace Brawler
{
    public static class BrawlerPlayer
    {
        private static Style[] m_Styles;
        private static Style m_currentStyle;

        private static float m_attackCooldown = 0;

        public static void Init()
        {
            m_Styles = GetStyles();
            m_currentStyle = m_Styles[0];
        }


        public static void InputUpdate()
        {

            Character kasugaPlayer = DragonEngine.GetHumanPlayer();
            Fighter kasugaFighter = kasugaPlayer.GetFighter();

            if (!kasugaFighter.IsValid())
                return;

            Fighter[] allEnemies = FighterManager.GetAllEnemies();

            if (allEnemies.Length <= 0)
                return;

            if (m_attackCooldown > 0)
                return;

            if (m_currentStyle != null)
                foreach (Move move in m_currentStyle.CommandSet.Moves)
                    if (move.CheckSimpleConditions(kasugaFighter) && move.AreInputKeysPressed())
                    {
                        ExecuteMove(move, kasugaFighter, allEnemies[0]);
                        break;
                    }
        }

        public static void GameUpdate()
        {
            if (!DragonEngine.GetHumanPlayer().Get().GetFighter().IsValid())
            {
                m_attackCooldown = 0;
                return;
            }

            if (m_attackCooldown > 0)
                m_attackCooldown -= DragonEngine.deltaTime;
        }

        public static void ExecuteMove(Move move, Fighter attacker, Fighter enemy)
        {
            m_attackCooldown = move.cooldown;
            BattleTurnManager.ForceCounterCommand(attacker, enemy, move.ID);
            
        }

        public static Style[] GetStyles()
        {
            //Legend, Rush, Crash


            /*
            if (DragonEngine.IsKeyDown(VirtualKey.F))
                BattleTurnManager.ForceCounterCommand(kasuga, enemies[0], RPGSkillID.boss_kiryu_atk_c);
            else
    if (DragonEngine.IsKeyDown(VirtualKey.G))
                BattleTurnManager.ForceCounterCommand(kasuga, enemies[0], RPGSkillID.boss_kiryu_atk_a);

            if (DragonEngine.IsKeyDown(VirtualKey.H))
                BattleTurnManager.ForceCounterCommand(kasuga, enemies[0], RPGSkillID.boss_kiryu_crash_atk_a);

            if (DragonEngine.IsKeyDown(VirtualKey.T))
                BattleTurnManager.ForceCounterCommand(kasuga, enemies[0], RPGSkillID.boss_kiryu_legend_atk_c);
                */

            //Legend
            Moveset legendMoveSet = new Moveset
            (

                new Move(RPGSkillID.boss_kiryu_legend_atk_c, 1f, new MoveInput[]
                {
                 //   new MoveInput(VirtualKey.Shift, true),
                    new MoveInput(VirtualKey.RightButton, false)
                }),

                new Move(RPGSkillID.boss_kiryu_atk_a, 0.5f, new MoveInput[]
                {
                    new MoveInput(VirtualKey.F, false)
                })

            );

            Style legendStyle = new Style(MotionID.E_KRL_BTL_SUD_styl_change, legendMoveSet);

            return new Style[]
            {
                legendStyle,
            };
        }
    }
}
