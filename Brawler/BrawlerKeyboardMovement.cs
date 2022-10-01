#define USE_DS4

using System.Threading;
using Nefarius.ViGEm.Client;
using Nefarius.ViGEm.Client.Targets;
using Nefarius.ViGEm.Client.Targets.Xbox360;
using Nefarius.ViGEm.Client.Targets.DualShock4;
using DragonEngineLibrary;


namespace Brawler
{
    //Emulation is required to allow keyboard players to move around
    //Ultimately i wanted to fix this, but despite my best efforts. I couldn't.
    //I traced it to a RPG targeting function, and it made zero sense
    //Y:LAD took "REAL YAKUZA USE A GAMEPAD" too far
    internal static class BrawlerKeyboardMovement
    {
        private static ViGEmClient VigCli = new ViGEmClient();
#if USE_XBOX
        internal static IXbox360Controller ControllerSim;
#else
        internal static IDualShock4Controller ControllerSim;
#endif

        static BrawlerKeyboardMovement()
        {
            VigCli = new ViGEmClient();
#if USE_XBOX
            ControllerSim = VigCli.CreateXbox360Controller();
#else
            ControllerSim = VigCli.CreateDualShock4Controller();
#endif

            ControllerSim.Connect();
            System.Console.WriteLine(DualShock4Axis.LeftThumbX + " " + DualShock4Axis.LeftThumbX);
        }

        public static void Clear()
        {

        }

        public static void Update()
        {
            while (true)
            {
                if (BattleTurnManager.CurrentPhase != BattleTurnManager.TurnPhase.Action || (BrawlerPlayer.FreezeInput || !Mod.IsGameFocused) || (!BrawlerBattleManager.BattleStartDoOnce || BrawlerBattleManager.HActIsPlaying))
                {
#if USE_XBOX
                    ControllerSim.SetAxisValue(Xbox360Axis.LeftThumbX, 0);
                    ControllerSim.SetAxisValue(Xbox360Axis.LeftThumbY, 0);
#else
                    ControllerSim.SetAxisValue(DualShock4Axis.LeftThumbX, 128);
                    ControllerSim.SetAxisValue(DualShock4Axis.RightThumbX, 128);

                    ControllerSim.SetAxisValue(DualShock4Axis.LeftThumbY, 128);
                    ControllerSim.SetAxisValue(DualShock4Axis.RightThumbY, 128);
#endif
                    continue;
                }

#if USE_XBOX
                if(DragonEngine.IsKeyHeld(VirtualKey.W))
                    ControllerSim.SetAxisValue(Xbox360Axis.LeftThumbY, 32767);
                else if(DragonEngine.IsKeyHeld(VirtualKey.S))
                    ControllerSim.SetAxisValue(Xbox360Axis.LeftThumbY, -32768);
                else
                    ControllerSim.SetAxisValue(Xbox360Axis.LeftThumbY, 0);

                if(DragonEngine.IsKeyHeld(VirtualKey.A))
                    ControllerSim.SetAxisValue(Xbox360Axis.LeftThumbX, 32767);
                else if (DragonEngine.IsKeyHeld(VirtualKey.D))
                    ControllerSim.SetAxisValue(Xbox360Axis.LeftThumbX, -32768);
                else
                    ControllerSim.SetAxisValue(Xbox360Axis.LeftThumbX, 0);
#else

                if (DragonEngine.IsKeyHeld(VirtualKey.W))
                {
                    ControllerSim.SetAxisValue(DualShock4Axis.LeftThumbY, 0);
                }
                else if (DragonEngine.IsKeyHeld(VirtualKey.S))
                {
                    ControllerSim.SetAxisValue(DualShock4Axis.LeftThumbY, 255);
                }
                else
                {
                    ControllerSim.SetAxisValue(DualShock4Axis.LeftThumbY, 128);
                    ControllerSim.SetAxisValue(DualShock4Axis.RightThumbY, 128);
                }

                if (DragonEngine.IsKeyHeld(VirtualKey.A))
                {
                    ControllerSim.SetAxisValue(DualShock4Axis.LeftThumbX, 0);
                }
                else if (DragonEngine.IsKeyHeld(VirtualKey.D))
                {
                    ControllerSim.SetAxisValue(DualShock4Axis.LeftThumbX, 255);
                }
                else
                {
                    ControllerSim.SetAxisValue(DualShock4Axis.LeftThumbX, 128);
                    ControllerSim.SetAxisValue(DualShock4Axis.RightThumbX, 128);
                }
#endif
            }
        }
    }
}
