using System;
using System.Collections.Generic;
using DragonEngineLibrary;
using ImGuiNET;

namespace Y7DebugTools
{
    public static class FighterManagerMenu
    {
        private static int m_chosenCharaID;
        private static int m_chosenPersonalGroupID;

        public static List<FighterManagerSpawn> CreationQueue = new List<FighterManagerSpawn>();

        public struct FighterManagerSpawn
        {
            public int m_CharaID;
            public int m_PersonalGroupID;
        }

        public static void SpawnEnemy()
        {
            Character player = DragonEngine.GetHumanPlayer();

            FighterManager.GenerateEnemyFighter(new PoseInfo(player.GetPosCenter(), player.GetAngleY()), (uint)m_chosenPersonalGroupID, (CharacterID)m_chosenCharaID);
        }

        public static void Draw()
        {
            if(ImGui.Begin("FighterManager Menu"))
            {
                if (ImGui.Button("Kill All Enemies"))
                    foreach (Fighter fighter in FighterManager.GetAllEnemies())
                        fighter.Character.ToDead();

                if (ImGui.Button("Kill All Allies"))
                    for (int i = 1; i < 4; i++)
                        FighterManager.GetFighter((uint)i).Character.ToDead();

                if (ImGui.CollapsingHeader("Create Enemy"))
                {
                    ImGui.InputInt("Character ID:", ref m_chosenCharaID);
                    ImGui.InputInt("Soldier Personal Data ID:", ref m_chosenPersonalGroupID);

                    if (ImGui.Button("Spawn") || (DragonEngine.IsKeyHeld(VirtualKey.LeftShift) && DragonEngine.IsKeyDown(VirtualKey.X)))
                    {
                        CreationQueue.Add(new FighterManagerSpawn() { m_CharaID = m_chosenCharaID, m_PersonalGroupID = m_chosenPersonalGroupID });
                    }
                        //SpawnEnemy();

                    ImGui.EndMenu();
                }


                ImGui.End();
            }
        }
    }
}
