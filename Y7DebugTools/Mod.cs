using System;
using System.Threading;
using System.Collections.Generic;
using DragonEngineLibrary;
using ImGuiNET;

namespace Y7DebugTools
{
    public class Mod : DragonEngineMod
    {
        private static bool Visible = true;

        public static Array m_enumValues_VirtualKey;
        public static string[] m_enumNames_VirtualKey;

        private bool m_npcMenuEnabled = false;
        private bool m_playerMenuEnabled = false;
        private bool m_animPlayerMenuEnabled = false;
        private bool m_fighterManagerMenuEnabled = false;
        private bool m_battleTurnManagerMenuEnabled = false;
        private bool m_effectMenuEnabled = false;
        private bool m_hactPlayerMenuEnabled = false;
        private bool m_jobMenuEnabled = false;


        public void ModUI()
        {
            if (Visible)
            {
                ImGui.Begin("Debug");
                ImGui.Checkbox("Player", ref m_playerMenuEnabled);
                ImGui.Checkbox("Animation", ref m_animPlayerMenuEnabled);
                ImGui.Checkbox("NPC", ref m_npcMenuEnabled);
                ImGui.Checkbox("Scene Info", ref SceneInfo.Open);
                ImGui.Checkbox("FighterManager", ref m_fighterManagerMenuEnabled);
                ImGui.Checkbox("BattleTurnManager", ref m_battleTurnManagerMenuEnabled);
                ImGui.Checkbox("HAct Player", ref m_hactPlayerMenuEnabled);
                ImGui.Checkbox("Effect", ref m_effectMenuEnabled);
                ImGui.Checkbox("Fighter CFC", ref FighterCommandMenu.Open);
                ImGui.Checkbox("Cue Player", ref SoundPlayer.Open);
                ImGui.Checkbox("UI Player", ref UIPlayer.Open);



                if (ImGui.Checkbox("DE Job Count", ref m_jobMenuEnabled))
                    JobCounter.Toggle(m_jobMenuEnabled);

                if (ImGui.Button("Unload all DB"))
                    DB.Unload(DbId.character_character_data);

                ImGui.EndMenu();
                ImGui.End();


                if (m_playerMenuEnabled)
                    PlayerMenu.Draw();
                if (m_animPlayerMenuEnabled)
                    AnimPlayer.Draw();
                if (m_npcMenuEnabled)
                    NPCMenu.Draw();
                if (m_fighterManagerMenuEnabled)
                    FighterManagerMenu.Draw();
                if (m_battleTurnManagerMenuEnabled)
                    BattleTurnManagerMenu.Draw();
                if (m_effectMenuEnabled)
                    EffectEventMenu.Draw();
                if (m_hactPlayerMenuEnabled)
                    HActPlayer.Draw();

                if (FighterCommandMenu.Open)
                    FighterCommandMenu.Draw();

                if (SoundPlayer.Open)
                    SoundPlayer.Draw();

                if (UIPlayer.Open)
                    UIPlayer.Draw();

                if (SceneInfo.Open)
                    SceneInfo.Draw();

                if (m_jobMenuEnabled)
                {
                    ImGui.Text("Average execution per second:");
                    for (int i = 0; i < (int)DEJob.Number; i++)
                    {
                        ImGui.Text(((DEJob)i).ToString() + ": " + JobCounter.m_countAverage[i]);
                    }
                }

            }
        }

        private static void InputThread()
        {
            while (true)
            {
                VirtualKey key = ((VirtualKey)m_enumValues_VirtualKey.GetValue(NPCMenu.SpawnBind));

                if (DragonEngine.IsKeyDown(key))
                    NPCMenu.Create();

                if (DragonEngine.IsKeyDown(VirtualKey.F2))
                    Visible = !Visible;

                if (DragonEngine.IsKeyDown(VirtualKey.F3))
                    DragonEngine.ForceSetCursorVisible(!DragonEngine.IsCursorForcedVisible());
                    
            }
        }

        public void Update()
        {
            //We have NPCs to spawn
            if (NPCMenu.CreationQueue.Count > 0)
            {
                NPCRequestMaterial[] list = NPCMenu.CreationQueue.ToArray();

                foreach (NPCRequestMaterial mat in list)
                    NPCMenu.CreatedNPCs.Add(NPCFactory.RequestCreate(mat));

                NPCMenu.CreationQueue.Clear();
            }

            if (FighterManagerMenu.CreationQueue.Count > 0)
            {
                foreach (FighterManagerMenu.FighterManagerSpawn spawn in FighterManagerMenu.CreationQueue)
                    FighterManager.GenerateEnemyFighter(new PoseInfo(DragonEngine.GetHumanPlayer().Transform.Position, 0), (uint)spawn.m_PersonalGroupID, (CharacterID)spawn.m_CharaID);

                FighterManagerMenu.CreationQueue.Clear();
            }
        }

        public override void OnModInit()
        {
            DragonEngine.Initialize();
            DragonEngineLibrary.Advanced.ImGui.Init();
            DragonEngineLibrary.Advanced.ImGui.RegisterUIUpdate(ModUI);

            m_enumNames_VirtualKey = Enum.GetNames(typeof(VirtualKey));
            m_enumValues_VirtualKey = Enum.GetValues(typeof(VirtualKey));

            DragonEngine.RegisterJob(Update, DEJob.Update);

            Thread inputThread = new Thread(InputThread);
            inputThread.Start();
        }
    }
}
