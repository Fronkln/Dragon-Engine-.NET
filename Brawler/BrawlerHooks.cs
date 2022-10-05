using System;
using System.Linq;
using System.Runtime.InteropServices;
using DragonEngineLibrary;
using MinHook.NET;

namespace Brawler
{
    public static class BrawlerHooks
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private delegate void BattleTurnManagerRequestWarpFighter(IntPtr mng, IntPtr fighter, IntPtr inf, IntPtr res);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private delegate void BattleTurnManagerDmgNotify(IntPtr mng, IntPtr inf);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private delegate void BattleTurnManagerRequestShowMiss(IntPtr mng, IntPtr fighter);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private delegate bool HumanModeManagerIsInputKamae(IntPtr mng);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private unsafe delegate bool HumanModeManagerTransitDamageCounter(IntPtr thisPtr, IntPtr args);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private delegate void EffectEventPlay(IntPtr effect, EffectEventCharaID id);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private delegate bool FighterTP(IntPtr character, IntPtr tpinf);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private delegate bool ECRenderCharacterBattleTransformOn(IntPtr character);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private delegate void DamageNodeProcHook(IntPtr node, IntPtr damageInf, IntPtr fighter);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private delegate float UICalcHealthGaugeWidth(float hp_max, float width_min, float width_max, float fit_range_min, float fit_range_max);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private delegate bool HijackedGuardFunc(IntPtr fighter);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private delegate void BattleTurnManagerChangePhase(IntPtr mng, BattleTurnManager.TurnPhase phase);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private unsafe delegate void TimingDropAssetPlay(IntPtr node, uint tick, IntPtr matrix, uint* parentHandle);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private unsafe delegate void GuardReactionSetMotionID(IntPtr guardReactionOb);

        public static void Init()
        {
            _kamaeDeleg = new HumanModeManagerIsInputKamae(HumanModeManager_IsInputKamae);
            _guardDeleg = new HumanModeManagerIsInputKamae(HumanModeManager_IsInputGuard);
            _pickupDeleg = new HumanModeManagerIsInputKamae(HumanModeManager_IsInputPickup);
            _effectEventPlayDeleg = new EffectEventPlay(EffectEvent_Play);
            _btlTurnManagerDmgNotifyDeleg = new BattleTurnManagerDmgNotify(BattleTurnManager_OnAfterDamage);
            _btlTurnManagerRequestShowMissDeleg = new BattleTurnManagerRequestShowMiss(BattleTurnManager_RequestShowMiss);
            _btlTurnManagerRequestWarpFighterDeleg = new BattleTurnManagerRequestWarpFighter(BattleTurnManager_RequestWarpFighter);
            _btlTurnManagerChangePhaseDeleg = new BattleTurnManagerChangePhase(BattleTurnManager_ChangePhase);
            _ecRenderCharacterBattleTransformOnDeleg = new ECRenderCharacterBattleTransformOn(ECRenderCharacter_BattleTransformOn);
            _dmgDeleg = new DamageNodeProcHook(DmgProc);
            _calcHpDeleg = new UICalcHealthGaugeWidth(CalcHealthGaugeWidth);

            unsafe
            {
                _justCounterValidEventDeleg = new HumanModeManagerTransitDamageCounter(JustGuard_ValidEvent);
                _dropPlayDeleg = new TimingDropAssetPlay(TimingDropAsset_Play);
                _guardReactionIDDeleg = new GuardReactionSetMotionID(GuardReaction_SetMotionID);
            }

            _npcGuardDeleg = new HijackedGuardFunc(HijackedGuardFunct);

            //DragonEngineLibrary.Unsafe.CPP.NopMemory((IntPtr)0x14094E919, 5);

            try
            {
                MinHookHelper.initialize();
            }
            catch { }

            MinHookHelper.createHook((IntPtr)0x1406D7BE0, _kamaeDeleg, out _kamaeTrampoline);
            MinHookHelper.createHook((IntPtr)0x1406D7C50, _guardDeleg, out _guardTrampoline);
            MinHookHelper.createHook((IntPtr)0x1406D8A50, _pickupDeleg, out _pickupTrampoline);
            MinHookHelper.createHook((IntPtr)0x1407ECF00, _effectEventPlayDeleg, out _effectEventPlayTrampoline);
            MinHookHelper.createHook((IntPtr)0x1404D2B10, _btlTurnManagerDmgNotifyDeleg, out _btlTurnManagerDmgNotifyTrampoline);
            MinHookHelper.createHook((IntPtr)0x140FF0ED0, _btlTurnManagerRequestShowMissDeleg, out _btlTurnManagerDmgRequestShowMissTrampoline);
            MinHookHelper.createHook((IntPtr)0x1404DDEF0, _btlTurnManagerRequestWarpFighterDeleg, out _btlTurnManagerRequestWarpFighterTrampoline);
            MinHookHelper.createHook((IntPtr)0x1404C8AE0, _btlTurnManagerChangePhaseDeleg, out _btlTurnManagerChangePhaseTrampoline);
            MinHookHelper.createHook((IntPtr)0x1407E2B20, _ecRenderCharacterBattleTransformOnDeleg, out _ecRenderCharacterBattleTransformOnTrampoline);
            MinHookHelper.createHook((IntPtr)0x140944EE0, _justCounterValidEventDeleg, out _justCounterValidEventTrampoline);
            MinHookHelper.createHook((IntPtr)0x157A9E640, _dropPlayDeleg, out _dropPlayTrampoline);

            //JUDGMENT UI GAUGE: Replace the Judgment check with Yazawa (YLAD)
            DragonEngineLibrary.Unsafe.CPP.PatchMemory((IntPtr)0x1410E4C8C, new byte[] { 0x7 });

            //JUDGMENT UI GAUGE: Create fake function at the address because MinHook believes it isnt a valid one.
            DragonEngineLibrary.Unsafe.CPP.PatchMemory((IntPtr)0x1411F45C0, new byte[] { 0xB0, 0x01, 0xC3 });
            MinHookHelper.createHook((IntPtr)0x1411F45C0, _calcHpDeleg, out _calcHpTrampoline);

            //GUARDING: Swap condition, player will have its humanmode checked. Enemies will use a hijacked function
            DragonEngineLibrary.Unsafe.CPP.PatchMemory((IntPtr)0x157B1E062, 0x75);
            DragonEngineLibrary.Unsafe.CPP.PatchMemory((IntPtr)0x157B1E07B, 0xE8, 0x40, 0x00, 0x46, 0xE9); //call hijacked func

            //CRANE TRUCK: prevent teleportation
            DragonEngineLibrary.Unsafe.CPP.NopMemory((IntPtr)0x140256BCF, 5);
            DragonEngineLibrary.Unsafe.CPP.NopMemory((IntPtr)0x140257156, 5); 

            DragonEngineLibrary.Unsafe.CPP.NopMemory((IntPtr)0x157B1E080, 7);
            DragonEngineLibrary.Unsafe.CPP.PatchMemory((IntPtr)0x157B1E087, 0x84, 0xC0, 0x90);
            DragonEngineLibrary.Unsafe.CPP.PatchMemory((IntPtr)0x157B1E08C, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x74); ;
            MinHookHelper.createHook((IntPtr)0x140F7E0C0, _npcGuardDeleg, out _npcGuardTrampoline);
            MinHookHelper.createHook((IntPtr)0x1406F2E90, _guardReactionIDDeleg, out _guardReactionIDTrampoline);
            MinHookHelper.createHook((IntPtr)0x14094E540, _dmgDeleg, out _dmgTrampoline);

            //COMBAT: Remove teleportation of player after combat ends
            DragonEngineLibrary.Unsafe.CPP.PatchMemory((IntPtr)0x1404CD190, 0xC3);

            MinHookHelper.enableAllHook();
        }


        private static GuardReactionSetMotionID _guardReactionIDDeleg;
        private static GuardReactionSetMotionID _guardReactionIDTrampoline;
        private unsafe static void GuardReaction_SetMotionID(IntPtr guardReactionObj)
        {
            //GuardReaction provides incomplete data for its damage info.
            //Solution? We set flags before and check them here

            long* mngPtr = (long*)(guardReactionObj.ToInt64() + 0x28);
            uint* motionID = (uint*)(guardReactionObj.ToInt64() + 0x78);

            HumanModeManager manager = new HumanModeManager() { Pointer = (IntPtr)(*mngPtr) };
            Fighter fighter = manager.Human.GetFighter();

            if (!fighter.IsValid() || !fighter.IsEnemy() || !EnemyManager.EnemyAIs.ContainsKey(fighter.Character.UID))
            {
                _guardReactionIDTrampoline(guardReactionObj);
                return;
            }

            EnemyAI ai = EnemyManager.EnemyAIs[fighter.Character.UID];

            if (ai.Flags.ShouldGuardBreakFlag)
            {
                Console.WriteLine("GUARD BREAK!");
                *motionID = 5542;
                ai.Flags.ShouldGuardBreakFlag = false;
            }
            else
            {
                //TODO: Fix AI code. Add boss traits to goons but never activate them
                if(ai.IsBoss())
                {
                    if ((ai as EnemyAIBoss).BlockModule.OnBlockedAnimEvent())
                        return;
                }

                _guardReactionIDTrampoline(guardReactionObj);
            }
        }

        private static HijackedGuardFunc _npcGuardDeleg;
        private static HijackedGuardFunc _npcGuardTrampoline;
        private static bool HijackedGuardFunct(IntPtr fighterPtr)
        {

            return true;

            /*
            Fighter fighter = new Fighter(fighterPtr);

            uint uid = fighter.Character.UID;

            if (!EnemyManager.EnemyAIs.ContainsKey(uid))
                return false;

            //Hacky for now
            EnemyAI ai = EnemyManager.EnemyAIs[fighter.Character.UID];

            if (!BrawlerPlayer.IsEXGamer)
            {
                ECBattleStatus kasugaStatus = BrawlerBattleManager.Kasuga.GetStatus();
                int maxHeat = Player.GetHeatMax(Player.ID.kasuga);

                //player will recover 5% heat for each hit
                if (kasugaStatus.Heat < maxHeat)
                    kasugaStatus.Heat = kasugaStatus.Heat + (int)(maxHeat * 0.05f);
            }

            bool blocked = ai.ShouldBlockAttack();

            if (blocked)
                ai.OnBlocked();

            ai.LastHitTime = 0;

            return blocked;
            */
        }

        private static UICalcHealthGaugeWidth _calcHpDeleg;
        private static UICalcHealthGaugeWidth _calcHpTrampoline;
        private static float CalcHealthGaugeWidth(float hp_max, float width_min, float width_max, float fit_range_min, float fit_range_max)
        {
            if (hp_max > width_max)
                return width_max;

            if (hp_max < width_min)
                return width_min;

            return hp_max;
        }

        private static DamageNodeProcHook _dmgDeleg;
        private static DamageNodeProcHook _dmgTrampoline;
        private static void DmgProc(IntPtr node, IntPtr dmg, IntPtr fighter)
        {


            unsafe
            {
                TimingInfoDamage* damage = (TimingInfoDamage*)dmg;

                //Bep/Sync damage
                if (damage->attaker > 5 || !HActManager.IsPlaying())
                {
                    _dmgTrampoline(node, dmg, fighter);
                    return;
                }

                Fighter victim = new Fighter(fighter);


                //Player attack. Thog care
                //A poorly implemented damage function
                if (damage->attaker == 0 || damage->attaker == 3)
                    ProcessDamage(FighterManager.GetFighter(0));

                //Closest enemy to player
                if (damage->attaker == 4)
                    if (BrawlerBattleManager.Enemies.Length > 0)
                        ProcessDamage(BrawlerBattleManager.Enemies[0]);


                void ProcessDamage(Fighter attacker)
                {
                    ECBattleStatus victimStatus = victim.GetStatus();
                    ECBattleStatus attackerStatus = attacker.GetStatus();

                    long finalDmg = 0;
                    float hpPercentage = 0;
                    long hpDamage = 0;

                    long calculatedDamage = 0;

                    float* directDmg = (float*)&damage->direct_damage;
                    float damageCap = *(float*)&damage->attack_id; //repurposed for damage cap

                    if (damage->direct_damage_is_hp_ratio == 1)
                    {
                        hpPercentage = *directDmg;
                        hpDamage = ((long)(victimStatus.MaxHP * hpPercentage));
                    }
                    else
                        hpDamage = damage->direct_damage;

                    uint victimDefense = victimStatus.DefensePower;
                    uint playerAttack = attackerStatus.AttackPower;

                    const float atkRatio = 0.55f;
                    const float defRatio = 1.25f;

                    if (damage->damage > 1)
                    {
                        long atkIncrease = (long)(playerAttack * atkRatio);
                        long dmgReduction = (long)(victimDefense * defRatio); 

                        calculatedDamage = damage->damage + atkIncrease - dmgReduction;
                    }

                    finalDmg = calculatedDamage + hpDamage;

                    if (finalDmg == 0)
                        finalDmg = 1;

#if DEBUG
                    string dbgText = "\n(";

                    if (damage->no_dead == 1)
                        dbgText += "NON ";

                    dbgText += "LETHAL)";
                    dbgText +=
                        $"\nCALCULATED: {calculatedDamage} {(calculatedDamage > 0 ? $"({damage->damage} + {(uint)(playerAttack * atkRatio)} - {(uint)(playerAttack * defRatio)})" : "")}" +
                        $"\nMAX HP DMG: {hpDamage} {(hpPercentage > 0 ? $"({victimStatus.MaxHP} * {hpPercentage})" : "")}" +
                        $"\nTOTAL: {finalDmg}";


                    Console.WriteLine(dbgText);
#endif

                    //Percentage
                    if (damageCap > 0)
                    {
                        long cappedDamage = (long)(victimStatus.MaxHP * damageCap);

                        //enemy has so little health this failed, 10% HP cap instead
                        if(cappedDamage == 0)
                            cappedDamage = (long)(victimStatus.MaxHP * 0.1f);

                        if (finalDmg > cappedDamage)
                        {
                            finalDmg = cappedDamage;
#if DEBUG
                            Console.WriteLine("DAMAGE WAS TOO HIGH, WAS CAPPED TO " + cappedDamage + $"({hpPercentage * 100} of max HP)");
#endif
                        }
                    }

                    if (victim.IsBoss())
                        EnemyManager.EnemyAIs[victim.Character.UID].ProcessHActDamage(HeatAction.LastHeatAction.heatAction, finalDmg);

                    long finalHp = victimStatus.CurrentHP - finalDmg;

                    bool dieThroughNormalMeans = finalHp <= 0 && damage->no_dead == 0;
                    bool forceDeath = damage->force_dead == 1;
                    bool dontDie = damage->no_dead == 1;

                    bool shouldDie = (dieThroughNormalMeans || forceDeath) && !dontDie;

                    //Yakuza 7 Bug: Fighters dying before hacts ending can cause hanging if they are the last
                    if (shouldDie)
                        victim.Character.ToDead();
                    else
                    {
                        if (finalHp <= 0)
                            finalHp = 1;
                    }

                    victimStatus.SetHPCurrent(finalHp);
                    return;
                }

            }

            _dmgTrampoline(node, dmg, fighter);
        }


        //This hook will be utilized for attack counters
        private static HumanModeManagerTransitDamageCounter _justCounterValidEventDeleg;
        private static HumanModeManagerTransitDamageCounter _justCounterValidEventTrampoline;
        private unsafe static bool JustGuard_ValidEvent(IntPtr thisPtr, IntPtr args)
        {
            Fighter victim = new Fighter((IntPtr)(((CalcDamageEventArgs*)args)->damage_fighter_));
            CalcDamageEventArgs* argsPtr = (CalcDamageEventArgs*)args;
            BattleDamageInfo* damageInf = argsPtr->info;

            if (!victim.IsPlayer())
            {
                Fighter fighter = victim;

                uint uid = fighter.Character.UID;

                if (!EnemyManager.EnemyAIs.ContainsKey(uid))
                    return false;

                //Hacky for now
                EnemyAI ai = EnemyManager.EnemyAIs[fighter.Character.UID];

                if (!BrawlerPlayer.IsEXGamer)
                {
                    ECBattleStatus kasugaStatus = BrawlerBattleManager.Kasuga.GetStatus();
                    int maxHeat = Player.GetHeatMax(Player.ID.kasuga);

                    //player will recover 5% heat for each hit
                    if (kasugaStatus.Heat < maxHeat)
                        kasugaStatus.Heat = kasugaStatus.Heat + (int)(maxHeat * 0.05f);
                }

                bool special = ai.DoSpecial(*argsPtr->info);
                bool blocked = (special ? false : ai.ShouldBlockAttack(*argsPtr->info));

                //BUG: guard break interrupts counter attack
                if (blocked)
                {
                    uint brawlerSpecialProperty = *((uint*)((uint)damageInf + 0xE8));

                    bool wouldHaveCounterAttacked = ai.EvasionModule.ShouldDoCounterAttack();

                    //YLAD Brawler: Guard Break
                    if (!wouldHaveCounterAttacked && brawlerSpecialProperty == 99999)
                        ai.OnGuardBroke();
                    else
                        ai.OnBlocked();
                }

                if(special)
                {
                    uint* dmg = (uint*)((long)argsPtr->info + 0x64);
                    bool* miss = (bool*)((long)argsPtr->info + 0xA9);

                    *dmg = 0;
                    *miss = true;
                }

                ai.OnHit();

                return blocked;
            }
            else
            {
                if (!BrawlerPlayer.AllowDamage(*damageInf))
                {
                    uint* dmg = (uint*)((long)argsPtr->info + 0x64);
                    bool* miss = (bool*)((long)argsPtr->info + 0xA9);

                    *dmg = 0;
                }
            }

            if (BrawlerPlayer.CurrentMoveset.RepelCounter == RPGSkillID.invalid)
                return false;

            Character attacker = new EntityHandle<Character>(*((uint*)((long)argsPtr->info + 0x58)));

            if (Vector3.Distance((Vector3)victim.Character.Transform.Position, (Vector3)attacker.Transform.Position) > 1.8f)
                return false;

            //Locked into the enemy that tried to attack us.
            if (BrawlerPlayer.GetLockOnTarget(BrawlerBattleManager.Kasuga, BrawlerBattleManager.Enemies).Character.UID == attacker.UID)
            {
                //We perfect guarded
                if (Mod.Input[BattleInput.LeftShift].TimeHeld > 0 && Mod.Input[BattleInput.LeftShift].TimeHeld <= 0.7f)
                {
                    uint* dmg = (uint*)((long)argsPtr->info + 0x64);
                    bool* miss = (bool*)((long)argsPtr->info + 0xA9);

                    *dmg = 0;
                    *miss = true;

                    BattleTurnManager.ForceCounterCommand(victim, attacker.GetFighter(), BrawlerPlayer.CurrentMoveset.RepelCounter);
                }
            }

            //Removed perfect guarding
            return false;
        }

        private static HumanModeManagerIsInputKamae _kamaeDeleg;
        private static HumanModeManagerIsInputKamae _kamaeTrampoline;
        public static bool HumanModeManager_IsInputKamae(IntPtr HumanModeManager)
        {
            if (!BrawlerBattleManager.Kasuga.IsValid())
                return false;

            if (!BrawlerPlayer.GenericShouldExecuteAttack())
                return false;

            if (HumanModeManager == BrawlerBattleManager.KasugaChara.HumanModeManager.Pointer)
            {
                MotionID gmt = BrawlerBattleManager.KasugaChara.GetMotion().GmtID;

                if (BrawlerPlayer.Info.IsDown || 
                    BrawlerPlayer.FreezeInput || 
                    gmt > 0)
                    
                    return false;

                if (DragonEngine.IsKeyHeld(VirtualKey.MiddleButton))
                    return true;
            }
            else
                return _kamaeTrampoline(HumanModeManager);

            return false;
        }

        private static HumanModeManagerIsInputKamae _guardDeleg;
        private static HumanModeManagerIsInputKamae _guardTrampoline;
        public static bool HumanModeManager_IsInputGuard(IntPtr HumanModeManager)
        {
            if (!BrawlerPlayer.GenericShouldExecuteAttack())
                return false;

            if (HumanModeManager == BrawlerBattleManager.KasugaChara.HumanModeManager.Pointer)
                return Mod.Input[BattleInput.LeftShift].TimeHeld > 0.1f;
            else
                return _guardTrampoline(HumanModeManager);
        }

        private static HumanModeManagerIsInputKamae _pickupDeleg;
        private static HumanModeManagerIsInputKamae _pickupTrampoline;
        public static bool HumanModeManager_IsInputPickup(IntPtr HumanModeManager)
        {
            if (!Mod.ShouldExecBrawlerInput())
                return false;

            if (BrawlerPlayer.Info.IsDown ||
                BrawlerPlayer.FreezeInput)

                return false;

            if (HumanModeManager == BrawlerBattleManager.KasugaChara.HumanModeManager.Pointer)
                return Mod.Input[BattleInput.F].LastTimeSincePressed < 0.1f;

            //   return DragonEngine.IsKeyDown(VirtualKey.E);
            else
                return _pickupTrampoline(HumanModeManager);
        }

        private static EffectEventPlay _effectEventPlayDeleg;
        private static EffectEventPlay _effectEventPlayTrampoline;
        private static void EffectEvent_Play(IntPtr effect, EffectEventCharaID id)
        {
            //TODO: clear out these beps and get rid of this hook
            if (id == EffectEventCharaID.yz_enemy_active_st || id == EffectEventCharaID.YZ_enemy_vanish || id == EffectEventCharaID.YZ_enemy_vanish_lp)
                return;

            _effectEventPlayTrampoline(effect, id);
        }


        private static BattleTurnManagerDmgNotify _btlTurnManagerDmgNotifyDeleg;
        private static BattleTurnManagerDmgNotify _btlTurnManagerDmgNotifyTrampoline;
        private static void BattleTurnManager_OnAfterDamage(IntPtr mng, IntPtr inf)
        {
            FighterID id = Marshal.PtrToStructure<FighterID>(inf + 0x8);

            if (!new EntityHandle<Character>(id.Handle).Get().Attributes.is_player)
                return;

            _btlTurnManagerDmgNotifyTrampoline(mng, inf);
        }

        private static BattleTurnManagerRequestShowMiss _btlTurnManagerRequestShowMissDeleg;
        private static BattleTurnManagerRequestShowMiss _btlTurnManagerDmgRequestShowMissTrampoline;
        private static void BattleTurnManager_RequestShowMiss(IntPtr mng, IntPtr fighter)
        {
            return;
        }


        private static BattleTurnManagerRequestWarpFighter _btlTurnManagerRequestWarpFighterDeleg;
        private static BattleTurnManagerRequestWarpFighter _btlTurnManagerRequestWarpFighterTrampoline;
        private static void BattleTurnManager_RequestWarpFighter(IntPtr mng, IntPtr fighter, IntPtr inf, IntPtr res)
        {
            return;
        }

        private static FighterTP _fighterTPDeleg;
        private static FighterTP _fighterTPTrampoline;
        private static bool Fighter_TP(IntPtr fighter, IntPtr tpInf)
        {
            return true;
        }

        private static ECRenderCharacterBattleTransformOn _ecRenderCharacterBattleTransformOnDeleg;
        private static ECRenderCharacterBattleTransformOn _ecRenderCharacterBattleTransformOnTrampoline;
        private static bool ECRenderCharacter_BattleTransformOn(IntPtr render)
        {
            ECRenderCharacter rend = new ECRenderCharacter() { Pointer = render };

            if (rend.Owner.UID == BrawlerBattleManager.KasugaChara.UID)
            {
                if (!BrawlerBattleManager.AllowPlayerTransformationDoOnce)
                    return true;
                else
                    BrawlerBattleManager.AllowPlayerTransformationDoOnce = false;
            }

            return _ecRenderCharacterBattleTransformOnTrampoline(render);
        }

        private static BattleTurnManagerChangePhase _btlTurnManagerChangePhaseDeleg;
        private static BattleTurnManagerChangePhase _btlTurnManagerChangePhaseTrampoline;
        private static void BattleTurnManager_ChangePhase(IntPtr mng, BattleTurnManager.TurnPhase phase)
        {

            //TODO:
            //Shitty fix. The game will directly need to be altered
            //To not do stupid shit while doing brawler hacts

            /*
                Broken Flow:
                Event -> Action

                Correct Flow:
                Action (or Event) -> Cleanup
            */
            if (BrawlerBattleManager.Enemies.Length == 0 && BattleTurnManager.CurrentPhase == BattleTurnManager.TurnPhase.Event && phase == BattleTurnManager.TurnPhase.Action)
            {
                BattleTurnManager.ChangePhase(BattleTurnManager.TurnPhase.Cleanup);
                BattleTurnManager.ChangePhase(BattleTurnManager.TurnPhase.BattleResult);

                return;
            }

            if (phase == BattleTurnManager.TurnPhase.Start)
            {
                BattleTurnManager.ChangePhase(BattleTurnManager.TurnPhase.Action);
                BattleTurnManager.ChangeActionStep(BattleTurnManager.ActionStep.Init);
            }

            Console.WriteLine(BattleTurnManager.CurrentPhase + " -> " + phase);


            _btlTurnManagerChangePhaseTrampoline(mng, phase);

            if (phase == BattleTurnManager.TurnPhase.BattleResult)
            {
                new SimpleTimer(0.1f, delegate { BattleTurnManager.ChangePhase(BattleTurnManager.TurnPhase.End); });
            }
        }

        private static TimingDropAssetPlay _dropPlayDeleg;
        private static TimingDropAssetPlay _dropPlayTrampoline;
        private unsafe static void TimingDropAsset_Play(IntPtr node, uint tick, IntPtr matrix, uint* parentHandle)
        {
            Character chara = new EntityHandle<Character>(*parentHandle).Get();
            chara.GetFighter().DropWeapon(new DropWeaponOption(AttachmentCombinationID.right_weapon, false));
        }
    }
}
