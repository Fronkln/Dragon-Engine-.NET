using System;
using DragonEngineLibrary;
using ImGuiNET;

namespace Y7DebugTools
{
    public static class CharaRender
    {
        private static float m_X;
        private static float m_Y;
        private static float m_Z;

        public static void Draw(Character chara)
        {
            if (chara == null || !chara.IsValid())
            {
                ImGui.Text("Character does not exist.");
                return;
            }

            ImGui.Text("Position: " + (Vector3)chara.Transform.Position);
            ImGui.Text("Orientation: " + chara.Transform.Orient);
            ImGui.Text("Rotation Y: " + chara.GetAngleY());

            Matrix4x4 mtx = chara.GetMatrix();

            ImGui.Text("Forward Direction: " + mtx.ForwardDirection);
            ImGui.Text("Up Direction: " + mtx.UpDirection);
            ImGui.Text("Left Direction: " + mtx.LeftDirection);

            ImGui.InputFloat("X", ref m_X);
            ImGui.InputFloat("Y", ref m_Y);
            ImGui.InputFloat("Z", ref m_Z);

            if (ImGui.Button("Teleport"))
                chara.RequestWarpPose(new PoseInfo(new Vector4(m_X, m_Y, m_Z), 0));

            if (ImGui.CollapsingHeader("Render Mesh"))
            {
                OrBox bounds = chara.GetRender().LocalBoundingBox;

                ImGui.Text("Character ID: " + chara.GetRender().CharacterID);
                ImGui.Text("Bounds Center: " + bounds.Center);
                ImGui.Text("Bounds Extent: " + bounds.Extent);
            }

            if (ImGui.CollapsingHeader("HumanMode"))

            {
                //ImGui.Text("HumanMode: " + chara.HumanModeManager.CurrentMode.ModeName);
                //ImGui.Text("Next HumanMode: " + chara.HumanModeManager.NextMode.ModeName);
            }

            if (ImGui.CollapsingHeader("Constructor"))
            {
                ECConstructorCharacter constructor = chara.GetConstructor();

                if (constructor.IsValid())
                {

                    ECAgentCharacter agent = constructor.GetAgentComponent();

                    if (agent.IsValid() && ImGui.CollapsingHeader("Agent"))
                    {

                        AgentCharacterRender.Draw(agent);
                    }
                }

            }

            if (ImGui.CollapsingHeader("Motion"))
            {
                ECMotion motion = chara.GetMotion();

                ImGui.Text("Address: " + motion.Pointer.ToInt64().ToString("x"));

                ImGui.Text("Current GMT ID: " + motion.GmtID + " " + motion.BhvPartsInfo.gmt_id_);
                ImGui.Text("Current Behavior Action: " + motion.GetBehaviorState());
                ImGui.Text("Adventure Behavior Set: " + motion.GetBehaviorSet(MotionBehaviorType.Adventure));
                ImGui.Text("Battle Behavior Set: " + motion.GetBehaviorSet(MotionBehaviorType.Battle));

                if (ImGui.Button("Test"))
                    motion.RequestBehavior(MotionBehaviorType.Battle, BehaviorActionID.P_KRU_MOV_std_btl_cautnml_tfm);
            }
        }
    }
}
