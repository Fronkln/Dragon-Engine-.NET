using System;
using System.Numerics;
using System.Collections.Generic;
using DragonEngineLibrary;
using ImGuiNET;

namespace Y7DebugTools
{
    public static class NPCMenu
    {
        public static List<Character> CreatedNPCs = new List<Character>();
        public static List<NPCRequestMaterial> CreationQueue = new List<NPCRequestMaterial>();

        public static int SpawnBind = (int)VirtualKey.None;

        //Cached enum names and values
        private static bool m_firstDraw = true;
        private static string[] m_enumNames_BehaviorSet;
        private static string[] m_enumNames_HeightScale;
        private static string[] m_enumNames_MapIcon;
        private static string[] m_enumNames_SetupID;
        private static string[] m_enumNames_VoicerID;
        private static string[] m_enumNames_RPGEnemyID;
        private static string[] m_enumNames_PersonalDataGroupID;

        private static Array m_enumValues_BehaviorSet;
        private static Array m_enumValues_HeightScale;
        private static Array m_enumValues_MapIcon;
        private static Array m_enumValues_SetupID;
        private static Array m_enumValues_VoicerID;
        private static Array m_enumValues_RPGEnemyID;
        private static Array m_enumValues_PersonalDataGroupID;

        private static int m_chosenCharaID = (int)CharacterID.m_ichiban_23;
        private static int m_chosenBehaviorID = (int)BehaviorSetID.m_human_npc_base;
        private static int m_chosenHeightID = (int)CharacterHeightID.height_180;
        private static int m_chosenMapID = (int)MapIconID.sujimon;
        private static int m_chosenSetupID = (int)CharacterNPCSetup.invalid;
        private static int m_chosenVoicerID = (int)CharacterVoicerID.invalid;
        private static int m_chosenRpgEnemyID = (int)BattleRPGEnemyID.invalid;
        private static int m_chosenSoldierPersonalDataGroupID = (int)CharacterNPCSoldierPersonalDataGroupID.foo;

        private static bool m_isPermanent = true;
        private static bool m_isEncounter = false;
        private static bool m_isEncounterBtlType = false;

        private static void Cache()
        {
            m_enumNames_BehaviorSet = Enum.GetNames(typeof(BehaviorSetID));
            m_enumNames_HeightScale = Enum.GetNames(typeof(CharacterHeightID));
            m_enumNames_MapIcon = Enum.GetNames(typeof(MapIconID));
            m_enumNames_SetupID = Enum.GetNames(typeof(CharacterNPCSetup));
            m_enumNames_VoicerID = Enum.GetNames(typeof(CharacterVoicerID));
            m_enumNames_RPGEnemyID = Enum.GetNames(typeof(BattleRPGEnemyID));
            m_enumNames_PersonalDataGroupID = Enum.GetNames(typeof(CharacterNPCSoldierPersonalDataGroupID));

            m_enumValues_BehaviorSet = Enum.GetValues(typeof(BehaviorSetID));
            m_enumValues_HeightScale = Enum.GetValues(typeof(CharacterHeightID));
            m_enumValues_MapIcon = Enum.GetValues(typeof(MapIconID));
            m_enumValues_SetupID = Enum.GetValues(typeof(CharacterNPCSetup));
            m_enumValues_VoicerID = Enum.GetValues(typeof(CharacterVoicerID));
            m_enumValues_RPGEnemyID = Enum.GetValues(typeof(BattleRPGEnemyID));
            m_enumValues_PersonalDataGroupID = Enum.GetValues(typeof(CharacterNPCSoldierPersonalDataGroupID));
        }

        public static void Create()
        {
            NPCRequestMaterial material = new NPCRequestMaterial();
            material.Material = new NPCMaterial();

            Character player = DragonEngine.GetHumanPlayer();

            material.Material.pos_ = player.GetPosCenter() + player.forwardDirection * 1.5f;
            material.Material.character_id_ = (CharacterID)m_chosenCharaID;
            material.Material.height_scale_id_ = (CharacterHeightID)m_enumValues_HeightScale.GetValue(m_chosenHeightID);
            material.Material.behavior_set_id_ = (BehaviorSetID)m_enumValues_BehaviorSet.GetValue(m_chosenBehaviorID);
            material.Material.npc_setup_id_ = (CharacterNPCSetup)m_enumValues_SetupID.GetValue(m_chosenSetupID);
            material.Material.map_icon_id_ = (MapIconID)m_enumValues_MapIcon.GetValue(m_chosenMapID);
            material.Material.voicer_id_ = (CharacterVoicerID)m_enumValues_VoicerID.GetValue(m_chosenVoicerID);

            material.Material.enemy_id_ = (BattleRPGEnemyID)m_enumValues_RPGEnemyID.GetValue(m_chosenRpgEnemyID);
            material.Material.personal_data_group_id_ = (CharacterNPCSoldierPersonalDataGroupID)m_enumValues_PersonalDataGroupID.GetValue(m_chosenSoldierPersonalDataGroupID);

            material.Material.collision_type_ = 0;

            material.Material.is_eternal_life_ = m_isPermanent;
            material.Material.is_encounter_ = m_isEncounter;
            material.Material.is_encount_btl_type_ = m_isEncounterBtlType;

            material.Material.parent_ = DragonEngineLibrary.Service.SceneService.CurrentScene.Get().GetSceneEntity<EntityBase>(SceneEntity.character_manager).UID;

            material.Material.is_force_create_ = true;
            material.Material.is_force_visible_ = true;

            CreationQueue.Add(material);
        }

        public static void Draw()
        {
            if(m_firstDraw)
            {
                Cache();
                m_firstDraw = false;
            }

            if(ImGui.Begin("NPC Creator"))
            {
                ImGui.InputInt("Character ID:", ref m_chosenCharaID);
                ImGui.Combo("Voicer ID:", ref m_chosenVoicerID, m_enumNames_VoicerID, m_enumNames_VoicerID.Length);
                ImGui.Combo("Behavior Set:", ref m_chosenBehaviorID, m_enumNames_BehaviorSet, m_enumNames_BehaviorSet.Length);
                ImGui.Combo("RPG Enemy ID:", ref m_chosenRpgEnemyID, m_enumNames_RPGEnemyID, m_enumNames_RPGEnemyID.Length);
                ImGui.Combo("Personal Data Group ID:", ref m_chosenSoldierPersonalDataGroupID, m_enumNames_PersonalDataGroupID, m_enumNames_PersonalDataGroupID.Length);
                ImGui.Combo("Setup:", ref m_chosenSetupID, m_enumNames_SetupID, m_enumNames_SetupID.Length);
                ImGui.Combo("Height Scale:", ref m_chosenHeightID, m_enumNames_HeightScale, m_enumNames_HeightScale.Length);
                ImGui.Combo("Map Icon:", ref m_chosenMapID, m_enumNames_MapIcon, m_enumNames_MapIcon.Length);


                ImGui.Dummy(new Vector2(0, 20));
                ImGui.Checkbox("Permanent", ref m_isPermanent);
                ImGui.Checkbox("Encounter", ref m_isEncounter);
                ImGui.Checkbox("Encounter Battle Type", ref m_isEncounterBtlType);

                ImGui.Dummy(new Vector2(0, 10));

                if (ImGui.Button("Spawn"))
                    Create();

                if (ImGui.Button("Clear Spawned"))
                {
                    foreach (Character chara in CreatedNPCs)
                        chara.DestroyEntity();

                    CreatedNPCs.Clear();
                }

                ImGui.Combo("Spawn Bind", ref SpawnBind, Mod.m_enumNames_VirtualKey, Mod.m_enumNames_VirtualKey.Length);

                ImGui.End();
            }
        }
    }
}
