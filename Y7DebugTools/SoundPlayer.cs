using System;
using DragonEngineLibrary;
using ImGuiNET;

namespace Y7DebugTools
{
    public static class SoundPlayer
    {
        public static bool Open = false;

        private static int m_ChosenSheet;
        private static  int m_ChosenCueID;

        private static string[] m_enumValues_sheet;

        static SoundPlayer()
        {
            m_enumValues_sheet = Enum.GetNames(typeof(SoundCuesheetID));
        }

        public static void Draw()
        {
            if(Open)
            {
                if(ImGui.Begin("Cue Player"))
                {
                    ImGui.Combo("Cuesheet", ref m_ChosenSheet, m_enumValues_sheet, m_enumValues_sheet.Length);
                    ImGui.InputInt("ID", ref m_ChosenCueID);

                    if (ImGui.Button("Play"))
                    {
                        SoundManager.PlayCue((ushort)m_ChosenSheet, (ushort)m_ChosenCueID, 0);
                    }
                }
            }
        }
    }
}
