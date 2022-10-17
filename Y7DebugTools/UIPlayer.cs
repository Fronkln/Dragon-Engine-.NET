﻿using System;
using DragonEngineLibrary;
using ImGuiNET;

namespace Y7DebugTools
{
    public static class UIPlayer
    {
        private static UIHandleBase m_lastUI;
        private static UIHandleBase m_selectedUI; //hierarchy

        private static int m_scene;
        private static int m_texture;
        private static int m_animation_set;
        private static float m_setWidth;
        private static float m_setValue;

        public static bool Open;

        public static bool ToCreate = false;
        public static bool ToPlay = false;

        private static byte[] handleBuf = new byte[24];

        private static byte[] txtBuf = new byte[256];


        private static float m_controlX;
        private static float m_controlY;

        private static int m_color;

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
                    ToCreate = true;

                if (ImGui.Button("Play"))
                    ToPlay = true;

                ImGui.Dummy(new System.Numerics.Vector2(0, 15));

                if (ImGui.CollapsingHeader("UI"))
                {
                    UIRecursion(m_lastUI);
                }

            }

            DrawHandleInfo();
        }

        public static void Create()
        {
            m_lastUI = UI.Create((uint)m_scene, 1);
        }

        public static void Play()
        {
            m_lastUI = UI.Play((uint)m_scene, 0);
        }

        public static void DrawHandleInfo()
        {
            if (ImGui.Begin("Properties"))
            {
                ImGui.Text("Texture: " + m_selectedUI.GetTexture());

                Vector4 pos = m_selectedUI.GetPosition();

                ImGui.Text("X: " + pos.x);
                ImGui.Text("Y: " + pos.y);
                ImGui.Text("Z: " + pos.z);

                ImGui.InputFloat("X Pos", ref m_controlX);
                ImGui.InputFloat("Y Pos", ref m_controlY);

               if(ImGui.InputInt("Color", ref m_color))
                m_selectedUI.SetMaterialColor((uint)m_color);

                if (ImGui.Button("Apply"))
                {
                    Vector4 vec = new Vector4();
                    vec.x = m_controlX;
                    vec.y = m_controlY;

                    m_selectedUI.SetPosition(vec);
                    m_selectedUI.SetMaterialColor((uint)m_color);
                }

                if (ImGui.InputFloat("Width", ref m_setWidth))
                    m_selectedUI.SetWidth(m_setWidth);

                if (ImGui.InputFloat("Value", ref m_setValue))
                    m_selectedUI.SetValue(m_setValue);

                if (ImGui.InputText("Text", txtBuf, (uint)txtBuf.Length))
                    m_selectedUI.SetText(System.Text.Encoding.UTF8.GetString(txtBuf));

                if (ImGui.InputInt("Texture", ref m_texture))
                    m_selectedUI.SetTexture((uint)m_texture);

                ImGui.InputInt("Animation Set", ref m_animation_set);

                if (ImGui.Button("Play Anim"))
                    m_selectedUI.PlayAnimationSet((uint)m_animation_set);

                ImGui.End();
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

                if (ImGui.Button("Pause"))
                    ui.Pause();

                if (ImGui.Button("Properties"))
                    m_selectedUI.Handle = ui.Handle;

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
