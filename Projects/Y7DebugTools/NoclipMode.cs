using System;
using DragonEngineLibrary;

namespace Y7DebugTools
{
    public static class NoclipMode
    {
        private static bool m_enabled = false;

        private static Vector4 m_curPos;
        private static float m_curAng;

        private const float m_speed = 8;

        public static void Toggle()
        {
            Toggle(!m_enabled);
        }

        public static void Toggle(bool enabled)
        {
            if (m_enabled == enabled)
                return;

            if (enabled)
            {
                m_curPos = DragonEngine.GetHumanPlayer().Transform.Position;
                DragonEngine.RegisterJob(Update, DEJob.DrawSetup);
            }
            else
                DragonEngine.UnregisterJob(Update, DEJob.DrawSetup);

            m_enabled = enabled;
        }

        private static void Update()
        {
            if (GameVarManager.GetValueBool(GameVarID.is_pause))
                 return;

            Character player = DragonEngine.GetHumanPlayer();

            Vector3 movement = Vector3.zero;
            float rotation = 0;


            if (DragonEngine.IsKeyHeld(VirtualKey.Q))
                movement += -player.Transform.upDirection;
            if (DragonEngine.IsKeyHeld(VirtualKey.E))
                movement += player.Transform.upDirection;

            if (DragonEngine.IsKeyHeld(VirtualKey.A))
                rotation += 0.5f;
            if (DragonEngine.IsKeyHeld(VirtualKey.D))
                rotation -= 0.5f;

            if (DragonEngine.IsKeyHeld(VirtualKey.W))
                movement += player.Transform.forwardDirection;
            if (DragonEngine.IsKeyHeld(VirtualKey.S))
                movement += -player.Transform.forwardDirection;


            float outSpeed = m_speed * (DragonEngine.IsKeyHeld(VirtualKey.LeftShift) ? 2 : 1);

            m_curPos += (movement * outSpeed) * DragonEngine.deltaTime;
            m_curAng += (rotation * outSpeed) * DragonEngine.deltaTime;

            player.RequestWarpPose(new PoseInfo(m_curPos, m_curAng));
           // player.Transform.Position = m_curPos;
           // player.Transform.Orient = m_curAng;
        }
    }
}
