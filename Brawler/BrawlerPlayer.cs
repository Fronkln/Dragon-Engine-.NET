using System;
using System.Linq;
using System.Timers;
using System.Collections.Generic;
using DragonEngineLibrary;

namespace Brawler
{
    public static class BrawlerPlayer
    {
        private static Style[] m_Styles;
        private static Style m_currentStyle;

        public static Moveset CurrentMoveset = null;
        public static Moveset CommonMoves = null;

        public static MoveBase m_lastMove = null;

        public static float m_attackCooldown = 0;
        public static float m_currentAttackTime = 0;

        public static Timer attackCancelTimer = null;

        public static bool IsEXGamer = false;
        public static bool WantSwapJobWeapon = false;
        public static bool WantTransform = false;

        public static bool FreezeInput = false;

        public static void Init()
        {
            m_Styles = GetStyles();
            m_currentStyle = m_Styles[0];
        }

        public static void UpdateTargeting(Fighter[] targets)
        {

        }


        public static void EnterEXGamerState()
        {
            Fighter kasugaFighter = FighterManager.GetFighter(0);

            BattleManager.AllowPlayerTransformationDoOnce = true;

            SimpleTimer timer = new SimpleTimer(0.5f,
                delegate
                {
                    BattleCommandSetID commandSet = RPG.GetJobCommandSetID(Player.ID.kasuga, Player.GetCurrentJob(Player.ID.kasuga));

                    kasugaFighter.Character.GetRender().BattleTransformationOn();
                    WantSwapJobWeapon = true;
                    // kasugaFighter.Equip(Party.GetEquipItemID(Player.ID.kasuga, PartyEquipSlotID.weapon), AttachmentCombinationID.right_weapon);
                    kasugaFighter.Character.GetBattleStatus().ActionCommand = commandSet;
                    kasugaFighter.Character.SetCommandSet(commandSet);
                    kasugaFighter.GetStatus().SetSuperArmor(true);
                    FreezeInput = false;
                    IsEXGamer = true;
                }
                );
        }

        public static void ExitEXGamerState()
        {
            Fighter kasugaFighter = FighterManager.GetFighter(0);
            FreezeInput = true;

            SimpleTimer timer = new SimpleTimer(0.5f,
                delegate
                {
                    BattleCommandSetID commandSet = BattleCommandSetID.p_kasuga_job_01;

                    kasugaFighter.Character.GetRender().BattleTransformationOff();
                    WantSwapJobWeapon = true;
                    //  kasugaFighter.DropWeapon(new DropWeaponOption(AttachmentCombinationID.right_weapon, true));
                    kasugaFighter.Character.GetBattleStatus().ActionCommand = commandSet;
                    kasugaFighter.Character.SetCommandSet(commandSet);
                    kasugaFighter.GetStatus().SetSuperArmor(false);
                    FreezeInput = false;
                    IsEXGamer = false;
                }
                );
        }

        public static void InputUpdate()
        {
            if (FreezeInput)
                return;

            Fighter kasugaFighter = FighterManager.GetFighter(0);
            Fighter[] allEnemies = FighterManager.GetAllEnemies().Where(x => !x.IsDead()).OrderBy(x => Vector3.Distance((Vector3)kasugaFighter.Character.GetPosCenter(), (Vector3)x.Character.GetPosCenter())).ToArray();

            if (allEnemies.Length > 0 && Vector3.Distance((Vector3)kasugaFighter.Character.GetPosCenter(), (Vector3)allEnemies[0].Character.GetPosCenter()) <= 6)
                kasugaFighter.Character.TargetDecide.SetTarget(allEnemies[0].GetID());
            else
                kasugaFighter.Character.TargetDecide.SetTarget(new FighterID() { Handle = 0 });

            if (m_lastMove != null)
            {
                m_lastMove.InputUpdate();

                bool executing = m_lastMove.MoveExecuting();

                if (!executing)
                {
                    m_lastMove.OnMoveEnd();
                    m_lastMove = null;
                }
            }
            else
            {

                AssetUnit unit = kasugaFighter.GetWeapon(AttachmentCombinationID.right_weapon).Unit;
                bool heatActionUpdate = HeatActionManager.InputUpdate(Asset.GetArmsCategory(unit.AssetID));

                //we executed a heat action.
                if (heatActionUpdate)
                    return;


                bool wepUpdate = WeaponManager.InputUpdate(unit);

                if (!wepUpdate)
                {
                    CurrentMoveset = m_currentStyle.CommandSet;

                    if (DragonEngine.IsKeyDown(VirtualKey.T))
                        kasugaFighter.ThrowEquipAsset(false, true);
                    if (DragonEngine.IsKeyHeld(VirtualKey.N1))
                        ChangeStyle(m_Styles[0]);
                    else if (DragonEngine.IsKeyHeld(VirtualKey.N2))
                        ChangeStyle(m_Styles[1]);
                    else if (DragonEngine.IsKeyHeld(VirtualKey.N3))
                        ChangeStyle(m_Styles[2]);
                }


                if (DragonEngine.IsKeyDown(VirtualKey.Q))
                {
                    RPGJobID job = Player.GetCurrentJob(Player.ID.kasuga);

                    if (job != RPGJobID.kasuga_freeter)
                        WantTransform = true;
                }

                if(CommonMoves != null)
                {
                    foreach (MoveBase move in CommonMoves.Moves)
                        if (move.AreInputKeysPressed() && move.CheckConditions(kasugaFighter, allEnemies))
                        {
                            ExecuteMove(move, kasugaFighter, allEnemies);
                            return; //else the code below for currentmoveset will execute
                        }
                }

                if (CurrentMoveset != null)
                    foreach (MoveBase move in CurrentMoveset.Moves)
                        if (move.AreInputKeysPressed() && move.CheckConditions(kasugaFighter, allEnemies))
                        {
                            ExecuteMove(move, kasugaFighter, allEnemies);
                            break;
                        }


            }
        }

        public static void GameUpdate()
        {
            Fighter kasugaFighter = FighterManager.GetFighter(0);

            if (FreezeInput)
                DragonEngine.GetHumanPlayer().Status.SetNoInputTemporary();

            float timeDelta = DragonEngine.deltaTime;

            if (m_attackCooldown > 0)
            {
                m_attackCooldown -= timeDelta;
                m_currentAttackTime += timeDelta;
            }

            if (m_lastMove != null && m_lastMove.MoveExecuting())
                m_lastMove.Update();
            else
            {
                if (WantTransform)
                {
                    RPGJobID job = Player.GetCurrentJob(Player.ID.kasuga);
                    kasugaFighter.Character.Components.EffectEvent.Get().StopEvent((EffectEventCharaID)0x104, false);
                    kasugaFighter.Character.Components.EffectEvent.Get().PlayEventOverride((EffectEventCharaID)0x104);
                    FreezeInput = true;

                    if (kasugaFighter.GetStatus().ActionCommand != RPG.GetJobCommandSetID(Player.ID.kasuga, job))
                        EnterEXGamerState();
                    else
                        ExitEXGamerState();

                    WantTransform = false;
                }

                if(WantSwapJobWeapon)
                {
                    RPGJobID job = Player.GetCurrentJob(Player.ID.kasuga);

                    //drop it
                    if (!FighterManager.GetFighter(0).IsValid() || kasugaFighter.GetStatus().ActionCommand != RPG.GetJobCommandSetID(Player.ID.kasuga, job))
                        kasugaFighter.DropWeapon(new DropWeaponOption(AttachmentCombinationID.right_weapon, true));
                    else
                        kasugaFighter.Equip(Party.GetEquipItemID(Player.ID.kasuga, PartyEquipSlotID.weapon), AttachmentCombinationID.right_weapon);

                    WantSwapJobWeapon = false;
                }

            }

        }

        //Standard attacking, does not include heat actions
        public static void ExecuteMove(MoveBase move, Fighter attacker, Fighter[] enemy)
        {
            if (m_lastMove != null && !m_lastMove.AllowChange())
                return;

            if (attackCancelTimer != null)
                attackCancelTimer.Enabled = false;

            m_lastMove = move;

            m_attackCooldown = move.cooldown;
            attackCancelTimer = new Timer() { Enabled = true, Interval = TimeSpan.FromSeconds(move.cooldown).TotalMilliseconds, AutoReset = false };
            attackCancelTimer.Elapsed += delegate { attacker.Character.HumanModeManager.ToEndReady(); };

            m_currentAttackTime = 0;
            move.Execute(attacker, enemy);
        }

        public static void ChangeStyle(Style style, bool force = false)
        {
            if (m_currentStyle == style && !force)
                return;

            if (style == null)
            {
                if (m_currentStyle != null)
                    style = m_currentStyle;
                else
                    style = m_Styles[0];
            }

            m_currentStyle = style;
            CurrentMoveset = m_currentStyle.CommandSet;
            DragonEngine.GetHumanPlayer().GetMotion().RequestGMT(style.SwapAnimation);

            m_attackCooldown = 0.7f;
        }

        public static Style[] GetStyles()
        {

            CommonMoves = new Moveset
            (
                new MoveSidestep(0.25f, new MoveInput[]
                {
                    new MoveInput(VirtualKey.Space, false)
                }, MoveSimpleConditions.FighterIsNotDown)
            );


            //Legend
            Moveset legendMoveSet = new Moveset
            (
                new MoveSidestep(0.25f, new MoveInput[]
                {
                    new MoveInput(VirtualKey.Space, false)
                }, MoveSimpleConditions.FighterIsNotDown),

                new MoveString(new MoveString.AttackFrame[]
                {
                    new MoveString.AttackFrame(new FighterCommandID(1337, 1), 0.5f, 1f),
                    new MoveString.AttackFrame(new FighterCommandID(1337, 2), 0.4f, 0.4f, 0.8f),
                    new MoveString.AttackFrame(new FighterCommandID(1337, 3), 0.5f, 0.7f, 0.8f),
                    new MoveString.AttackFrame(new FighterCommandID(1337, 4), 0.5f, 0.4f, 0.8f),

                }, new MoveInput[]
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

            Style legendStyle = new Style((MotionID)17033, legendMoveSet);
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
