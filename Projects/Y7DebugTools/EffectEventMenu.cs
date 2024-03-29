﻿using System;
using DragonEngineLibrary;
using ImGuiNET;

namespace Y7DebugTools
{
    public static class EffectEventMenu
    {
        private static int m_curEffectID = 0;
        private static int m_curEffectID2 = 0;

        private static int m_screenEffectID = 0;

        public static void Draw()
        {
            Character player = DragonEngine.GetHumanPlayer();

            if (!player.IsValid())
                return;

            if(ImGui.Begin("Effect Event Menu"))
            {
                ImGui.Text("Character");

                ImGui.InputInt("Effect ID 1", ref m_curEffectID);
                ImGui.InputInt("Effect ID 2", ref m_curEffectID2);

                if (ImGui.Button("Play"))
                    player.Components.EffectEvent.Get().PlayEvent((EffectEventCharaID)m_curEffectID, (EffectEventCharaID)m_curEffectID2);
                if (ImGui.Button("Play Override"))
                    player.Components.EffectEvent.Get().PlayEventOverride((EffectEventCharaID)m_curEffectID);

                if (ImGui.Button("Stop Effect ID 1"))
                    player.Components.EffectEvent.Get().StopEvent((EffectEventCharaID)m_curEffectID, true);
                if (ImGui.Button("Stop Effect ID 2"))
                    player.Components.EffectEvent.Get().StopEvent((EffectEventCharaID)m_curEffectID2, true);
                if (ImGui.Button("Stop All"))
                    player.Components.EffectEvent.Get().StopEventAll();


                ImGui.Text("Screen");

                ImGui.InputInt("Screen Effect ID", ref m_screenEffectID);

                if(ImGui.Button("Play Screen"))
                {
                    EffectEventManager.LoadScreen((uint)m_screenEffectID);
                    EffectEventManager.PlayScreen((uint)m_screenEffectID);
                }

                ImGui.End();
            }
        }
    }
}
