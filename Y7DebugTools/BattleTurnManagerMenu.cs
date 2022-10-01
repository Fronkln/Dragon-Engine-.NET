using System;
using DragonEngineLibrary;
using ImGuiNET;

namespace Y7DebugTools
{
    public static class BattleTurnManagerMenu
    {
        private static string[] m_phaseList;
        private static string[] m_ActionList;
        private static string[] m_ActionTypeList;

        private static int m_phase;
        private static int m_action;
        private static int m_actionType;

        static BattleTurnManagerMenu()
        {
            m_phaseList = Enum.GetNames(typeof(BattleTurnManager.TurnPhase));
            m_ActionList = Enum.GetNames(typeof(BattleTurnManager.ActionStep));
            m_ActionTypeList = Enum.GetNames(typeof(BattleTurnManager.ActionType));
        }


        public static void Draw()
        {
            if (ImGui.Begin("BattleTurnManager Menu"))
            {

                m_phase = (int)BattleTurnManager.CurrentPhase;
                m_action = (int)BattleTurnManager.CurrentActionStep;    


                if (ImGui.Combo("Current Phase:", ref m_phase, m_phaseList, m_phaseList.Length))
                    BattleTurnManager.ChangePhase((BattleTurnManager.TurnPhase)m_phase);

                if (ImGui.Combo("Current Action Step", ref m_action, m_ActionList, m_ActionList.Length))
                    BattleTurnManager.ChangeActionStep((BattleTurnManager.ActionStep)m_action);

                //   ImGui.Text("Current Phase: " + BattleTurnManager.CurrentPhase);
                //ImGui.Text("Current Action Step: " + BattleTurnManager.CurrentActionStep);
                ImGui.Text("Current Action Type: " + BattleTurnManager.CurrentActionType);

                ImGui.End();
            }
        }
    }
}
