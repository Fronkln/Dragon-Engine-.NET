using System;
using System.IO;
using System.Threading;
using DragonEngineLibrary;
using Steamworks;
using Steamworks.Data;
using DearImguiSharp;

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

                    if (DragonEngine.IsKeyDown(VirtualKey.Numpad2))
                    {
                        DragonEngineLibrary.Advanced.ImGui.Init();
                    }
                }
                else
                {
                    if (DragonEngine.IsKeyDown(VirtualKey.Numpad2))
                        MPManager.CurrentLobby.Leave();

                    if(DragonEngine.IsKeyDown(VirtualKey.Numpad3))
                    {
                        NetPacket packet = new NetPacket(false);

                        packet.Writer.Write((byte)PacketMessage.CharacterPlayGMT);
                        packet.Writer.Write(454u);

                        MPManager.SendToEveryone(packet, P2PSend.Unreliable);
                    }

                    if (DragonEngine.IsKeyDown(VirtualKey.Numpad4))
                    {
                        foreach (Friend friend in MPManager.CurrentLobby.Members)
                            DragonEngine.Log("Lobby Member: " + friend.Name);
                    }
                }
            }
        }


        public static void PresentTest()
        {           
            ImVec2 vec = new ImVec2();
            vec.X = 339;
            vec.Y = 221;

            bool no = true;

            ImGui.SetNextWindowSize(vec, 4);
            ImGui.Begin("Yakuza 7: Multiplayer", ref no, 1024);
            ImGui.End();
        }

        public override void OnModInit()
        {

            DragonEngine.Log(ImGui.GetVersion() + " imgui version");

            try
            {
                DragonEngine.Initialize();

                //Initialize Steam API
                SteamClient.Init(1235140);

                //Initialize Steam API callbacks
                SteamMatchmaking.OnLobbyCreated += MPManager.OnLobbyCreated;
                SteamMatchmaking.OnLobbyEntered += MPManager.OnLobbyEnter;
                SteamMatchmaking.OnLobbyInvite += MPManager.OnInvitedToLobby;
                SteamNetworking.OnP2PSessionRequest += MPManager.OnP2PRequest;
                SteamMatchmaking.OnLobbyMemberJoined += MPManager.LobbyBugTest;
                SteamMatchmaking.OnLobbyMemberDisconnected += MPManager.LobbyBugTest;
                SteamMatchmaking.OnLobbyMemberLeave += MPManager.LobbyBugTest;

                //Start the input thread (reads keyboard input)
                Thread inputThread = new Thread(InputThread);
                inputThread.Start();

                //Register Dragon Engine callback
                DragonEngine.RegisterJob(MPManager.Update, DEJob.Update);


                DragonEngineLibrary.Advanced.ImGui.Init();
                DragonEngineLibrary.Advanced.ImGui.RegisterUIUpdate(PresentTest);
            }
            catch (Exception ex)
            {
                DragonEngine.Log("Failed to initialize Yakuza 7 Online" + ex.Message);
            }

            DragonEngine.Log("Yakuza 7 Online");
        }
    }
}
