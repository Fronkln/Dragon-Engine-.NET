using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using DragonEngineLibrary;

namespace Brawler
{
    public class Mod : DragonEngineMod
    {
        //0000000140085CF0 basemode change on debug build find the same place on retail and name it
        //used for getting up from attack after X interval
        [DllImport("Y7Internal.dll", EntryPoint = "VFUNC_TEST", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void TEST_FUNC();

        public const float CriticalHPRatio = 0.4f;

        public static bool DebugAutomaticCombo = false;
        public static bool DisableAttacksFromAI = false;

        public void InputThread()
        {
            while (true)
            {
                if (DragonEngine.IsKeyDown(VirtualKey.X))
                {
                    //doesnt work check function + offset of var
                    FighterManager.GetFighter(0).GetStatus().SetSuperArmor(true);
                }

                if (DragonEngine.IsKeyDown(VirtualKey.Numpad7))
                {
                    bool toggle = !FighterManager.IsBrawlerMode();
                    FighterManager.ForceBrawlerMode(toggle);

                    DragonEngine.Log("Brawler Mode: " + toggle);
                }

                if (DragonEngine.IsKeyDown(VirtualKey.Numpad8))
                {
                    ECBattleStatus status = DragonEngine.GetHumanPlayer().GetBattleStatus();
                    status.AttackPower = status.AttackPower = 0;

                    DragonEngine.Log("No damage");
                }

                if (DragonEngine.IsKeyDown(VirtualKey.Numpad9))
                {
                    DragonEngine.GetHumanPlayer().HumanModeManager.ToSway();
      
                }

                if (DragonEngine.IsKeyHeld(VirtualKey.LeftShift))
                {
                    if (DragonEngine.IsKeyDown(VirtualKey.T))
                    {
                        DisableAttacksFromAI = !DisableAttacksFromAI;
                        DragonEngine.Log("AI Attack: " + !DisableAttacksFromAI); ;
                    }

                    if (DragonEngine.IsKeyDown(VirtualKey.P))
                    {
                        DebugAutomaticCombo = !DebugAutomaticCombo;
                        DragonEngine.Log("Automatic combo: " + DebugAutomaticCombo);
                    }
                }

                if (FighterManager.IsBrawlerMode() && ShouldExecBrawlerInput())
                    BrawlerPlayer.InputUpdate();
            }
        }

        public static bool ShouldExecBrawlerInput()
        {
            Fighter kasugaFighter = FighterManager.GetPlayer();

            if (!kasugaFighter.IsValid() || kasugaFighter.IsDead())
                return false;

            if (BattleManager.BattleTime < BattleManager.BattleStartTime)
                return false;


            Fighter[] allEnemies = FighterManager.GetAllEnemies();

            if (allEnemies.Length <= 0)
                return false;

            if (allEnemies.Where(x => x.IsDead()).ToArray().Length == allEnemies.Length)
                return false;

            return true;
        }


        public override void OnModInit()
        {
            DragonEngine.Initialize();

            BrawlerPlayer.Init();
            HeatActionManager.Init();
            WeaponManager.InitWeaponMovesets();

            DragonEngine.RegisterJob(BattleManager.Update, DEJob.Update);

            BattleTurnManager.OverrideAttackerSelection(OnAttackerSelect);
            FighterManager.ForceBrawlerMode(true);

            BrawlerHooks.Init();

            Thread thread = new Thread(InputThread);
            thread.Start();
        }

        public static Fighter OnAttackerSelect(bool readOnly, bool getNextFighter)
        {
            if (DisableAttacksFromAI)
                return FighterManager.GetFighter(0);

            if (!FighterManager.IsBrawlerMode())
                return null;

            Fighter[] allEnemies = FighterManager.GetAllEnemies();

            if (allEnemies.Length <= 0)
                return null;

            Random rnd = new Random();

            return allEnemies[rnd.Next(0, allEnemies.Length)];
        }

    }
}
