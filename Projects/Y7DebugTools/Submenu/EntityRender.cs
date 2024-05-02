using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DragonEngineLibrary;
using ImGuiNET;

namespace Y7DebugTools
{
    internal class EntityRender
    {

        public static void Draw(EntityBase ent)
        {
            ImGui.Text("Handle UID " + ent.UID);
            ImGui.Text("Entity UID " + ent.EntityUID);
            ImGui.Text("Kind " + ent.EntityUID.Kind.ToString());
            ImGui.Text("Position: " + ent.GetPosCenter());
            ImGui.Text("Orient: " + ent.Transform.Orient);

            if (ImGui.Button("Teleport To Entity"))
                DragonEngine.GetHumanPlayer().RequestWarpPose(new PoseInfo(ent.GetPosCenter(), 0));
        }
    }
}
