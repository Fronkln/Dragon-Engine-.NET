using System;
using DragonEngineLibrary;
using ImGuiNET;

namespace Y7DebugTools
{
    internal static class ScreenEffectMenu
    {
        public static bool Open = false;
        private static int m_screenID = 0;

        public static void Draw()
        {
            ImGui.Begin("Screen Effect");
            ImGui.InputInt("ID", ref m_screenID);

            if (ImGui.Button("Play"))
            {
                EffectEventManager.LoadScreen((uint)m_screenID);
                EffectEventManager.PlayScreen((uint)m_screenID, false, false);
            }
  
            ImGui.End();
        }
    }
}
