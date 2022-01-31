using System;
using System.Threading;
using DragonEngineLibrary;
using Steamworks;
using Steamworks.Data;

namespace Y7MP
{
    public class Mod : DragonEngineMod
    {
        //Only used for keypress checks
        //Making this a thread and using while loop makes key functions consistent
        public void InputThread()
        {
            //A while(true) loop is not dangerous here because it's a seperate thread.
            //It does not block any other functions
            while (true)
            {
                if (!MPManager.Connected)
                {
                    if (DragonEngine.IsKeyDown(VirtualKey.Numpad1))
                        SteamMatchmaking.CreateLobbyAsync(MPManager.MaxPlayers);
                }
                else
                {
                    if (DragonEngine.IsKeyDown(VirtualKey.Numpad2))
                        MPManager.CurrentLobby.Leave();
                }
            }
        }

        public override void OnModInit()
        {
            DragonEngine.Log("Yakuza 7 Online");
            try
            {
                //Initialize Steam API
                SteamClient.Init(1235140);

                //Initialize Steam API callbacks
                SteamMatchmaking.OnLobbyCreated += MPManager.OnLobbyCreated;
                SteamMatchmaking.OnLobbyEntered += MPManager.OnLobbyEnter;

                //Start the input thread (reads keyboard input)
                Thread inputThread = new Thread(InputThread);
                inputThread.Start();
            }
            catch (Exception ex)
            {
                DragonEngine.Log("Failed to initialize Yakuza 7 Online" + ex.Message);
            }
        }
    }
}
