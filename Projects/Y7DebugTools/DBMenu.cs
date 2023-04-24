using DragonEngineLibrary;
using ImGuiNET;

namespace Y7DebugTools
{
    internal static class DBMenu
    {
        public static bool Open;

        private static int m_uiSceneID;
        private static int m_dbID;
        public static void Draw()
        {
            if(ImGui.Begin("DB"))
            {
                if (ImGui.CollapsingHeader("UI Scene"))
                {
                    ImGui.InputInt("UI Scene ID", ref m_uiSceneID);

                    if (ImGui.Button("Reload"))
                    {
                        UI.Unload((uint)m_uiSceneID);
                        UI.Load((uint)m_uiSceneID);
                    }

                    if(ImGui.Button("Load"))
                        UI.Load((uint)m_uiSceneID);

                    if(ImGui.Button("Unload"))
                        UI.Unload((uint)m_uiSceneID);
                }

                if(ImGui.CollapsingHeader("DB"))
                {
                    ImGui.InputInt("DB ID", ref m_dbID);

                    if (ImGui.Button("Reload"))
                    {
                        DB.Unload((DbId)m_dbID);
                        DB.Refresh();
                    }
                }
            }
        }
    }
}
