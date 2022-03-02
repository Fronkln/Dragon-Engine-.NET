using System.Collections.Generic;
using DragonEngineLibrary;

namespace Brawler
{
    public static class BrawlerPlayer
    {
        private static Style[] m_Styles;
        private static Style m_currentStyle;

        private const float BATTLE_START_TIME = 2.8f;

        private static bool m_battleStartDoOnce = false;

        private static float m_battleTime = 0;
        private static float m_attackCooldown = 0;

        public static void Init()
        {
            m_Styles = GetStyles();
        }


        public static void InputUpdate()
        {


            Fighter kasugaFighter = FighterManager.GetPlayer();

            if (!kasugaFighter.IsValid())
                return;

            if (kasugaFighter.IsDead())
                return;

            if (m_battleTime < BATTLE_START_TIME || m_attackCooldown > 0)
                return;


            Fighter[] allEnemies = FighterManager.GetAllEnemies();

            if (allEnemies.Length <= 0)
                return;


            if (allEnemies.Length == 1)
                if (allEnemies[0].IsDead())
                    return;

            if (DragonEngine.IsKeyDown(VirtualKey.T))
                kasugaFighter.ThrowEquipAsset(false, true);

            if (DragonEngine.IsKeyHeld(VirtualKey.N1))
                ChangeStyle(m_Styles[0]);
            else if (DragonEngine.IsKeyHeld(VirtualKey.N2))
                ChangeStyle(m_Styles[1]);
            else if (DragonEngine.IsKeyHeld(VirtualKey.N3))
                ChangeStyle(m_Styles[2]);

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
            if (!DragonEngine.GetHumanPlayer().GetFighter().IsValid())
            {
                m_battleStartDoOnce = false;
                m_battleTime = 0;
                m_attackCooldown = 0;
                return;
            }

            if (!m_battleStartDoOnce)
            {
                if (m_battleTime > BATTLE_START_TIME)
                {
                    if (m_currentStyle != null)
                        ChangeStyle(m_currentStyle, true);
                    else
                        ChangeStyle(m_Styles[0], true);

                    m_battleStartDoOnce = true;
                }
            }

            if (m_attackCooldown > 0)
                m_attackCooldown -= DragonEngine.deltaTime;

            m_battleTime += DragonEngine.deltaTime;
        }

        public static void ExecuteMove(Move move, Fighter attacker, Fighter enemy)
        {
            m_attackCooldown = move.cooldown;
            move.Execute(attacker, enemy);
        }

        public static void ChangeStyle(Style style, bool force = false)
        {
            if (m_currentStyle == style && !force)
                return;

            m_currentStyle = style;
            DragonEngine.GetHumanPlayer().GetMotion().RequestGMT(style.SwapAnimation);

            m_attackCooldown = 0.4f;
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

                new MoveGetUp(1, new MoveInput[]
                {
                    new MoveInput(VirtualKey.Space, false)
                }, MoveSimpleConditions.FighterIsDown),

                new MoveSidestep(0.1f, new MoveInput[]
                {
                    new MoveInput(VirtualKey.Space, false)
                }, MoveSimpleConditions.FighterIsNotDown),

                new Move(RPGSkillID.boss_kiryu_legend_atk_b, 0.5f, new MoveInput[]
                {
                    new MoveInput(VirtualKey.LeftButton, false)
                }, MoveSimpleConditions.FighterIsNotDown),

                new Move(RPGSkillID.boss_kiryu_legend_atk_c, 1f, new MoveInput[]
                {
                    new MoveInput(VirtualKey.LeftShift, true),
                    new MoveInput(VirtualKey.RightButton, false)
                }, MoveSimpleConditions.FighterIsNotDown),

                new Move(RPGSkillID.boss_kiryu_legend_atk_d, 0.5f, new MoveInput[]
                {
                    new MoveInput(VirtualKey.RightButton, false)
                }, MoveSimpleConditions.FighterIsNotDown),

                new Move(RPGSkillID.boss_kiryu_atk_a, 0.5f, new MoveInput[]
                {
                    new MoveInput(VirtualKey.F, false)
                }, MoveSimpleConditions.FighterIsNotDown)

            );

            Moveset rushMoveSet = new Moveset
            (
                new MoveGMTOnly(MotionID.C_COM_BTL_SUD_dwnB_to_dge_r, 1, new MoveInput[]
                {
                    new MoveInput(VirtualKey.Space, false)
                }, MoveSimpleConditions.FighterIsDown),

                new MoveSidestep(0.1f, new MoveInput[]
                {
                    new MoveInput(VirtualKey.Space, false)
                }, MoveSimpleConditions.FighterIsNotDown),

                new Move(RPGSkillID.boss_kiryu_rush_atk_a, 0.35f, new MoveInput[]
                {
                    new MoveInput(VirtualKey.LeftButton, false),
                }, MoveSimpleConditions.FighterIsNotDown),

                new Move(RPGSkillID.boss_kiryu_rush_atk_b, 0.35f, new MoveInput[]
                {
                    new MoveInput(VirtualKey.RightButton, false),
                }, MoveSimpleConditions.FighterIsNotDown)
            );


            Moveset crashMoveSet = new Moveset
            (
                new MoveGMTOnly(MotionID.C_COM_BTL_SUD_dwnB_to_dge_r, 1, new MoveInput[]
                {
                    new MoveInput(VirtualKey.Space, false)
                }, MoveSimpleConditions.FighterIsDown),

                new MoveSidestep(0.1f, new MoveInput[]
                {
                    new MoveInput(VirtualKey.Space, false)
                }, MoveSimpleConditions.FighterIsNotDown),

                new Move(RPGSkillID.boss_kiryu_atk_e, 0.5f, new MoveInput[]
                {
                    new MoveInput(VirtualKey.LeftButton, false),
                }, MoveSimpleConditions.FighterIsNotDown),

                new Move(RPGSkillID.boss_kiryu_crash_atk_a, 0.5f, new MoveInput[]
                {
                    new MoveInput(VirtualKey.RightButton, false),
                }, MoveSimpleConditions.FighterIsNotDown)
            );


            Style legendStyle = new Style(MotionID.E_KRL_BTL_SUD_styl_change, legendMoveSet);
            Style rushStyle = new Style(MotionID.E_KRH_BTL_SUD_styl_change, rushMoveSet);
            Style crashStyle = new Style(MotionID.E_KRC_BTL_SUD_styl_change, crashMoveSet);

            return new Style[]
            {
                legendStyle,
                rushStyle,
                crashStyle,
            };
        }
    }
}
