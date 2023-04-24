using System;
using DragonEngineLibrary;
using ImGuiNET;

namespace Y7DebugTools
{
    internal static class GameVarMenu
    {
        public static bool Open = false;

        private static string[] m_names_gameVar;
        private static int m_curVar;

        static GameVarMenu()
        {
            m_names_gameVar = Enum.GetNames(typeof(GameVarID));
        }

        public static void Draw()
        {
            if(ImGui.Begin("Game Var Manager"))
            {
                ImGui.Combo("Variable", ref m_curVar, m_names_gameVar, m_names_gameVar.Length);

                ImGui.Text("Bool: " + GameVarManager.GetValueBool((GameVarID)m_curVar));
                ImGui.Text("UInt: " + GameVarManager.GetValueUInt((GameVarID)m_curVar));

                ImGui.End();
            }
        }
    }
}
