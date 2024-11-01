using System;
using System.Collections.Generic;
using System.Linq;
using DragonEngineLibrary;
using DragonEngineLibrary.Service;
using ImGuiNET;

namespace Y7DebugTools
{
    public static class ParticleMenu
    {
        public static bool Open = false;
        public static bool OpenPIBEditor = false;

        public static int ParticleID;

        private static bool m_loadRaw = false;


        public static void Draw()
        {
            if(ImGui.Begin("Particle"))
            {
                ImGui.InputInt("Particle ID", ref ParticleID);
                ImGui.Checkbox("Open PIB Editor", ref OpenPIBEditor);

                if (OpenPIBEditor)
                    PIBEditorMainWindow.Draw();

                if (ImGui.Button("Play"))
                {
                    Matrix4x4 playPos = DragonEngine.GetHumanPlayer().GetMatrix();
                    playPos.Position = new Vector4(playPos.Position.x, playPos.Position.y + 1.5f, playPos.Position.z, 0);
                    DragonEngine.Log("PTC " +  ParticleManager.Play((ParticleID)ParticleID, playPos, ParticleType.None));
                }
                ImGui.Checkbox("Load Raw Event", ref m_loadRaw);


                if (ImGui.Button("Load"))
                {
                    ParticleManager.LoadRaw((ParticleID)ParticleID, m_loadRaw);
                }

                if (ImGui.Button("Unload"))
                {
                    ParticleManager.UnloadRaw((ParticleID)ParticleID);
                }

                ImGui.End();
            }
                
        }
    }
}
