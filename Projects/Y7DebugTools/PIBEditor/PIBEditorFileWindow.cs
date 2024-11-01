using System;
using ImGuiNET;
using DragonEngineLibrary;
using System.Text;

namespace Y7DebugTools
{
    internal static class PIBEditorFileWindow
    {
        private static int m_ptcToLoad = 8671;

        public static bool Open = true;

        private static byte[] m_textBuf = new byte[255];

        static PIBEditorFileWindow()
        {
            byte[] buf = Encoding.ASCII.GetBytes(PIBEditorMainWindow.TestSavePath);
            Array.Copy(buf, 0, m_textBuf, 0, buf.Length);
        }

        public static void Draw()
        {
            if (Open)
            {
                if (ImGui.Begin("File", ref Open))
                {
                    ImGui.InputText("PIB Path", m_textBuf, 255);

                    if (ImGui.Button("Load"))
                    {
                        DragonEngine.Log("Load PIB " + m_ptcToLoad);
                        PIBEditorMainWindow.OpenPIB(Encoding.ASCII.GetString(m_textBuf).Split(new[] { '\0' }, 2)[0]);
                        Open = false;
                    }

                    ImGui.End();
                }
            }
        }
    }
}
