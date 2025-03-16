using System;
using System.Threading;
using System.Collections.Generic;
using DragonEngineLibrary;
using ImGuiNET;
using MinHook.NET;
using System.Diagnostics;

namespace Y7DebugTools
{
    public class Mod : DragonEngineMod
    {
        public static bool Visible = true;

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


        private bool m_initOnce = false;

        public void ModUI()
        {
            HActPlayer.DrawTimeline();

            if (Visible)
            {
                ImGui.Begin("Debug");
                ImGui.Checkbox("Player", ref m_playerMenuEnabled);
                ImGui.Checkbox("Animation", ref m_animPlayerMenuEnabled);
                ImGui.Checkbox("NPC", ref m_npcMenuEnabled);
                ImGui.Checkbox("Scene Info", ref SceneInfo.Open);
                ImGui.Checkbox("Entity", ref EntityMenu.Open);
                ImGui.Checkbox("World", ref WorldMenu.Open);
                ImGui.Checkbox("Asset", ref AssetMenu.Enabled);
                ImGui.Checkbox("Game Var Manager", ref GameVarMenu.Open);
                ImGui.Checkbox("Particle", ref ParticleMenu.Open);
                ImGui.Checkbox("Scenario", ref ScenarioMenu.Open);
                ImGui.Checkbox("FighterManager", ref m_fighterManagerMenuEnabled);
                ImGui.Checkbox("BattleTurnManager", ref m_battleTurnManagerMenuEnabled);
                ImGui.Checkbox("HAct Player", ref m_hactPlayerMenuEnabled);
                ImGui.Checkbox("Camera", ref CameraMenu.Open);
                ImGui.Checkbox("Effect", ref m_effectMenuEnabled);
                ImGui.Checkbox("Fighter CFC", ref FighterCommandMenu.Open);
                ImGui.Checkbox("Cue Player", ref SoundPlayer.Open);
                ImGui.Checkbox("UI Player", ref UIPlayer.Open);
                ImGui.Checkbox("DB", ref DBMenu.Open);
                ImGui.Checkbox("Screen Effect", ref ScreenEffectMenu.Open);

                if (ImGui.Checkbox("DE Job Count", ref m_jobMenuEnabled))
                    JobCounter.Toggle(m_jobMenuEnabled);


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

#if TURN_BASED
                if (m_battleTurnManagerMenuEnabled)
                    BattleTurnManagerMenu.Draw();
#endif
                if (m_effectMenuEnabled)
                    EffectEventMenu.Draw();
                if (m_hactPlayerMenuEnabled)
                    HActPlayer.Draw();

                if (ParticleMenu.Open)
                    ParticleMenu.Draw();

                if (FighterCommandMenu.Open)
                    FighterCommandMenu.Draw();

                if (SoundPlayer.Open)
                    SoundPlayer.Draw();

                if (UIPlayer.Open)
                    UIPlayer.Draw();

                if (SceneInfo.Open)
                    SceneInfo.Draw();

                if (ScenarioMenu.Open)
                    ScenarioMenu.Draw();

                if (DBMenu.Open)
                    DBMenu.Draw();

                if (EntityMenu.Open)
                    EntityMenu.Draw();

                if (GameVarMenu.Open)
                    GameVarMenu.Draw();
                
                if(ScreenEffectMenu.Open)
                    ScreenEffectMenu.Draw();

                if (CameraMenu.Open)
                    CameraMenu.Draw();

                if (WorldMenu.Open)
                    WorldMenu.Draw();

                if (AssetMenu.Enabled)
                    AssetMenu.Draw();

                if (m_jobMenuEnabled)
                {
                    ImGui.Text("Average execution per second:");
                    for (int i = 0; i < (int)DEJob.Number; i++)
                    {
                        ImGui.Text(((DEJob)i).ToString() + ": " + JobCounter.m_countAverage[i]);
                    }
                }

                if (PlayerMenu.m_toEquip)
                {
                    PlayerMenu.m_toEquip = false;

                    if(PlayerMenu.equipItem > 0)
                        FighterManager.GetFighter(0).Equip((ItemID)PlayerMenu.equipItem, AttachmentCombinationID.right_weapon);
                    else
                        FighterManager.GetFighter(0).Equip((AssetID)PlayerMenu.equipAsset, AttachmentCombinationID.right_weapon, ItemID.invalid, RPGSkillID.invalid);
                }

                if(AssetMenu.CreateNext)
                {
                    Character plr = DragonEngine.GetHumanPlayer();
                    EntityHandle<AssetUnit> asset = AssetManager.CreateAsset((AssetID)AssetMenu.CreateID, plr.GetPosCenter(), plr.Transform.Orient);
                    AssetMenu.CreateNext = false;
                }
            }
        }

        private static void InputThread()
        {
            while (true)
            {
                VirtualKey key = ((VirtualKey)m_enumValues_VirtualKey.GetValue(NPCMenu.SpawnBind));

                if (DragonEngine.IsKeyHeld(VirtualKey.LeftShift))
                    if (DragonEngine.IsKeyDown(VirtualKey.V))
                        NoclipMode.Toggle();
                        

                if (DragonEngine.IsKeyDown(key))
                    NPCMenu.Create();

                if (DragonEngine.IsKeyDown(VirtualKey.F2))
                    Visible = !Visible;

                if (DragonEngine.IsKeyDown(VirtualKey.F3))
                    DragonEngine.ForceSetCursorVisible(!DragonEngine.IsCursorForcedVisible());

                HActPlayer.DrawInputThread();
                PIBEditorMainWindow.InputThread();
                    
            }
        }

        public void Update()
        {
            if(!m_initOnce)
                if(DragonEngine.GetHumanPlayer().IsValid())
                {
                    m_initOnce = true;
                }


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

            if (UIPlayer.ToCreate || UIPlayer.ToPlay)
            {
                if (UIPlayer.ToCreate)
                    UIPlayer.Create();
                else
                    UIPlayer.Play();

                UIPlayer.ToCreate = false;
                UIPlayer.ToPlay = false;
            }

            DragonEngine.AllowAltTabPause(!ParticleMenu.OpenPIBEditor);

            HActPlayer.Update();
            PIBEditorMainWindow.GameUpdate();
        }

        public override void OnModInit()
        {
            DragonEngine.Log("DebugTools Start");
            DragonEngine.Log("DebugTools Imgui Update Registered");

            m_enumNames_VirtualKey = Enum.GetNames(typeof(VirtualKey));
            m_enumValues_VirtualKey = Enum.GetValues(typeof(VirtualKey));

            DragonEngine.RegisterJob(Update, DEJob.Update);

            Thread inputThread = new Thread(InputThread);
            inputThread.Start();


            DragonEngine.Log("Testy " + DragonEngine.TestFunc());
            try
            {
                DragonEngineLibrary.Advanced.DXHook.Init();
                DragonEngineLibrary.Advanced.ImGui.RegisterUIUpdate(ModUI);
                MinHookHelper.initialize();
            }
            catch
            {

            }

        }
    }
}
