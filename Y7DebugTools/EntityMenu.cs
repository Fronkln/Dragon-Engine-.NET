using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImGuiNET;
using DragonEngineLibrary;

namespace Y7DebugTools
{
    internal static class EntityMenu
    {
        public static bool Open = false;

        private static byte[] m_uidBuf = new byte[32];

        private static int m_serial;
        private static int m_user;
        private static int m_kind;

        private static EntityHandle<EntityBase> m_foundEnt = new EntityHandle<EntityBase>();

        public static void Draw()
        {
            if(ImGui.Begin("Entity"))
            {

                ImGui.InputText("UID", m_uidBuf, (uint)m_uidBuf.Length);

                ImGui.InputInt("Serial", ref m_serial);
                ImGui.InputInt("User", ref m_user);
                ImGui.InputInt("Kind", ref m_kind);

                ImGui.Dummy(new System.Numerics.Vector2(0, 20));

                ImGui.Text("Found Entity Handle: " + m_foundEnt.UID);

                if (ImGui.Button("Find"))
                {
                    Console.WriteLine(EntityBase.GetGlobalEntityFromUID(DragonEngine.GetHumanPlayer().EntityUID).UID + " g");

                    m_foundEnt = EntityBase.GetGlobalEntityFromUID(new EntityUID() { Kind = (EUIDKind)m_kind, User = (ushort)m_user, Serial = (uint)m_serial }); ;
                }

                if (ImGui.Button("Find UID"))
                {
                    string uidStr = System.Text.Encoding.UTF8.GetString(m_uidBuf);
                    ulong uid = ulong.Parse(uidStr, System.Globalization.NumberStyles.HexNumber);
                    Console.WriteLine(uidStr);
                    Console.WriteLine(EntityBase.GetGlobalEntityFromUID(new EntityUID() { UID = uid }));
                }
            }
        }
    }
}
