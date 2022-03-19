using System;
using System.Collections.Generic;
using DragonEngineLibrary.Service;
using ImGuiNET;
using DragonEngineLibrary;

namespace Y7DebugTools
{
    public static class HActPlayer
    {
        private static int m_chosenHAct = 0;

        private static bool m_battleMode = true;
        private static bool m_isPlayerAction = true;
        private static bool m_canSkip = true;
        private static bool m_forcePlay = true;

        private static int[] m_chosenIDs = new int[(int)HActReplaceID.num];

        private static string[] m_enumNames_HactReplace;

        private static List<EntityHandle<Character>> m_createdHActChara = new List<EntityHandle<Character>>();

        static HActPlayer()
        {
            m_enumNames_HactReplace = Enum.GetNames(typeof(HActReplaceID));
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

            m_createdHActChara.Clear();

            HActRequestOptions opts = new HActRequestOptions();
            opts.Init();
            opts.id = (TalkParamID)m_chosenHAct;

            opts.is_warp_return = true;
            opts.is_player_action = m_isPlayerAction;
            opts.is_force_play = m_forcePlay;
            opts.can_skip = m_canSkip;

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

            bool result;

            if (m_battleMode)
                result = BattleTurnManager.RequestHActEvent(opts);
            else
                result = HActManager.RequestHAct(opts);

            if (!result)
                DragonEngine.Log("HAct fail");
        }

        public static void Draw()
        {
            if (ImGui.Begin("Hact Player"))
            {
                ImGui.InputInt("HAct ID", ref m_chosenHAct);
                ImGui.Checkbox("Battle Mode", ref m_battleMode);

                ImGui.Dummy(new System.Numerics.Vector2(0, 10));

                ImGui.Checkbox("Is Player Action", ref m_isPlayerAction);
                ImGui.Checkbox("Can Skip", ref m_canSkip);
                ImGui.Checkbox("Force Play", ref m_forcePlay);

                ImGui.Dummy(new System.Numerics.Vector2(0, 20));

                if (ImGui.Button("Play"))
                    Play();

                ImGui.Text("Character ID's");

                for (int i = 0; i < (int)HActReplaceID.num; i++)
                    ImGui.InputInt(m_enumNames_HactReplace[i], ref m_chosenIDs[i]);

            }
        }
    }
}
