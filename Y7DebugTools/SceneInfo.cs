using System;
using DragonEngineLibrary;
using DragonEngineLibrary.Service;
using ImGuiNET;

namespace Y7DebugTools
{
    public static class SceneInfo
    {
        public static bool Open = false;

        public static void Draw()
        {
            if(ImGui.Begin("Scene Info"))
            {
                Scene scene = SceneService.CurrentScene.Get();


                ImGui.Text("Stage ID " + scene.StageID);
                ImGui.Text("Scene ID " + scene.SceneID);
                ImGui.Text("Scene Config ID " + scene.ConfigID);

                ImGui.End();
            }
        }
    }
}
