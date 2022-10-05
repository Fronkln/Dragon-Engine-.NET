using System;
using DragonEngineLibrary;
using ImGuiNET;

namespace Y7DebugTools
{
    public static class FighterCommandMenu
    {
        public static bool Open = false;

        private static int m_chosenSetID = 0;
        private static int m_chosenCmdID = 0;

        private static int m_chosenRPGSkillID = 0;

        private static bool m_incremental = false;

        private static int m_min = -1;
        private static int m_max = -1;


        public static void Draw()
        {
            if (ImGui.Begin("Fighter Command"))
            {
                if (ImGui.InputInt("Set ID:", ref m_chosenSetID))
                    if (m_chosenSetID < ushort.MinValue || m_chosenSetID > ushort.MaxValue)
                        m_chosenSetID = 0;

                if (ImGui.InputInt("Command ID:", ref m_chosenCmdID))
                    if (m_chosenCmdID < ushort.MinValue || m_chosenSetID > ushort.MaxValue)
                        m_chosenCmdID = 0;

                ImGui.InputInt("RPG Skill:", ref m_chosenRPGSkillID);

                ImGui.Dummy(new System.Numerics.Vector2(0, 5));

                ImGui.Checkbox("Incremental", ref m_incremental);

                if (m_incremental)
                {
                    ImGui.InputInt("Start", ref m_min);
                    ImGui.InputInt("End", ref m_max);
                }

                if (ImGui.Button("Execute") || DragonEngine.IsKeyDown(VirtualKey.J))
                {
                    if (m_chosenRPGSkillID != 0)
                        BattleTurnManager.ForceCounterCommand(FighterManager.GetFighter(0), FighterManager.GetAllEnemies()[0], (RPGSkillID)m_chosenRPGSkillID);
                    else
                    {
                        if (m_incremental)
                        {
                            if (m_min > -1 && m_chosenCmdID < m_min)
                                m_chosenCmdID = m_min;

                            if (m_max > -1 && m_chosenCmdID > m_max)
                                m_chosenCmdID = m_min;
                        }

                        BattleCommandSetID id = (BattleCommandSetID)(m_chosenSetID);
                        DragonEngine.GetHumanPlayer().HumanModeManager.ToAttackMode(new FighterCommandID() { set_ = (ushort)m_chosenSetID, cmd = (ushort)m_chosenCmdID });
                    }

                    if (m_incremental)
                        m_chosenCmdID++;
                }

                if (ImGui.Button("Execute RPG Skill"))
                {
                    BattleTurnManager.ForceCounterCommand(FighterManager.GetFighter(0), FighterManager.GetAllEnemies()[0], (RPGSkillID)m_chosenRPGSkillID);
                }
            }
        }
    }
}
