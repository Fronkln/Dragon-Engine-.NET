using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using DragonEngineLibrary;
using ImGuiNET;

namespace Y7DebugTools
{
    public static class FighterManagerMenu
    {
        private static int m_chosenCharaID;
        private static int m_chosenPersonalGroupID;
        private static int m_effectID;
        private static int m_effParamSetID;
        private static int m_category = 11;

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
            if (ImGui.Begin("FighterManager Menu"))
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
                }

                if (ImGui.CollapsingHeader("Fighters"))
                {

                    Fighter[] fighters = FighterManager.GetAllFighters();

                    for (int i = 0; i < fighters.Length; i++)
                    {
                        var fighter = fighters[i];

                        if (ImGui.TreeNode($"Fighter {i}"))                                     
                        {

                            if (ImGui.Button("Kill"))
                            {
                                fighter.Character.ToDead();
                            }
#if TURN_BASED
                            if (ImGui.TreeNode($"Status Effect"))
                            {
                                ImGui.InputInt("Effect ID", ref m_effectID);
                                ImGui.InputInt("Effect Param Set ID", ref m_effParamSetID);
                                ImGui.InputInt("Effect Category ID", ref m_category);

                                if (ImGui.Button("Apply Effect"))
                                {
                                    ExEffectInfo str = new ExEffectInfo();
                                    str.effID = m_effectID;
                                    str.idk = str.effID;
                                    str.effSetID = m_effParamSetID;
                                    str.idk2 = str.effSetID;
                                    str.category = m_category;
                                    str.bKeepInfinity = true;
                                    str.nKeepDamage = 255;

                                    var ptr = str.ToIntPtr();
                                    fighter.GetStatus().AddExEffect(ptr, false, false);
                                    Marshal.FreeHGlobal(ptr);
                                }

                                if (ImGui.Button("Remove Effect"))
                                {
                                    fighter.GetStatus().RemoveExEffect((uint)m_effectID, false, false);
                                }

                                ImGui.TreePop();
                            }
#endif
                            ImGui.TreePop();
                        }
                    }
                }

                ImGui.End();
            }
        }
    }
}
