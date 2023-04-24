using DragonEngineLibrary;
using Steamworks;
using ImGuiNET;

namespace Y7MP
{
    public static class MPUI
    {
        public static bool m_secretMenuToggle = false;
        private static bool m_secretMenuHact = true;

        private static ulong m_secretHActMenuChosenPlayer = 0;
        private static ulong m_secretHActMenuChosenEnemy = 0;
        private static int m_chosenHAct = 0;


        private static bool m_menu_active;
        private static bool m_chat_open;
        private static bool m_playermodel_selector_open;
        private static bool m_animation_player_open;

        public static void Draw()
        {
            if (m_secretMenuToggle)
            {
                if (ImGui.Begin("Super secret MP menu"))
                {
                    ImGui.Checkbox("HAct menu", ref m_secretMenuHact);

                    if (m_secretMenuHact)
                        DrawSecretHActMenu();

                    if (ImGui.CollapsingHeader("Misc"))
                    {
                        if (ImGui.Button("Make everyone hostile"))
                        {
                            NetPacket packet = new NetPacket(false);
                            packet.Writer.Write((byte)PacketMessage.TEST_EveryoneBecomesHostile);
                            MPManager.SendToEveryone(packet);
                        }

                        if (ImGui.Button("Make everyone friendly"))
                        {
                            NetPacket packet = new NetPacket(false);
                            packet.Writer.Write((byte)PacketMessage.TEST_EveryoneBecomesFriendly);
                            MPManager.SendToEveryone(packet);
                        }

                        if (ImGui.Button("Toggle ghost visibility"))
                            MPPlayer.DebugLocalGhostVisible = !MPPlayer.DebugLocalGhostVisible;
                    }

                    ImGui.End();
                }
            }


            if (MPManager.Connected)
            {
                MPChat.Draw();

                ImGui.SetNextWindowSize(new System.Numerics.Vector2(339, 221), ImGuiCond.FirstUseEver);
                ImGui.Begin("Yakuza 7: Multiplayer", ImGuiWindowFlags.MenuBar);

                if (ImGui.CollapsingHeader("Lobby"))
                {
                    ShowLobbyDropdown();
                    ImGui.EndMenu();
                }

                ImGui.End();
            }
        }

        private static void ShowLobbyDropdown()
        {
            ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 30);

            if (ImGui.CollapsingHeader("Player List"))
            {
                int playerCount = SteamMatchmaking.GetNumLobbyMembers(MPManager.CurrentLobby);

                for (int i = 0; i < playerCount; i++)
                {
                    CSteamID lobbyPlayer = SteamMatchmaking.GetLobbyMemberByIndex(MPManager.CurrentLobby, i);

                    ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 35);
                    ImGui.Text(lobbyPlayer.Name());
                }
            }

            ImGui.Dummy(new System.Numerics.Vector2(0, 6));

            if (ImGui.Button("Disconnect"))
            {
                MPManager.Leave();
                return;
            }

            if (ImGui.Button("Force clear players (advanced)"))
                MPManager.ClearPlayers();
        }


        private static void DrawSecretHActMenu()
        {

            if (ImGui.CollapsingHeader("HAct"))
            {
                ImGui.Text("Player: " + new CSteamID(m_secretHActMenuChosenPlayer).Name());
                ImGui.Text("Enemy: " + new CSteamID(m_secretHActMenuChosenEnemy).Name());
                ImGui.InputInt("HAct: ", ref m_chosenHAct);

                if (ImGui.Button("Play"))
                {

                    NetPacket packet = new NetPacket(false);

                    packet.Writer.Write((byte)PacketMessage.PlayerOnPlayerHAct);
                    packet.Writer.Write(m_secretHActMenuChosenPlayer);
                    packet.Writer.Write(m_secretHActMenuChosenEnemy);
                    packet.Writer.Write((uint)m_chosenHAct);
                    packet.Writer.Write(false);

                    MPManager.SendToEveryone(packet, EP2PSend.k_EP2PSendReliable);
                }


                ImGui.Dummy(new System.Numerics.Vector2(0, 10));

                ImGui.Text("Player Select");

                foreach (var kv in MPManager.playerList)
                {
                    if (ImGui.Button(kv.Key.Name() + "(Player)"))
                        m_secretHActMenuChosenPlayer = kv.Key.m_SteamID;
                }

                ImGui.Dummy(new System.Numerics.Vector2(0, 10));

                ImGui.Text("Enemy Select");

                foreach (var kv in MPManager.playerList)
                {
                    if (ImGui.Button(kv.Key.Name() + "(Enemy)"))
                        m_secretHActMenuChosenEnemy = kv.Key.m_SteamID;
                }
            }
        }
    }
}
