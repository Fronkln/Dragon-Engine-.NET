using DragonEngineLibrary;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Y7DebugTools
{
    internal static class PIBEditorPibWindow
    {
        private static int m_pibID;

        public static void OnLoad()
        {
            m_pibID = (int)PIBEditorMainWindow.Pib.ParticleID;
        }

        public static void Draw()
        {
            if (PIBEditorMainWindow.Pib == null)
                return;

            if (ImGui.Begin("PIB"))
            {
                ImGui.Text("Version: " + PIBEditorMainWindow.Pib.Version);

                if (ImGui.InputInt("Pib ID", ref m_pibID))
                    PIBEditorMainWindow.Pib.ParticleID = (uint)m_pibID;

                for (int i = 0; i < PIBEditorMainWindow.PibRoot.Children.Count; i++)
                {
                    PIBEditorNodePibEmitter emitter = PIBEditorMainWindow.PibRoot.Children[i] as PIBEditorNodePibEmitter;

                    if (ImGui.TreeNode(emitter.Text + $"##{i}"))
                    {
                        if (ImGui.Button("General"))
                        {
                            PIBEditorEmitterWindow.EditingEmitter = emitter;
                            PIBEditorEmitterWindow.Open = true;

                            if (PIBEditorFlagWindow.ActiveWindow == PIBEditorFlagWindow.ActiveFlagsWindow.Flag1)
                                PIBEditorEmitterWindow.ShowActiveEmitterFlag1();
                            else if (PIBEditorFlagWindow.ActiveWindow == PIBEditorFlagWindow.ActiveFlagsWindow.Flag2)
                                PIBEditorEmitterWindow.ShowActiveEmitterFlag2();
                            else if (PIBEditorFlagWindow.ActiveWindow == PIBEditorFlagWindow.ActiveFlagsWindow.Flag3)
                                PIBEditorEmitterWindow.ShowActiveEmitterFlag3();
                        }
                        ImGui.TreePop();
                    }
                }


                if (!PIBEditorMainWindow.WaitingUnload && !PIBEditorMainWindow.WaitingLoad)
                {
                    if (ImGui.Button("Play"))
                    {
                        PIBEditorMainWindow.Play();
                    }


                    if (ImGui.Button("Save And Reload"))
                    {
                        PIBEditorMainWindow.Reload();
                    }
                }
                ImGui.End();
            }
        }
    }
}
