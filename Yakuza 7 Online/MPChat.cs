using System;
using System.Text;
using System.Collections.Generic;
using DragonEngineLibrary;
using ImGuiNET;


namespace Y7MP
{
    public static class MPChat
    {
        private static List<string> m_messages = new List<string>();
        private static bool m_chatOpen = true;

        public static bool focusOneTime = false;

        private static byte[] m_buffer = new byte[100];

        public static void Draw()
        {
            if (!m_chatOpen)
                return;

            ImGui.SetNextWindowSize(new System.Numerics.Vector2(307, 228), ImGuiCond.FirstUseEver);

            if (!ImGui.Begin("Chat"))
            {
                ImGui.End();
                return;
            }

            if (ImGui.BeginPopupContextItem())
            {
                if (ImGui.MenuItem("Close Console"))
                    m_chatOpen = false;
                ImGui.EndPopup();
            }

            ImGui.Separator();

            // Options, Filter
            if (ImGui.Button("Options"))
                ImGui.OpenPopup("Options");

            // Reserve enough left-over height for 1 separator + 1 input text
             float footer_height_to_reserve = ImGui.GetStyle().ItemSpacing.Y + ImGui.GetFrameHeightWithSpacing();

            ImGui.BeginChild("ScrollingRegion", new System.Numerics.Vector2(0, -footer_height_to_reserve), false, ImGuiWindowFlags.HorizontalScrollbar);
            if (ImGui.BeginPopupContextWindow())
            {
                if (ImGui.Selectable("Clear"))
                    ClearMessage();

                ImGui.EndPopup();
            }

            ImGui.PushStyleVar(ImGuiStyleVar.ItemSpacing, new System.Numerics.Vector2(4, 1)); // Tighten spacing

            for (int i = 0; i < m_messages.Count; i++)
                ImGui.TextWrapped(m_messages[i]);
        
            //if (ScrollToBottom || (MPChat::AutoScroll && ImGui.GetScrollY() >= ImGui.GetScrollMaxY()))
            ImGui.SetScrollHereY(1.0f);
            // ScrollToBottom = false;

            ImGui.PopStyleVar();
            ImGui.EndChild();
            ImGui.Separator();


            ImGuiInputTextFlags input_text_flags = ImGuiInputTextFlags.EnterReturnsTrue | ImGuiInputTextFlags.CallbackCompletion | ImGuiInputTextFlags.CallbackHistory;

            if (focusOneTime)
            {
                ImGui.SetKeyboardFocusHere(0);
                focusOneTime = false;
            }

            bool reclaim_focus = false;

            if (ImGui.InputText("Input", m_buffer, (uint)m_buffer.Length, input_text_flags))
            {
                string str = Encoding.UTF8.GetString(m_buffer);
                byte[] strBytes = Encoding.UTF8.GetBytes(str);
                Array.Clear(m_buffer, 0, m_buffer.Length);

                NetPacket chatMsg = new NetPacket(false);
                chatMsg.Writer.Write((byte)PacketMessage.PlayerChatMessage);
                chatMsg.Writer.Write(str);
                //  chatMsg.Writer.Write(strBytes.Length);
                //chatMsg.Writer.Write(strBytes);

                MPManager.SendToEveryone(chatMsg);

                reclaim_focus = true;
            }

            // Auto-focus on window apparition
            ImGui.SetItemDefaultFocus();

            if (reclaim_focus)
                ImGui.SetKeyboardFocusHere(-1); // Auto focus previous widget

            ImGui.End();
        }

        public static void AddMessage(string message)
        {
            m_messages.Add(message);
        }

        public static void ClearMessage() => m_messages.Clear();
    }
}
