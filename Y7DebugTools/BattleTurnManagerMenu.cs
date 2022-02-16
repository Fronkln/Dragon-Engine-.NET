using System;
using DragonEngineLibrary;
using ImGuiNET;

namespace Y7DebugTools
{
    public static class BattleTurnManagerMenu
    {
        public static void Draw()
        {
            if (ImGui.Begin("BattleTurnManager Menu"))
            {
                ImGui.Text("Current Phase: " + BattleTurnManager.CurrentPhase);
                ImGui.Text("Current Action Step: " + BattleTurnManager.CurrentActionStep);
                ImGui.Text("Current Action Type: " + BattleTurnManager.CurrentActionType);

                ImGui.End();
            }
        }
    }
}
