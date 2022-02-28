using System;
using DragonEngineLibrary;
using ImGuiNET;

namespace Y7DebugTools
{
    public static class AnimPlayer
    {
        private static int m_chosenAnim;

        public static void Draw()
        {
            if(ImGui.Begin("Anim Player"))
            {
                ImGui.InputInt("Anim", ref m_chosenAnim);

                if (ImGui.Button("Play"))
                    DragonEngine.GetHumanPlayer().GetMotion().RequestGMT((MotionID)m_chosenAnim);

                ImGui.End();
            }
        }
    }
}
