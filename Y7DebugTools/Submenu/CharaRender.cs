using System;
using DragonEngineLibrary;
using ImGuiNET;

namespace Y7DebugTools
{
    public static class CharaRender
    {
        public static void Draw(Character chara)
        {
            if (chara == null || !chara.IsValid())
            {
                ImGui.Text("Character does not exist.");
                return;
            }

            ImGui.Text("Position: " + (Vector3)chara.PosCenter);
            ImGui.Text("Up: " + chara.Orient * Vector3.up);
            ImGui.Text("Forward: " + chara.Orient * Vector3.forward);
            ImGui.Text("Right: " + chara.Orient * Vector3.right);
            ImGui.Text("Orientation: " + chara.Orient);

            if (ImGui.CollapsingHeader("Render Mesh"))
            {
                OrBox bounds = chara.GetRender().LocalBoundingBox;

                ImGui.Text("Bounds Center: " + bounds.Center);
                ImGui.Text("Bounds Extent: " + bounds.Extent);
            }

            ECConstructorCharacter constructor = chara.GetConstructor();

            if (constructor.IsValid() && ImGui.CollapsingHeader("Constructor"))
            {
                ECAgentCharacter agent = constructor.GetAgentComponent();

                if (agent.IsValid() && ImGui.CollapsingHeader("Agent"))
                {

                    AgentCharacterRender.Draw(agent);
                }

            }
        }
    }
}
