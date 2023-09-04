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

        private static int m_particleID;


        public static void Draw()
        {
            if(ImGui.Begin("Particle"))
            {

                ImGui.InputInt("Particle ID", ref m_particleID);

                if (ImGui.Button("Apply"))
                {
                    Matrix4x4 playPos = DragonEngine.GetHumanPlayer().GetPosture().GetRootMatrix();
                    playPos.Position = new Vector4(playPos.Position.x, playPos.Position.y + 1.5f, playPos.Position.z, 0);
                    ParticleManager.Play((ParticleID)m_particleID, playPos, ParticleType.None);
                }

                ImGui.End();
            }
                
        }
    }
}
