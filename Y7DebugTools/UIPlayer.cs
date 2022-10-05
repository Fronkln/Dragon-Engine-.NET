using System;
using DragonEngineLibrary;
using ImGuiNET;

namespace Y7DebugTools
{
    public static class UIPlayer
    {
        private static UIHandleBase m_lastUI;

        private static int m_scene;
        private static int m_animation_set;
        private static float m_setWidth;
        private static float m_setValue;

        public static bool Open;


        private static byte[] handleBuf = new byte[24];

        private static byte[] txtBuf = new byte[256];

        public static void Draw()
        {
            if (ImGui.Begin("UI Player"))
            {
                ImGui.InputInt("Scene ID", ref m_scene);
                ImGui.InputInt("Animation Set", ref m_animation_set);
                ImGui.InputText("UI Handle", handleBuf, 24);

                if (ImGui.Button("Hook to Handle"))
                    m_lastUI = new UIHandleBase() { Handle = ulong.Parse(System.Text.Encoding.UTF8.GetString(handleBuf)) };

                if (ImGui.Button("Create"))
                    m_lastUI = UI.Create((uint)m_scene, 1);


                ImGui.Dummy(new System.Numerics.Vector2(0, 15));

                if (ImGui.CollapsingHeader("UI"))
                {
                    UIRecursion(m_lastUI);
                }

            }
        }

        private static void UIRecursion(UIHandleBase ui)
        {
            if (ImGui.TreeNode("Control: " + ui.GetControlID() + " " + ui.Handle))
            {

                if (ImGui.Button("Show"))
                    ui.SetVisible(true);

                ImGui.SameLine(0, 20);

                if (ImGui.Button("Hide"))
                    ui.SetVisible(false);

                if (ImGui.InputFloat("Width", ref m_setWidth))
                    ui.SetWidth(m_setWidth);

                if (ImGui.InputFloat("Value", ref m_setValue))
                    ui.SetValue(m_setValue);
                
                if(ImGui.InputText("Text", txtBuf, (uint)txtBuf.Length))
                    ui.SetText(System.Text.Encoding.UTF8.GetString(txtBuf));

                ImGui.InputInt("Animation Set", ref m_animation_set);

                if (ImGui.Button("Play Anim"))
                    ui.PlayAnimationSet((uint)m_animation_set);

                if (ImGui.TreeNode("Children"))
                {
                    if (ui.GetChildCount() > 0)
                        for (int i = 0; i < ui.GetChildCount(); i++)
                            UIRecursion(ui.GetChild(i));


                    ImGui.TreePop();
                }

                ImGui.TreePop();
            }
        }
    }
}
