using System;
using DragonEngineLibrary;
using ImGuiNET;

namespace Y7DebugTools
{
    public static class UIPlayer
    {
        private static UIHandleBase m_lastUI;

        private static int m_scene;
        private static int m_animation_set;

        public static bool Open;

        public static void Draw()
        {
            if (ImGui.Begin("UI Player"))
            {
                ImGui.InputInt("Scene ID", ref m_scene);
                ImGui.InputInt("Animation Set", ref m_animation_set);

                if (ImGui.Button("Create"))
                    m_lastUI = UI.Create((uint)m_scene, 1);
                if (ImGui.Button("Play"))
                    if (m_lastUI != null)
                        m_lastUI.PlayAnimationSet((uint)m_animation_set);
            }
        }
    }
}
