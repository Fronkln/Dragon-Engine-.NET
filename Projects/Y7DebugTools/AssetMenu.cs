using DragonEngineLibrary;
using ImGuiNET;

namespace Y7DebugTools
{
    internal static class AssetMenu
    {
        public static int CreateID = 0;
        public static bool CreateNext;
        public static bool Enabled = false;

        public static void Draw()
        {

            if(Enabled)
            {
                ImGui.Begin("Asset");

                AssetUnit nearest = AssetManager.FindNearestAssetFromAll(DragonEngine.GetHumanPlayer().Transform.Position, 0);
                ImGui.Text("Nearest Asset ID: " + nearest.AssetID + $"({(uint)nearest.AssetID})");
                ImGui.InputInt("Asset ID", ref CreateID);

                if (ImGui.Button("Create"))
                    CreateNext = true;

                ImGui.End();
            }
        }
    }
}
