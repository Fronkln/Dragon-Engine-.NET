using System;
using DragonEngineLibrary;
using ImGuiNET;

namespace Y7DebugTools
{
    public static class FighterManagerMenu
    {
        private static int m_chosenCharaID;
        private static int m_chosenPersonalGroupID;

        public static void SpawnEnemy()
        {
            Character player = DragonEngine.GetHumanPlayer();

            FighterManager.GenerateEnemyFighter(new PoseInfo(player.GetPosCenter(), player.GetAngleY()), (uint)m_chosenPersonalGroupID, (CharacterID)m_chosenCharaID);
        }

        public static void Draw()
        {
            if(ImGui.Begin("FighterManager Menu"))
            {
                if (ImGui.CollapsingHeader("Create Enemy"))
                {
                    ImGui.InputInt("Character ID:", ref m_chosenCharaID);
                    ImGui.InputInt("Soldier Personal Data ID:", ref m_chosenPersonalGroupID);

                    if (ImGui.Button("Spawn"))
                        SpawnEnemy();

                    ImGui.EndMenu();
                }


                ImGui.End();
            }
        }
    }
}
