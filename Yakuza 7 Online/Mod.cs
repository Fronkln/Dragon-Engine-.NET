using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using DragonEngineLibrary;
using Steamworks;
using MinHook.NET;

namespace Y7MP
{
    public class Mod : DragonEngineMod
    {
        private List<object> m_callbacks = new List<object>();

        //Only used for keypress checks
        //Making this a thread and using while loop makes key functions consistent
        public void InputThread()
        {
            //A while(true) loop is not dangerous here because it's a seperate thread.
            //It does not block any other functions
            while (true)
            {
                SteamAPI.RunCallbacks();

                if (!MPManager.Connected)
                {
                    if (DragonEngine.IsKeyDown(VirtualKey.Numpad5))
                        SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypePublic, MPManager.MaxPlayers);
                }
                else
                {
                    if (DragonEngine.IsKeyDown(VirtualKey.Numpad2))
                    {
                        MPManager.Leave();
                        continue;
                    }

                    if (DragonEngine.IsKeyDown(VirtualKey.T))
                    {
                        MPChat.focusOneTime = true;
                    }

                    if (DragonEngine.IsKeyDown(VirtualKey.Numpad3))
                    {
                        MPPlayer player = MPManager.CreateFakePlayer();
                        player.PlayerInfo.isPVP = true;
                    }

                    if (DragonEngine.IsKeyHeld(VirtualKey.LeftShift))
                    {

                        if (DragonEngine.IsKeyDown(VirtualKey.J))
                            MPUI.m_secretMenuToggle = !MPUI.m_secretMenuToggle;
                    }

                }
            }
        }

        public override void OnModInit()
        {

            try
            {
                DragonEngine.Initialize();
                SteamAPI.Init();

                //Initialize Steam API callbacks
                m_callbacks.Add(Callback<LobbyCreated_t>.Create(MPManager.OnLobbyCreated));
                m_callbacks.Add(Callback<LobbyEnter_t>.Create(MPManager.OnLobbyEnter));
                m_callbacks.Add(Callback<LobbyInvite_t>.Create(MPManager.OnInvitedToLobby));
                m_callbacks.Add(Callback<P2PSessionRequest_t>.Create(MPManager.OnP2PRequest));
                m_callbacks.Add(Callback<LobbyChatUpdate_t>.Create(MPManager.OnLobbyChatUpdate));

                //Start the input thread (reads keyboard input)
                Thread inputThread = new Thread(InputThread);
                inputThread.Start();

                MPHooks.Init();

                DragonEngineLibrary.Advanced.ImGui.Init();
                DragonEngineLibrary.Advanced.ImGui.RegisterUIUpdate(MPUI.Draw);

                //Register Dragon Engine callback
                DragonEngine.RegisterJob(MPManager.Update, DEJob.DrawSetup);
            }
            catch (Exception ex)
            {
                DragonEngine.Log("Failed to initialize Yakuza 7 Online" + ex.Message);
            }

            DragonEngine.Log("Yakuza 7 Online");
        }
    }
}
