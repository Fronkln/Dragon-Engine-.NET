using DragonEngineLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImGuiNET;

namespace Y7DebugTools
{
    internal static class PadInfoRender
    {
        private static string[] m_buttonNames;

        static PadInfoRender()
        {
            m_buttonNames = Enum.GetNames(typeof(BattleButtonID));
        }

        public static void Draw(PadInputInfo input)
        {
            for(int i = 0; i < m_buttonNames.Length; i++)
            {
                ImGui.Text($" Pushing: {m_buttonNames[i]} {input.CheckCommand((BattleButtonID)i, 1, 2000)}");
            }
        }
    }
}
