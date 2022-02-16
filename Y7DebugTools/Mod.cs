﻿using System;
using System.Threading;
using System.Collections.Generic;
using DragonEngineLibrary;
using ImGuiNET;

namespace Y7DebugTools
{
    public class Mod : DragonEngineMod
    {
        public static Array m_enumValues_VirtualKey;
        public static string[] m_enumNames_VirtualKey;

        private bool m_npcMenuEnabled;
        private bool m_playerMenuEnabled;
        private bool m_fighterManagerMenuEnabled;
        private bool m_battleTurnManagerMenuEnabled;
        private bool m_effectMenuEnabled = false;

        public void ModUI()
        {
            ImGui.Begin("Debug");
            ImGui.Checkbox("Player", ref m_playerMenuEnabled);
            ImGui.Checkbox("NPC", ref m_npcMenuEnabled);
            ImGui.Checkbox("FighterManager", ref m_fighterManagerMenuEnabled);
            ImGui.Checkbox("BattleTurnManager", ref m_battleTurnManagerMenuEnabled);
            ImGui.Checkbox("Effect", ref m_effectMenuEnabled);
            ImGui.EndMenu();
            ImGui.End();


            if (m_playerMenuEnabled)
                PlayerMenu.Draw();
            if (m_npcMenuEnabled)
                NPCMenu.Draw();
            if (m_fighterManagerMenuEnabled)
                FighterManagerMenu.Draw();
            if (m_battleTurnManagerMenuEnabled)
                BattleTurnManagerMenu.Draw();
            if (m_effectMenuEnabled)
                EffectEventMenu.Draw();
        }

        private static void InputThread()
        {
            while (true)
            {
                VirtualKey key = ((VirtualKey)m_enumValues_VirtualKey.GetValue(NPCMenu.SpawnBind));

                if (DragonEngine.IsKeyDown(key))
                    NPCMenu.Create();
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