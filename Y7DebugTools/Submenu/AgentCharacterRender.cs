using System;
using DragonEngineLibrary;
using ImGuiNET;

namespace Y7DebugTools
{
    public class AgentCharacterRender
    {
        public static void Draw(ECAgentCharacter chara)
        {
            if (chara == null)
                return;

            if (chara.AI.IsValid())
            {
                if (ImGui.CollapsingHeader("AI"))
                    ECAIRender.Draw(chara.AI);
            }
        }
    }
}
