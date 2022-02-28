using DragonEngineLibrary;
using ImGuiNET;

namespace Y7DebugTools
{
    public static class PlayermodelChanger
    {
        private static int m_chosenID = 0;

        public static void Draw()
        {
            ImGui.InputInt("Character ID", ref m_chosenID);

            if (ImGui.Button("Change"))
                DragonEngine.GetHumanPlayer().GetRender().Reload((CharacterID)m_chosenID);
        }
    }
}
