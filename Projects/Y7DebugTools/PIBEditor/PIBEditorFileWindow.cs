using System;
using ImGuiNET;
using DragonEngineLibrary;

namespace Y7DebugTools
{
    internal static class PIBEditorFileWindow
    {
        private static int m_ptcToLoad = 8671;

        public static bool Open = true;
        public static void Draw()
        {
            if (Open)
            {
                if (ImGui.Begin("File", ref Open))
                {
                    ImGui.InputInt("PIB Path", ref m_ptcToLoad);

                    if (ImGui.Button("Load"))
                    {
                        DragonEngine.Log("Load PIB " + m_ptcToLoad);
                        PIBEditorMainWindow.OpenPIB(PIBEditorMainWindow.TestSavePath);
                        Open = false;
                    }

                    ImGui.End();
                }
            }
        }
    }
}
