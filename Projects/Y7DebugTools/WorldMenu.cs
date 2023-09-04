using DragonEngineLibrary;
using ImGuiNET;

namespace Y7DebugTools
{
    internal static class WorldMenu
    {
        public static bool Open;
        public static bool Active;

        private static float m_generalSpeed = 1;
        private static float m_playerSpeed = 1;
        private static float m_characterSpeed = 1;
        private static float m_unsortedSpeed = 1;


        public static void Update()
        {
            DragonEngine.SetSpeed(DESpeedType.General, m_characterSpeed);
            DragonEngine.SetSpeed(DESpeedType.Character, m_characterSpeed);
            DragonEngine.SetSpeed(DESpeedType.Player, m_playerSpeed);
            DragonEngine.SetSpeed(DESpeedType.Unprocessed, m_unsortedSpeed);
        }

        public static void Draw()
        {        
            if(ImGui.Begin("World"))
            {
                if(ImGui.Checkbox("Enabled", ref Active))
                {
                    if (Active)
                        DragonEngine.RegisterJob(Update, DEJob.Update);
                    else
                        DragonEngine.UnregisterJob(Update, DEJob.Update);
                }

                ImGui.InputFloat("General Speed", ref m_generalSpeed);
                ImGui.InputFloat("Player Speed", ref m_playerSpeed);
                ImGui.InputFloat("Character Speed", ref m_characterSpeed);
                ImGui.InputFloat("Unsorted Speed", ref m_unsortedSpeed);

                if (Active)
                {

                    if(DragonEngine.IsKeyDown(VirtualKey.P))
                    {
                        if(m_generalSpeed > 0)
                        {
                            m_generalSpeed = 0;
                            m_characterSpeed = 0;
                            m_playerSpeed = 0;
                            m_unsortedSpeed = 0;
                        }
                        else
                        {
                            m_generalSpeed = 1;
                            m_characterSpeed = 1;
                            m_playerSpeed = 1;
                            m_unsortedSpeed = 1;
                        }
                    }
                }

                ImGui.End();
            }
        }
    }
}
