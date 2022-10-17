#define USE_DS4

using System;
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
        public static bool IsKeyboard = false;

        private static ViGEmClient VigCli;
#if USE_XBOX
        internal static IXbox360Controller ControllerSim;
#else
        internal static IDualShock4Controller ControllerSim;
#endif

        private static byte m_LX;
        private static byte m_LY;

        private static byte m_RX;
        private static byte m_RY;

        private static DateTime m_noTimeSinceEmulation;
        private static bool m_vigemFail = false;

        static BrawlerKeyboardMovement()
        {
            try
            {
                VigCli = new ViGEmClient();
#if USE_XBOX
            ControllerSim = VigCli.CreateXbox360Controller();
#else
                ControllerSim = VigCli.CreateDualShock4Controller();
#endif

                ControllerSim.Connect();
                Console.WriteLine("[LIKE A BRAWLER] Vigem start");
            }
            catch(Nefarius.ViGEm.Client.Exceptions.VigemBusNotFoundException ex)
            {
                Console.WriteLine("[LIKE A BRAWLER] !!!!!!!!!!!!!!!!!!VIGEMBUS NOT INSTALLED, KEYBOARD MOVEMENT WILL NOT WORK!!!!!!!!!!!!!!!!!!");
                m_vigemFail = true;
            }
        }

        public static void Clear()
        {

        }

        //14.10.2022: No longer a while loop, when i formatted my computer, this section flared up and lagged my PC prolly because it was executing too much
        //It's no longer seperate thread but managed by BattleManager and it works because we dont check for presses we check for holds
        //It executes less frequently now, we'll see if this causes problems later down the line
        public static void Update()
        {

                if (m_vigemFail)
                    return;

                    if (BattleTurnManager.CurrentPhase != BattleTurnManager.TurnPhase.Action || (BrawlerPlayer.FreezeInput || !Mod.IsGameFocused) || (!BrawlerBattleManager.BattleStartDoOnce || BrawlerBattleManager.HActIsPlaying))
                    {
#if USE_XBOX
                    ControllerSim.SetAxisValue(Xbox360Axis.LeftThumbX, 0);
                    ControllerSim.SetAxisValue(Xbox360Axis.LeftThumbY, 0);
#else

                        ControllerSim.SetAxisValue(DualShock4Axis.LeftThumbX, 128);
                        ControllerSim.SetAxisValue(DualShock4Axis.RightThumbX, 128);

                        m_LX = 128;
                        m_RX = 128;

                        ControllerSim.SetAxisValue(DualShock4Axis.LeftThumbY, 128);
                        ControllerSim.SetAxisValue(DualShock4Axis.RightThumbY, 128);

                        m_LY = 128;
                        m_RY = 128;
#endif
                        return;
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

                    bool set = false;

                    if (DragonEngine.IsKeyHeld(VirtualKey.W))
                    {
                        ControllerSim.SetAxisValue(DualShock4Axis.LeftThumbY, 0);
                        m_LY = 0;

                        set = true;
                    }
                    else if (DragonEngine.IsKeyHeld(VirtualKey.S))
                    {
                        ControllerSim.SetAxisValue(DualShock4Axis.LeftThumbY, 255);
                        m_LY = 255;
                        set = true;
                    }
                    else
                    {
                        m_LY = 128;
                        m_RY = 128;

                        ControllerSim.SetAxisValue(DualShock4Axis.LeftThumbY, 128);
                        ControllerSim.SetAxisValue(DualShock4Axis.RightThumbY, 128);
                    }

                    if (DragonEngine.IsKeyHeld(VirtualKey.A))
                    {
                        ControllerSim.SetAxisValue(DualShock4Axis.LeftThumbX, 0);
                        m_LY = 0;
                        set = true;
                    }
                    else if (DragonEngine.IsKeyHeld(VirtualKey.D))
                    {
                        ControllerSim.SetAxisValue(DualShock4Axis.LeftThumbX, 255);
                        m_LX = 255;
                        set = true;
                    }
                    else
                    {
                        m_LX = 128;
                        m_RX = 128;

                        ControllerSim.SetAxisValue(DualShock4Axis.LeftThumbX, 128);
                        ControllerSim.SetAxisValue(DualShock4Axis.RightThumbX, 128);
                    }

                    if (set)
                    {
                        IsKeyboard = true;
                        m_noTimeSinceEmulation = DateTime.Now;
                    }
                    else
                    {


                        if (m_LX == 128 && m_LY == 128 && m_RX == 128 && m_RY == 128)
                            if ((DateTime.Now - m_noTimeSinceEmulation).Seconds >= 2)
                                if (BrawlerBattleManager.Kasuga.IsValid())
                                    if (BrawlerPlayer.Info.IsMove)
                                        IsKeyboard = false;
                    }
#endif
                }     
        }
    }