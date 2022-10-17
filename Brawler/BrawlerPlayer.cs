using System;
using System.Linq;
using System.Timers;
using System.Collections.Generic;
using DragonEngineLibrary;

namespace Brawler
{
    public static class BrawlerPlayer
    {
        public static BrawlerFighterInfo Info;

        public static Style[] m_Styles;
        private static Style m_currentStyle;

        public static Dictionary<AssetArmsCategoryID, Moveset> EXMovesets = new Dictionary<AssetArmsCategoryID, Moveset>();
        public static Dictionary<AssetArmsCategoryID, Moveset> EXHeats = new Dictionary<AssetArmsCategoryID, Moveset>();

        public static MotionPlayInfo MotionInfo;

        public static Moveset CurrentMoveset = null;
        public static Moveset CommonMoves = null;

        public static MoveBase m_lastMove = null;

        public static float m_attackCooldown = 0;
        public static float m_currentAttackTime = 0;

        public static Timer attackCancelTimer = null;

        public static bool ThrowingWeapon = false;

        public static bool IsEXGamer = false;
        public static bool WantSwapJobWeapon = false;
        public static bool WantTransform = false;

        public static bool FreezeInput = false;

        public static event Action OnPlayerGuard;

        public static void Init()
        {
            m_Styles = GetStyles();
            m_currentStyle = m_Styles[0];
        }


        //Should execute brawler input
        //Battle start done once
        //Input not frozen
        //Not downed
        //Not dead
        //Not ragdolled
        //Not sync
        //Not attacking
        //Not getting up
        public static bool GenericShouldExecuteAttack()
        {
            if (!Mod.ShouldExecBrawlerInput())
                return false;

            if (!BrawlerBattleManager.BattleStartDoOnce)
                return false;

            if (FreezeInput)
                return false;

            return !Info.IsDown && !Info.IsDead && !Info.IsSync && !Info.IsGettingUp && !Info.IsRagdoll &&
                    m_lastMove == null;
        }

        public static void ThrowWeapon()
        {
            Weapon wep = BrawlerBattleManager.Kasuga.GetWeapon(AttachmentCombinationID.right_weapon);
            AssetUnit unit = wep.Unit;

            if (unit.AssetID == AssetID.invalid)
                return;

            AssetArmsCategoryID wepCategory = Asset.GetArmsCategory(unit.AssetID);

            switch (wepCategory)
            {
                default:
                    BrawlerBattleManager.KasugaChara.GetMotion().RequestGMT(17075);
                    break;
            }

            ThrowingWeapon = true;
        }

        public static Fighter GetLockOnTarget(Fighter kasugaFighter, Fighter[] allEnemies)
        {
            if (BrawlerBattleManager.DisableTargetingOnce)
                return new Fighter(IntPtr.Zero);

            if (allEnemies.Length <= 0)
                return new Fighter(IntPtr.Zero);

            Fighter nearestFighter = allEnemies[0];
            FighterID nearestFighterID = nearestFighter.GetID();
            bool isLockingIn = BrawlerHooks.HumanModeManager_IsInputKamae(kasugaFighter.Character.HumanModeManager.Pointer);

            //always prioritize locking in
            if (isLockingIn)
                return nearestFighter;

            float distToNearestEnemy = Vector3.Distance((Vector3)kasugaFighter.Character.GetPosCenter(), (Vector3)nearestFighter.Character.GetPosCenter());

            if (allEnemies.Length < 2)
                return (distToNearestEnemy <= 3.5f ? nearestFighter : new Fighter(IntPtr.Zero));
            else
            {
                if (distToNearestEnemy <= 2.5f)
                    return nearestFighter;
            }

            return new Fighter(IntPtr.Zero);
        }

        public static void UpdateTargeting(Fighter kasugaFighter, Fighter[] allEnemies)
        {
            ECBattleTargetDecide targetDecide = kasugaFighter.Character.TargetDecide;
            targetDecide.SetTarget(GetLockOnTarget(kasugaFighter, allEnemies).GetID());
        }

        public static bool AllowDamage(BattleDamageInfo inf)
        {
            return TutorialManager.AllowPlayerDamage();
        }

        public static void OnGuard()
        {
            OnPlayerGuard?.Invoke();
        }

        public static void InputUpdate()
        {
            if (FreezeInput || !Mod.IsGameFocused)
                return;

            Fighter kasugaFighter = FighterManager.GetFighter(0);
            UpdateTargeting(kasugaFighter, BrawlerBattleManager.Enemies);

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

            AssetUnit unit = kasugaFighter.GetWeapon(AttachmentCombinationID.right_weapon).Unit;
            bool heatActionUpdate = false;

            if (m_lastMove == null || m_lastMove.AllowHActWhileExecuting())
                heatActionUpdate = HeatActionManager.InputUpdate(Asset.GetArmsCategory(unit.AssetID), Asset.GetArmsCategorySub(unit.AssetID));


            //we executed a heat action.
            if (heatActionUpdate || m_lastMove != null)
                return;


            bool wepUpdate = WeaponManager.InputUpdate(unit);

            if (!wepUpdate)
            {
                //cheap fix
                if (!IsEXGamer)
                    CurrentMoveset = m_currentStyle.CommandSet;
                // kasugaFighter.ThrowEquipAsset(false, true);
                if (DragonEngine.IsKeyHeld(VirtualKey.N1))
                    ChangeStyle(m_Styles[0]);
                else if (DragonEngine.IsKeyHeld(VirtualKey.N2))
                    ChangeStyle(m_Styles[1]);
                else if (DragonEngine.IsKeyHeld(VirtualKey.N3))
                    ChangeStyle(m_Styles[2]);
            }


            if (Mod.Input[BattleInput.Q].Pressed && !WantTransform)
            {
                RPGJobID job = Player.GetCurrentJob(Player.ID.kasuga);

                if (!IsEXGamer)
                {
                    if (kasugaFighter.GetStatus().Heat > 0)
                    {
                        IsEXGamer = true;
                        WantTransform = true;
                    }
                }
                else
                {
                    IsEXGamer = false;
                    WantTransform = true;
                }
            }

            Fighter[] allEnemies = BrawlerBattleManager.Enemies;

            if (CommonMoves != null)
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

        public static void GameUpdate()
        {
            Fighter kasugaFighter = FighterManager.GetFighter(0);
            Info.Update(kasugaFighter);

            MotionInfo = kasugaFighter.Character.GetMotion().PlayInfo;

            //Second condition is for player movement freezing on talk
            if (FreezeInput || (BattleTurnManager.CurrentPhase == BattleTurnManager.TurnPhase.Event && BattleTurnManager.CurrentActionStep == BattleTurnManager.ActionStep.Init))
                DragonEngine.GetHumanPlayer().Status.SetNoInputTemporary();

            foreach (var kv in Mod.Input)
            {
                if (kv.Value.Held || kv.Value.Pressed)
                {
                    kv.Value.TimeHeld += DragonEngine.deltaTime;
                    kv.Value.LastTimeSincePressed = 0;
                }
                else
                {
                    kv.Value.TimeHeld = 0;
                    kv.Value.LastTimeSincePressed += DragonEngine.deltaTime;
                }
            }

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
                    bool sameJob = kasugaFighter.GetStatus().ActionCommand == RPG.GetJobCommandSetID(Player.ID.kasuga, job);

                    if (!sameJob)
                    {
                        kasugaFighter.Character.Components.EffectEvent.Get().StopEvent((EffectEventCharaID)0x104, false);
                        kasugaFighter.Character.Components.EffectEvent.Get().PlayEventOverride((EffectEventCharaID)0x104);
                    }
                    FreezeInput = true;

                    if (IsEXGamer)
                        BrawlerBattleManager.OnEXGamerON();
                    else
                        BrawlerBattleManager.OnEXGamerOFF();

                    WantTransform = false;
                }

                if (WantSwapJobWeapon)
                {
                    RPGJobID job = Player.GetCurrentJob(Player.ID.kasuga);

                    //drop it
                    if (!FighterManager.GetFighter(0).IsValid() || kasugaFighter.GetStatus().ActionCommand != RPG.GetJobCommandSetID(Player.ID.kasuga, job))
                    {
                        kasugaFighter.DropWeapon(new DropWeaponOption(AttachmentCombinationID.right_weapon, true));
                        kasugaFighter.DropWeapon(new DropWeaponOption(AttachmentCombinationID.left_weapon, true));
                    }
                    else
                    {
                        //Enforcer: Dual weapons
                        if (job == RPGJobID.man_06)
                        {
                            kasugaFighter.Equip(Party.GetEquipItemID(Player.ID.kasuga, PartyEquipSlotID.weapon), AttachmentCombinationID.left_weapon);
                            kasugaFighter.Equip(ItemID.yazawa_pocket_weapon_adachi_004, AttachmentCombinationID.right_weapon);
                        }
                        else
                            kasugaFighter.Equip(Party.GetEquipItemID(Player.ID.kasuga, PartyEquipSlotID.weapon), AttachmentCombinationID.right_weapon);
                    }

                    WantSwapJobWeapon = false;
                }

            }

            foreach (var kv in Mod.Input)
            {
                if (kv.Value.Pressed)
                    kv.Value.Pressed = false;
            }

        }

        //Standard attacking, does not include heat actions
        public static void ExecuteMove(MoveBase move, Fighter attacker, Fighter[] enemy, bool force = false)
        {
            if (!force && m_lastMove != null && !m_lastMove.AllowChange())
                return;

            if (attackCancelTimer != null)
                attackCancelTimer.Enabled = false;

            m_lastMove = move;

            m_attackCooldown = move.cooldown;

            if (move.cooldown > 0.1f)
            {
                attackCancelTimer = new Timer() { Enabled = true, Interval = TimeSpan.FromSeconds(move.cooldown).TotalMilliseconds, AutoReset = false };
                attackCancelTimer.Elapsed += delegate { attacker.Character.HumanModeManager.ToEndReady(); };
            }

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
            BrawlerBattleManager.Kasuga.Character.GetMotion().RequestGMT(style.SwapAnimation);

            m_attackCooldown = 0.7f;
        }

        public static Style[] GetStyles()
        {

            CommonMoves = new Moveset
            (
                RPGSkillID.invalid,
                new MoveSidestep(0.25f, new MoveInput[]
                {
                    new MoveInput(BattleInput.Space, false)
                }, MoveSimpleConditions.FighterIsNotDown)
            );


            //Legend
            Moveset legendMoveSet = new Moveset
            (
                (RPGSkillID)1750,
                new MoveSidestep(0.25f, new MoveInput[]
                {
                    new MoveInput(BattleInput.Space, false)
                }, MoveSimpleConditions.FighterIsNotDown),

                //Stomp
                new MoveRPG((RPGSkillID)1752, 0.8f, new MoveInput[]
                {
                    new MoveInput(BattleInput.RightMouse, false)
                }, MoveSimpleConditions.LockedEnemyIsDown),

                new MoveString(new MoveString.AttackFrame[]
                {
                    new MoveString.AttackFrame(new FighterCommandID(1337, 1), new FighterCommandID(1337, 5), false, 1f),
                    new MoveString.AttackFrame(new FighterCommandID(1337, 2), new FighterCommandID(1337, 6), true, 0.4f, 0.8f),
                    new MoveString.AttackFrame(new FighterCommandID(1337, 3), new FighterCommandID(1337, 7), true, 0.7f, 0.8f),
                    new MoveString.AttackFrame(new FighterCommandID(1337, 4), new FighterCommandID(1337, 8), false, 0.4f, 0.8f),

                }, new MoveInput[]
                {
                    new MoveInput(BattleInput.LeftMouse, false)
                }, MoveSimpleConditions.FighterIsNotDown),

                new MoveGrab(new MoveInput[] { new MoveInput(BattleInput.E, false) }, MoveSimpleConditions.FighterIsNotDown)
                {
                    Grab = (RPGSkillID)194,
                    GrabAnim = (MotionID)17096, //p_yag_btl_mai_sy0_gsp_lp
                    GrabSync = (RPGSkillID)1759,
                    ShakeOff = (RPGSkillID)1754,
                    HitThrow = (RPGSkillID)1755,
                    HitLight = new RPGSkillID[] { (RPGSkillID)1756, (RPGSkillID)1757, (RPGSkillID)1758 },
                    HitHeavy = (RPGSkillID)1760
                },

                /*
                new MoveRPG((RPGSkillID)246, 0.001f, new MoveInput[]
                {
                    new MoveInput(BattleInput.E, false)
                }, MoveSimpleConditions.FighterIsNotDown, 3.5f),
                */

                /*
                new MoveRPG((RPGSkillID)243, 2.5f, new MoveInput[]
                {
                    new MoveInput(BattleInput.E, true),
                }, MoveSimpleConditions.FighterIsNotDown),
                */

                new MoveRPG(RPGSkillID.boss_kiryu_legend_atk_c, 1f, new MoveInput[]
                {
                    new MoveInput(BattleInput.LeftShift, true),
                    new MoveInput(BattleInput.RightMouse, false)
                }, MoveSimpleConditions.FighterIsNotDown)

            );

            Moveset rushMoveSet = new Moveset
            (
                RPGSkillID.invalid,
                new MoveGMTOnly(MotionID.C_COM_BTL_SUD_dwnB_to_dge_r, 1, new MoveInput[]
                {
                    new MoveInput(BattleInput.Space, false)
                }, MoveSimpleConditions.FighterIsDown),

                new MoveSidestep(0.1f, new MoveInput[]
                {
                    new MoveInput(BattleInput.Space, false)
                }, MoveSimpleConditions.FighterIsNotDown),

                new MoveRPG(RPGSkillID.boss_kiryu_rush_atk_a, 0.35f, new MoveInput[]
                {
                    new MoveInput(BattleInput.LeftMouse, false),
                }, MoveSimpleConditions.FighterIsNotDown),

                new MoveRPG(RPGSkillID.boss_kiryu_rush_atk_b, 0.35f, new MoveInput[]
                {
                    new MoveInput(BattleInput.RightMouse, false),
                }, MoveSimpleConditions.FighterIsNotDown)
            );


            Moveset crashMoveSet = new Moveset
            (
                RPGSkillID.invalid,
                new MoveGMTOnly(MotionID.C_COM_BTL_SUD_dwnB_to_dge_r, 1, new MoveInput[]
                {
                    new MoveInput(BattleInput.Space, false)
                }, MoveSimpleConditions.FighterIsDown),

                new MoveSidestep(0.1f, new MoveInput[]
                {
                    new MoveInput(BattleInput.Space, false)
                }, MoveSimpleConditions.FighterIsNotDown),

                new MoveRPG(RPGSkillID.boss_kiryu_atk_e, 0.5f, new MoveInput[]
                {
                    new MoveInput(BattleInput.LeftMouse, false),
                }, MoveSimpleConditions.FighterIsNotDown),

                new MoveRPG(RPGSkillID.boss_kiryu_crash_atk_a, 0.5f, new MoveInput[]
                {
                    new MoveInput(BattleInput.RightMouse, false),
                }, MoveSimpleConditions.FighterIsNotDown)
            );


            EXMovesets = new Dictionary<AssetArmsCategoryID, Moveset>()
            {
                [AssetArmsCategoryID.invalid] = new Moveset
            (
                (RPGSkillID)1751,
                new MoveSidestep(0.25f, new MoveInput[]
                {
                    new MoveInput(BattleInput.Space, false)
                }, MoveSimpleConditions.FighterIsNotDown),

                new MoveRPG(RPGSkillID.kasuga_job01_nml_atk_down, 1f, new MoveInput[]
                {
                    new MoveInput(BattleInput.RightMouse, false)
                }, MoveSimpleConditions.LockedEnemyIsDown, 3f),

                new MoveString(new MoveString.AttackFrame[]
                {
                    new MoveString.AttackFrame(new FighterCommandID(1337, 1), new FighterCommandID(1337, 5), false, 1f),
                    new MoveString.AttackFrame(new FighterCommandID(1337, 2), new FighterCommandID(1337, 6), true, 0.4f, 0.8f),
                    new MoveString.AttackFrame(new FighterCommandID(1337, 3), new FighterCommandID(1337, 7), true, 0.7f, 0.8f),
                    new MoveString.AttackFrame(new FighterCommandID(1337, 4), new FighterCommandID(1337, 8), false, 0.4f, 0.8f),

                }, new MoveInput[]
                {
                    new MoveInput(BattleInput.LeftMouse, false)
                }, MoveSimpleConditions.FighterIsNotDown),

                new MoveRPG((RPGSkillID)245, 3.5f, new MoveInput[]
                {
                    new MoveInput(BattleInput.E, false)
                }, MoveSimpleConditions.FighterIsNotDown),

                /*
                new MoveRPG((RPGSkillID)243, 2.5f, new MoveInput[]
                {
                    new MoveInput(BattleInput.E, true),
                }, MoveSimpleConditions.FighterIsNotDown),
                */

                new MoveRPG(RPGSkillID.boss_kiryu_legend_atk_c, 1f, new MoveInput[]
                {
                    new MoveInput(BattleInput.LeftShift, true),
                    new MoveInput(BattleInput.RightMouse, false)
                }, MoveSimpleConditions.FighterIsNotDown)

            )
            };

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
