using System;
using DragonEngineLibrary;
using ImGuiNET;

namespace Y7DebugTools
{
    public static class ECAIRender
    {
        public static void Draw(ECAI ai)
        {
            if (ai == null)
                return;

            if (ImGui.CollapsingHeader("Request Command"))
                AIUtilCommandRender.Draw(ai.RequestCommand);

            if (ImGui.CollapsingHeader("Last AcceptedRequest Command"))
                AIUtilCommandRender.Draw(ai.LastAcceptedRequestCommand);
        }
    }
}
