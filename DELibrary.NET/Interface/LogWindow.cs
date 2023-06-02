using DragonEngineLibrary.Diagnostics;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DragonEngineLibrary.Interface
{
    internal static class LogWindow
    {
        internal static bool isLogWindowVisible = false;
        private static bool isAutoScroll = true;
        private static bool isScrollToBottom = false;
        private static int eventFilter = 0;
        private static List<string> eventNames = Enum.GetNames(typeof(Logger.Event)).ToList();
        private static int eventEnumAmount = eventNames.Count;
        private static int currentSourceFilterIndex = 0;


        private static void SetEventColor(Logger.Event eventType)
        {
            uint color = Logger.GetColorForLogEventType(eventType);
            ImGui.PushStyleColor(ImGuiCol.Header, color);
            ImGui.PushStyleColor(ImGuiCol.HeaderHovered, color);
            ImGui.PushStyleColor(ImGuiCol.HeaderActive, color);
            ImGui.Selectable(string.Empty, true, ImGuiSelectableFlags.AllowItemOverlap | ImGuiSelectableFlags.SpanAllColumns);
            ImGui.SameLine();
            ImGui.PopStyleColor(3);
        }


        internal static void Draw()
        {
            if (isLogWindowVisible)
            {
                if (ImGui.Begin("Log", ref isLogWindowVisible, ImGuiWindowFlags.None))
                {

                    if (ImGui.BeginCombo("", eventFilter == 0 ? "All Events" : "Selected Events..."))
                    {
                        for (var i = 0; i < eventEnumAmount; i++)
                        {
                            if (ImGui.Selectable(eventNames[i], ((eventFilter >> i) & 1) == 1))
                            {
                                eventFilter ^= 1 << i;
                            }
                        }

                        ImGui.EndCombo();
                    }

                    ImGui.SameLine();
                    ImGui.Spacing();
                    ImGui.SameLine();
                    ImGui.Text("Source");
                    ImGui.SameLine();

                    var assemblyNames = new List<string>(AssemblyManager.ModNames);
                    assemblyNames.Insert(0, "All");
                    if (ImGui.Combo("Source", ref currentSourceFilterIndex, assemblyNames.ToArray(), assemblyNames.Count))
                    {
                    }

                    string sourceFilter = string.Empty;
                    if (currentSourceFilterIndex != 0)
                    {
                        sourceFilter = AssemblyManager.AssemblyNames[currentSourceFilterIndex-1];
                    }


                    ImGui.BeginChild("log_text");

                    List<Logger.Event> eventFilterList = new List<Logger.Event>();
                    for (var j = 0; j < eventEnumAmount; j++)
                    {
                        if ((eventFilter & (1 << j)) != 0)
                        {
                            eventFilterList.Add((Logger.Event)j);
                        }
                    }

                    List<Logger.LogMessage> filteredLogs = Logger.GetFilteredLogs(sourceFilter, eventFilterList); //todo
                    foreach (Logger.LogMessage logMessage in  filteredLogs)
                    {
                        if (logMessage.Message.Contains(Environment.NewLine))
                        {
                            string[] lines = logMessage.Message.Split('\n');
                            SetEventColor(logMessage.Event);
                            ImGui.TextUnformatted($"{logMessage.GetLogInfoString()} {lines[0]}\n");
                            
                            for (int j=1; j<lines.Length; j++)
                            {
                                SetEventColor(logMessage.Event);
                                ImGui.TextUnformatted(lines[j] +'\n');
                            }
                        }
                        else
                        {
                            SetEventColor(logMessage.Event);
                            ImGui.TextUnformatted(logMessage.ToString());
                        }

                        if (isScrollToBottom || (isAutoScroll && ImGui.GetScrollY() >= ImGui.GetScrollMaxY()))
                            ImGui.SetScrollHereY(1.0f);

                        
                    }

                    ImGui.EndChild();

                    ImGui.End();
                }
            }
        }
    }
}
