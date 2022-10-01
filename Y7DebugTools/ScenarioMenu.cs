using System;
using System.Collections.Generic;
using System.Linq;
using DragonEngineLibrary;
using DragonEngineLibrary.Service;
using ImGuiNET;

namespace Y7DebugTools
{
    public static class ScenarioMenu
    {
        public static bool Open = false;

        private static string[] m_scenarioNames;
        private static Array m_scenarioValues;


        private static byte[] m_scenarioFilter = new byte[64];
        private static string m_appliedFilter = "";

        private static int[] filterIndices;

        static ScenarioMenu()
        {
            m_scenarioNames = Enum.GetNames(typeof(SceneConfigID));
            m_scenarioValues = Enum.GetValues(typeof(SceneConfigID));
        }

        public static void Draw()
        {
            if(ImGui.Begin("Scenario"))
            {

                ImGui.InputText("Filter", m_scenarioFilter, 64);


                if (ImGui.Button("Apply"))
                {
                    m_appliedFilter = System.Text.Encoding.UTF8.GetString(m_scenarioFilter).Split(new[] { '\0' }, 2)[0];


                    List<int> indices = new List<int>();

                    for (int i = 0; i < m_scenarioNames.Length; i++)
                        if (m_scenarioNames[i].StartsWith(m_appliedFilter))
                        {
                         //   DragonEngine.Log(i + " " + m_scenarioNames[i]);
                            indices.Add(i);
                        }

                    filterIndices = indices.ToArray();
                }

                if (string.IsNullOrEmpty(m_appliedFilter))
                    ImGui.Text("Please enter filter");
                else
                {
                    for(int i = 0; i < filterIndices.Length; i++)
                    {
                        if(ImGui.Button(m_scenarioNames[filterIndices[i]]))
                        {
                            ScenePhaseBase scene = new EntityHandle<ScenePhaseBase>(SceneService.CurrentScene.UID);
                            scene.AlterActSwitchSceneID((SceneConfigID)m_scenarioValues.GetValue(filterIndices[i]), 0, new Vector4());
                        }
                    }
                }

                ImGui.End();
            }
                
        }
    }
}
