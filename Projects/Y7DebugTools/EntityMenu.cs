using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImGuiNET;
using DragonEngineLibrary;
using DragonEngineLibrary.Service;

namespace Y7DebugTools
{
    internal unsafe static class EntityMenu
    {
        public static bool Open = false;

        private static byte[] m_uidBuf = new byte[19];

        private static int m_serial;
        private static int m_user;
        private static int m_kind;

        private static EntityHandle<EntityBase> m_foundEnt = new EntityHandle<EntityBase>();

        public static void Draw()
        {
            if (ImGui.Begin("Entity"))
            {

                ImGui.InputText("UID", m_uidBuf, (uint)m_uidBuf.Length);

                ImGui.InputInt("Serial", ref m_serial);
                ImGui.InputInt("User", ref m_user);
                ImGui.InputInt("Kind", ref m_kind);

                ImGui.Dummy(new System.Numerics.Vector2(0, 20));

                ImGui.Text("Found Entity Handle: " + m_foundEnt.UID);

                if (ImGui.Button("Find"))
                {
                    m_foundEnt = EntityBase.GetGlobalEntityFromUID(new EntityUID() { Kind = (EUIDKind)m_kind, User = (ushort)m_user, Serial = (uint)m_serial }); ;
                    DragonEngine.Log(m_foundEnt + " Found Entity");
                }

                if (ImGui.Button("Find UID"))
                {
                    string uidStr = Encoding.UTF8.GetString(m_uidBuf).Split(new[] { '\0' }, 2)[0];
                    ulong uid = ulong.Parse(uidStr);
                    m_foundEnt = EntityBase.GetGlobalEntityFromUID(new EntityUID() { UID = uid });
                }

                if (m_foundEnt.IsValid())
                    if (ImGui.CollapsingHeader("Found Entity"))
                    {
                        EntityRender.Draw(m_foundEnt.Get());
                    }


                if (ImGui.TreeNode("Nearby Assets"))
                {
                    PXDStaticVector* collected = DragonEngine.GetHumanPlayer().Components.OctCollectAsset.Get().Collected;

                    for (int i = 0; i < collected->ElementSize; i++)
                    {
                        EntityHandle<EntityBase> foundEnt = collected->ElementAtU32(i);

                        if (ImGui.TreeNode(foundEnt.Get().EntityUID.UID.ToString("x")))
                        {
                            EntityRender.Draw(foundEnt.Get());
                            ImGui.TreePop();
                        }
                    }

                    ImGui.TreePop();
                }

                if (ImGui.TreeNode("Nearby Characters"))
                {
                    PXDStaticVector* collected = DragonEngine.GetHumanPlayer().Components.OctCollectCharacter.Get().Collected;

                    for (int i = 0; i < collected->ElementSize; i++)
                    {
                        EntityHandle<EntityBase> foundEnt = collected->ElementAtU32(i);

                        if(ImGui.TreeNode(foundEnt.Get().EntityUID.UID.ToString("x")))
                        {
                            EntityRender.Draw(foundEnt.Get());
                            ImGui.TreePop();
                        }
                    }

                    ImGui.TreePop();
                }


                ImGui.Text("Scene Entities");

                SceneBase scene = SceneService.CurrentScene;

                for (int i = 1; i < (int)SceneEntity.num; i++)
                {
                    EntityHandle<EntityBase> ent = scene.GetSceneEntity<EntityBase>((SceneEntity)i);

                    if (ent.IsValid())
                        ImGui.Text(i + " " + ent.Get().Pointer.ToString("x") + ((SceneEntity)i).ToString());
                }
            }
        }
    }
}
