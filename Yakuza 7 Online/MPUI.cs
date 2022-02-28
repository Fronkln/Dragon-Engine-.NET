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

        public static void Draw()
        {
            if (m_secretMenuToggle)
                if (ImGui.Begin("Super secret MP menu"))
                {
                    ImGui.Checkbox("HAct menu", ref m_secretMenuHact);

                    if (m_secretMenuHact)
                        DrawSecretHActMenu();

                    ImGui.End();
                }

            MPChat.Draw();
        }

        private static void DrawSecretHActMenu()
        {
            ImGui.Text("Player: " + new CSteamID(m_secretHActMenuChosenPlayer).Name());
            ImGui.Text("Enemy: " + new CSteamID(m_secretHActMenuChosenEnemy).Name());
            ImGui.InputInt("HAct: ", ref m_chosenHAct);

            if(ImGui.Button("Play"))
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
