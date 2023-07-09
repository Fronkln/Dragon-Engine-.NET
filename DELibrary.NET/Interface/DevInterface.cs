using ImGuiNET;
using System;

namespace DragonEngineLibrary.Interface
{
    public class DevInterface
    {
        static bool isDevMenuVisible = true;


        internal static void Draw()
        {
            if (isDevMenuVisible)
            {
                if (ImGui.BeginMainMenuBar())
                {
                    if (ImGui.BeginMenu("DragonEngineLibrary"))
                    {
                        if (ImGui.MenuItem("Draw dev menu", null, isDevMenuVisible))
                        {
                            isDevMenuVisible = !isDevMenuVisible;
                        }
                        ImGui.Separator();
                        if (ImGui.MenuItem("Log window", null, LogWindow.isLogWindowVisible))
                        {
                            LogWindow.isLogWindowVisible = !LogWindow.isLogWindowVisible;
                        }
                        ImGui.Separator();

                        // Version Info
                        ImGui.TextDisabled(Diagnostics.LibraryAssembly.Version);
                        ImGui.TextDisabled(Diagnostics.LibraryAssembly.CommitHash);
                        ImGui.TextDisabled($"CLR: {Diagnostics.LibraryAssembly.CLRVersion}");
                        //

                        ImGui.EndMenu();
                    }

                    if (ImGui.BeginMenu("Mods"))
                    {
                        ImGui.TextDisabled("Loaded mods");
                        ImGui.Separator();
                        foreach (string modName in Diagnostics.AssemblyManager.ModNames)
                        {
                            int depCount = Diagnostics.AssemblyManager.GetModAssembly(modName).GetDependencyCount();
                            ImGui.Text($"{modName} [{depCount} dependencies]");
                            foreach (var name in Diagnostics.AssemblyManager.GetModAssembly(modName).GetDependencyNames())
                                ImGui.TextDisabled(name);
                        }

                        ImGui.EndMenu();
                    }

                    if (ImGui.BeginMenu("Assemblies"))
                    {
                        ImGui.TextDisabled("Loaded assemblies");
                        ImGui.Separator();
                        foreach (string modName in Diagnostics.AssemblyManager.ModNames)
                        {
                            foreach (System.Reflection.Assembly ass in AppDomain.CurrentDomain.GetAssemblies())
                            {
                                ImGui.TextDisabled(ass.FullName);
                            }
                        }

                        ImGui.EndMenu();
                    }

                    ImGui.Spacing();
                    ImGui.TextDisabled(Diagnostics.LibraryAssembly.CommitHash);
                    ImGui.Spacing();
                    ImGui.TextDisabled(ImGui.GetFrameCount().ToString("D8"));
                    ImGui.Spacing();
                    ImGui.TextDisabled($"FPS:{Convert.ToInt32(DragonEngine.FrameRate).ToString("D3")}");
                    ImGui.Spacing();
                    ImGui.TextDisabled($"W:{Diagnostics.Util.FormatBytes(Diagnostics.LibraryAssembly.GetProcessWorkingSet64())}");
                    ImGui.Spacing();
                    ImGui.TextDisabled($"V:{Diagnostics.Util.FormatBytes(Diagnostics.LibraryAssembly.GetProcessVirtualMemorySize64())}");
                    ImGui.EndMainMenuBar();
                }

                LogWindow.Draw();
            }
        }


        internal static void InputThread()
        {
            while (true)
            {
                if (DragonEngine.IsKeyDown(VirtualKey.F1))
                {
                    isDevMenuVisible = !isDevMenuVisible;
                }
            }
        }
    }
}
