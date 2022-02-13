using System;
using DragonEngineLibrary;
using ImGuiNET;


namespace Y7DebugTools
{
    public static class PlayerMenu
    {
        public static void Draw()
        {
            Character player = DragonEngine.GetHumanPlayer();
            
            if(ImGui.Begin("Player"))
            {
                CharaRender.Draw(player);
                ImGui.End();
            }
        }
    }
}
