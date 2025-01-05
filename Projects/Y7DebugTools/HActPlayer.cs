using System;
using System.Linq;
using System.Timers;
using System.Collections.Generic;
using DragonEngineLibrary.Service;
using ImGuiNET;
using DragonEngineLibrary;

namespace Y7DebugTools
{
    public static class HActPlayer
    {
        private static int m_chosenHAct = 0;
        private static int m_chosenHActRange = 0;

        private static bool m_battleMode = false;
        private static bool m_isPlayerAction = true;
        private static bool m_atPlayerPos = true;
        private static bool m_canSkip = true;
        private static bool m_forcePlay = true;
        private static bool m_simple = true;
        private static bool m_rpgPlayer = false;

        private static int[] m_chosenIDs = new int[(int)HActReplaceID.num];

        private static string[] m_enumNames_HactReplace;
        private static string[] m_enumNames_HactRange;

        private static List<EntityHandle<Character>> m_createdHActChara = new List<EntityHandle<Character>>();
        private static bool m_cleanDoOnce = false;

        private static bool m_allyIsPlayer = false;
        private static bool m_allyIsAlly = true;
        private static bool m_allyIsNPC = false;
        private static bool m_allyIsEnemy = false;
        private static bool m_Reverse = false;
        private static bool m_closeOnPlay = false;

        public static bool hactTimeLine = false;


        private static bool m_wantPlay = false;

        static HActPlayer()
        {
            m_enumNames_HactReplace = Enum.GetNames(typeof(HActReplaceID));
            m_enumNames_HactRange = Enum.GetNames(typeof(HActRangeType));
        }

        private static EntityHandle<Character> CreateHActChara(CharacterID id)
        {

            return BattleResourceManager.CreateTempHActChara(id, Player.ID.invalid);

            //   return NPCFactory.RequestCreate(material);

        }

        private static void Play()
        {
            foreach (EntityHandle<Character> chara in m_createdHActChara)
                if (chara.IsValid())
                {
                    DragonEngine.Log("Disposed valid NPC");
                    chara.Get().DestroyEntity();
                }

            bool inFight = FighterManager.GetAllEnemies().Length > 0;

            m_createdHActChara.Clear();

            HActRequestOptions opts = new HActRequestOptions();
            opts.id = (TalkParamID)m_chosenHAct;

            if (m_atPlayerPos)
                opts.base_mtx.matrix = DragonEngine.GetHumanPlayer().GetMatrix();

            if (m_chosenHActRange > 0 && FighterManager.GetPlayer().IsValid())
            {
                HActRangeInfo inf = new HActRangeInfo();

                if (FighterManager.GetPlayer().GetStatus().HAct.GetPlayInfo(ref inf, (HActRangeType)m_chosenHActRange))
                    opts.base_mtx.matrix = inf.GetMatrix();
            }

            //opts.is_warp_return = true;
            //opts.is_player_action = m_isPlayerAction;
            opts.is_force_play = m_forcePlay;
            //opts.can_skip = m_canSkip;

            if (!m_simple)
            {
                //Register everything for the hacts
                for (int i = 0; i < m_chosenIDs.Length; i++)
                {
                    if (i > 0)
                    {
                        if (m_chosenIDs[i] > 0)
                        {
                            EntityHandle<Character> hactChara = CreateHActChara((CharacterID)m_chosenIDs[i]);
                            m_createdHActChara.Add(hactChara);

                            opts.Register((HActReplaceID)i, hactChara.UID);

                            DragonEngine.Log("Register: " + ((HActReplaceID)i).ToString() + " Chara: " + ((CharacterID)m_chosenIDs[i]).ToString());
                        }
                    }
                }
            }
            else
            {
                if (!inFight)
                {
                    if (!m_allyIsPlayer)
                    {
                       // opts.Register(HActReplaceID.hu_player, DragonEngine.GetHumanPlayer().UID);
                        opts.Register(HActReplaceID.hu_player1, DragonEngine.GetHumanPlayer().UID);
                    }
                    else
                    {
                        opts.Register(HActReplaceID.hu_player, NakamaManager.GetCharacterHandle(1).UID);
                        opts.Register(HActReplaceID.hu_player1, NakamaManager.GetCharacterHandle(1).UID);
                    }
                }
                else
                {
                    // opts.Register(HActReplaceID.hu_player1, FighterManager.GetFighter(0).Character);
                    // opts.RegisterWeapon(AuthAssetReplaceID.we_player_r, FighterManager.GetFighter(0).GetWeapon(AttachmentCombinationID.right_weapon).Unit.Get().AssetID);

                    opts.Register(HActReplaceID.hu_player1, FighterManager.GetFighter(0).CharacterUID);
                    opts.RegisterWeapon(AuthAssetReplaceID.we_player1_r, FighterManager.GetFighter(0).GetWeapon(AttachmentCombinationID.right_weapon));
                    opts.RegisterWeapon(AuthAssetReplaceID.we_player1_l, FighterManager.GetFighter(0).GetWeapon(AttachmentCombinationID.left_weapon));
                }

                Fighter[] enemies = FighterManager.GetAllEnemies();
                HActReplaceID curEnemy = HActReplaceID.hu_enemy_00;

                if (!m_allyIsEnemy)
                    foreach (Fighter enemy in enemies)
                    {
                        opts.Register(curEnemy, enemy.Character);
                        curEnemy = (HActReplaceID)((uint)curEnemy + 1);
                    }

                HActReplaceID curAllyRegister = HActReplaceID.invalid;

                if (m_allyIsAlly)
                {
                    curAllyRegister = (!m_Reverse ? HActReplaceID.hu_nakama_00 : HActReplaceID.hu_nakama_03);
                }

                if (m_allyIsNPC)
                {
                    curAllyRegister = (!m_Reverse ? HActReplaceID.hu_npc_00 : HActReplaceID.hu_npc_10);
                }


                if (m_allyIsEnemy)
                {
                    curAllyRegister = (!m_Reverse ? HActReplaceID.hu_enemy_00 : HActReplaceID.hu_enemy_10);
                }



                for (int i = 1; i < 4; i++)
                {
                    if (i == 1 && m_allyIsPlayer)
                        i++;

                    if (inFight)
                        opts.Register(curAllyRegister, FighterManager.GetFighter((uint)i).Character);
                    else
                        opts.Register(curAllyRegister, NakamaManager.GetCharacterHandle((uint)i).UID);

                    if (!m_Reverse)
                        curAllyRegister = (HActReplaceID)((uint)curAllyRegister + 1);
                    else
                        curAllyRegister = (HActReplaceID)((uint)curAllyRegister - 1);
                }
            }

            PlayHAct(opts);

            if(m_closeOnPlay)
            {
                Mod.Visible = false;
                m_closeOnPlay = false;
            }
        }

        private static void PlayHAct(HActRequestOptions inf)
        {
            bool result;

#if TURN_BASED
            if (m_battleMode)
                result = BattleTurnManager.RequestHActEvent(inf);
            else
                result = HActManager.RequestHAct(inf);
#else
            result = HActManager.RequestHAct(inf);
#endif


            if (!result)
                DragonEngine.Log("HAct fail");
        }

        public static void DrawInputThread()
        {
            if (!GameVarManager.GetValueBool(GameVarID.is_hact))
                return;

            if (!hactTimeLine)
                return;

            AuthPlay currentHAct = AuthManager.PlayingScene;

            if (m_pause)
                currentHAct.SetGameFrame(m_pauseFrame);
        }

        private static bool m_pause = false;
        private static float m_pauseFrame = 0;
        public static void DrawTimeline()
        {
            if (!GameVarManager.GetValueBool(GameVarID.is_hact))
            {
                m_pause = false;
                m_pauseFrame = 0;
                return;
            }

            if (!hactTimeLine)
                return;


            if (ImGui.Begin("HAct Timeline"))
            {
                AuthPlay currentHAct = AuthManager.PlayingScene;

                DragonEngine.Log(currentHAct.Pointer.ToString("X"));

                if (ImGui.Button("Stop"))
                    HActManager.Skip();

                float test = currentHAct.GetGameFrame();

                ImGui.Text("HAct: " + currentHAct.TalkParamID + $"({(uint)currentHAct.TalkParamID})");
                ImGui.Text("Current Page ID: " + currentHAct.GetCurrentPageIndex());

                if (ImGui.SliderFloat("Frame", ref test, 0, currentHAct.GetEndFrame()))
                {
                    m_pauseFrame = test;
                    currentHAct.SetGameFrame(test);
                }

                if (!m_pause)
                {
                    if (ImGui.Button("Pause"))
                    {
                        m_pause = !m_pause;

                        if (m_pause)
                            m_pauseFrame = currentHAct.GetGameFrame();

                        //currentHAct.SetSpeed(0);
                    }
                }
                else
                {
                    if (ImGui.Button("Play"))
                    {
                        m_pause = false;
                    }
                }

                ImGui.SameLine(0, 30);


                ImGui.SameLine(0, 45);

                if (ImGui.Button("Reset"))
                    currentHAct.Restart();

                ImGui.End();
            }

        }

        public static void Draw()
        {

            if (ImGui.Begin("Hact Player"))
            {
                ImGui.InputInt("HAct ID", ref m_chosenHAct);
                ImGui.InputInt("Range ID", ref m_chosenHActRange);

                ImGui.Checkbox("HAct Timeline", ref hactTimeLine);

                ImGui.Dummy(new System.Numerics.Vector2(0, 10));

                ImGui.Checkbox("Battle Mode", ref m_battleMode);

                ImGui.Dummy(new System.Numerics.Vector2(0, 10));

                ImGui.Checkbox("Is Player Action", ref m_isPlayerAction);
                ImGui.Checkbox("Play at Player Pos", ref m_atPlayerPos);
                ImGui.Checkbox("Can Skip", ref m_canSkip);
                ImGui.Checkbox("Force Play", ref m_forcePlay);
                ImGui.Checkbox("Simple", ref m_simple);

#if TURN_BASED
                ImGui.Checkbox("Turn Based Player", ref m_rpgPlayer);
#endif

                ImGui.Checkbox("Close On Play", ref m_closeOnPlay);

                ImGui.Dummy(new System.Numerics.Vector2(0, 20));

                if (ImGui.Button("Play"))
                    m_wantPlay = true;


                if (!m_simple)
                {
                    ImGui.Text("Character ID's");

                    for (int i = 0; i < (int)HActReplaceID.num; i++)
                        ImGui.InputInt(m_enumNames_HactReplace[i], ref m_chosenIDs[i]);
                }
                else
                {
                    ImGui.Checkbox("Reverse Order", ref m_Reverse);

                    ImGui.Text("Ally is:");

                    ImGui.Checkbox("Player", ref m_allyIsPlayer);

                    if (ImGui.Checkbox("Ally", ref m_allyIsAlly))
                        if (m_allyIsEnemy)
                        {
                            m_allyIsNPC = false;
                            m_allyIsEnemy = false;
                        }

                    if (ImGui.Checkbox("NPC", ref m_allyIsNPC))
                        if (m_allyIsEnemy)
                        {
                            m_allyIsAlly = false;
                            m_allyIsEnemy = false;
                        }

                    if (ImGui.Checkbox("Enemy", ref m_allyIsEnemy))
                    {
                        if (m_allyIsEnemy)
                        {
                            m_allyIsAlly = false;
                            m_allyIsNPC = false;
                        }
                    }


                    ImGui.Dummy(new System.Numerics.Vector2(0, 10));

                    Fighter kasugaFighter = FighterManager.GetPlayer();

                    if (!kasugaFighter.IsValid())
                    {
                        for (int i = 1; i < m_enumNames_HactRange.Length; i++)
                        {
                            HActRangeInfo inf = new HActRangeInfo();
                            ImGui.Text(m_enumNames_HactRange[i] + " " + HActManager.FindRange(DragonEngine.GetHumanPlayer().Transform.Position, (HActRangeType)i, ref inf));
                        }
                    }
                    else
                    {
                        for (int i = 1; i < m_enumNames_HactRange.Length - 2; i++)
                        {

                            HActRangeInfo inf = new HActRangeInfo();
                            ImGui.Text(m_enumNames_HactRange[i] + " " + kasugaFighter.GetStatus().HAct.GetPlayInfo(ref inf, (HActRangeType)i) + " " + inf.is_play_range_in);
                        }
                    }

                }

                ImGui.End();
            }
        }
        
        public static void Update()
        {
            if(m_wantPlay)
            {
                Play();
                m_wantPlay = false;
            }
        }
    }
}
