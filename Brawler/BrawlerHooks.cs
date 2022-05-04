using System;
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
        private delegate void EffectEventPlay(IntPtr effect, EffectEventCharaID id);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private delegate bool FighterTP(IntPtr character, IntPtr tpinf);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private delegate bool ECRenderCharacterBattleTransformOn(IntPtr character);

        public static void Init()
        {
            _kamaeDeleg = new HumanModeManagerIsInputKamae(HumanModeManager_IsInputKamae);
            _effectEventPlayDeleg = new EffectEventPlay(EffectEvent_Play);
            _btlTurnManagerDmgNotifyDeleg = new BattleTurnManagerDmgNotify(BattleTurnManager_OnAfterDamage);
            _btlTurnManagerRequestShowMissDeleg = new BattleTurnManagerRequestShowMiss(BattleTurnManager_RequestShowMiss);
            _btlTurnManagerRequestWarpFighterDeleg = new BattleTurnManagerRequestWarpFighter(BattleTurnManager_RequestWarpFighter);
            _ecRenderCharacterBattleTransformOnDeleg = new ECRenderCharacterBattleTransformOn(ECRenderCharacter_BattleTransformOn);

            try
            {
                MinHookHelper.initialize();
            }
            catch { }

            MinHookHelper.createHook((IntPtr)0x1406D7BE0, _kamaeDeleg, out _kamaeTrampoline);
            MinHookHelper.createHook((IntPtr)0x1407ECF00, _effectEventPlayDeleg, out _effectEventPlayTrampoline);
            MinHookHelper.createHook((IntPtr)0x1404D2B10, _btlTurnManagerDmgNotifyDeleg, out _btlTurnManagerDmgNotifyTrampoline);
            MinHookHelper.createHook((IntPtr)0x140FF0ED0, _btlTurnManagerRequestShowMissDeleg, out _btlTurnManagerDmgRequestShowMissTrampoline);
            MinHookHelper.createHook((IntPtr)0x1404DDEF0, _btlTurnManagerRequestWarpFighterDeleg, out _btlTurnManagerRequestWarpFighterTrampoline);
            MinHookHelper.createHook((IntPtr)0x1407E2B20, _ecRenderCharacterBattleTransformOnDeleg, out _ecRenderCharacterBattleTransformOnTrampoline);
            //   MinHookHelper.createHook((IntPtr)0x1404C45C0, _fighterTPDeleg, out _fighterTPTrampoline);



            MinHookHelper.enableAllHook();
        }

        private static HumanModeManagerIsInputKamae _kamaeDeleg;
        private static HumanModeManagerIsInputKamae _kamaeTrampoline;
        private static bool HumanModeManager_IsInputKamae(IntPtr HumanModeManager)
        {
            if (HumanModeManager == DragonEngine.GetHumanPlayer().HumanModeManager.Pointer)
                return DragonEngine.IsKeyHeld(VirtualKey.MiddleButton);
            else
                return _kamaeTrampoline(HumanModeManager);
        }

        private static EffectEventPlay _effectEventPlayDeleg;
        private static EffectEventPlay _effectEventPlayTrampoline;
        private static void EffectEvent_Play(IntPtr effect, EffectEventCharaID id)
        {
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

            if (rend.Owner.UID == DragonEngine.GetHumanPlayer().UID)
            {
                if (!BattleManager.AllowPlayerTransformationDoOnce)
                    return true;
                else
                    BattleManager.AllowPlayerTransformationDoOnce = false;
            }

            return _ecRenderCharacterBattleTransformOnTrampoline(render);
        }
    }
}
