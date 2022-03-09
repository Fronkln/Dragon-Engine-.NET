using System;
using DragonEngineLibrary;
using ImGuiNET;

namespace Y7DebugTools
{
    public static class AIUtilCommandRender
    {
        public static void Draw(AIUtilCommand command)
        {
            ImGui.Text("Pack ID: " + command.pack_id_);
            ImGui.Text("Command ID: " + command.command_id_);
            ImGui.Text("State: " + command.state_);
            ImGui.Text("Commander: " + command.commander_);
            ImGui.Text("Priority: " + command.priority_);
            ImGui.Text("Target Position " + command.element_.target_pos_);
        }
    }
}
