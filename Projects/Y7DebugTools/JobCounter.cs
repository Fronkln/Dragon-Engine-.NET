using System;
using DragonEngineLibrary;

namespace Y7DebugTools
{
    public static class JobCounter
    {
        public static int[] m_count = new int[255];
        public static int[] m_countAverage = new int[255];

        private static bool m_countEnable = false;
        private static bool doOnce = false;

        private static float timer = 0;

        public static void Toggle(bool toggle)
        {
            if (toggle == m_countEnable)
                return;

            if (toggle)
            {
                timer = 0;

                for (int i = 0; i < m_count.Length; i++)
                    m_count[i] = 0;

                if (!doOnce)
                {
                    DragonEngine.RegisterJob(CounterUpdate, DEJob.Update);

                    DragonEngine.RegisterJob(delegate { Phase(DEJob.GameVarUpdate); }, DEJob.GameVarUpdate);
                    DragonEngine.RegisterJob(delegate { Phase(DEJob.SystemNormal); }, DEJob.SystemNormal);
                    DragonEngine.RegisterJob(delegate { Phase(DEJob.DisposeParse); }, DEJob.DisposeParse);
                    DragonEngine.RegisterJob(delegate { Phase(DEJob.DisposeLoad); }, DEJob.DisposeLoad);
                    DragonEngine.RegisterJob(delegate { Phase(DEJob.SceneJobRegister); }, DEJob.SceneJobRegister);
                    DragonEngine.RegisterJob(delegate { Phase(DEJob.EditorUpdate); }, DEJob.EditorUpdate);
                    DragonEngine.RegisterJob(delegate { Phase(DEJob.EntityJobRegister); }, DEJob.EntityJobRegister);
                    DragonEngine.RegisterJob(delegate { Phase(DEJob.StaticCollisionUpdate); }, DEJob.StaticCollisionUpdate);
                    DragonEngine.RegisterJob(delegate { Phase(DEJob.StaticCollisionFeedback); }, DEJob.StaticCollisionFeedback);
                    DragonEngine.RegisterJob(delegate { Phase(DEJob.PreUpdate); }, DEJob.PreUpdate);
                    DragonEngine.RegisterJob(delegate { Phase(DEJob.CCCUpdate); }, DEJob.CCCUpdate);
                    DragonEngine.RegisterJob(delegate { Phase(DEJob.OctTreeUpdate); }, DEJob.OctTreeUpdate);
                    DragonEngine.RegisterJob(delegate { Phase(DEJob.Update); }, DEJob.Update);
                    DragonEngine.RegisterJob(delegate { Phase(DEJob.UpdateCharacter); }, DEJob.UpdateCharacter);
                    DragonEngine.RegisterJob(delegate { Phase(DEJob.PostUpdate); }, DEJob.PostUpdate);
                    DragonEngine.RegisterJob(delegate { Phase(DEJob.PhysicsSetup); }, DEJob.PhysicsSetup);
                    DragonEngine.RegisterJob(delegate { Phase(DEJob.Update); }, DEJob.Update);
                    DragonEngine.RegisterJob(delegate { Phase(DEJob.UpdateCharacter); }, DEJob.UpdateCharacter);
                    DragonEngine.RegisterJob(delegate { Phase(DEJob.PostUpdate); }, DEJob.PostUpdate);
                    DragonEngine.RegisterJob(delegate { Phase(DEJob.PhysicsSetup); }, DEJob.PhysicsSetup);
                    DragonEngine.RegisterJob(delegate { Phase(DEJob.Physics); }, DEJob.Physics);
                    DragonEngine.RegisterJob(delegate { Phase(DEJob.PhysicsFeedback); }, DEJob.PhysicsFeedback);
                    DragonEngine.RegisterJob(delegate { Phase(DEJob.UpdateCharacterAfterPhysics); }, DEJob.UpdateCharacterAfterPhysics);
                    DragonEngine.RegisterJob(delegate { Phase(DEJob.Camera); }, DEJob.Camera);
                    DragonEngine.RegisterJob(delegate { Phase(DEJob.CameraFeedback); }, DEJob.CameraFeedback);
                    DragonEngine.RegisterJob(delegate { Phase(DEJob.UpdateUI); }, DEJob.UpdateUI);
                    DragonEngine.RegisterJob(delegate { Phase(DEJob.DrawSetup); }, DEJob.DrawSetup);
                    DragonEngine.RegisterJob(delegate { Phase(DEJob.Draw); }, DEJob.Draw);
                    DragonEngine.RegisterJob(delegate { Phase(DEJob.DebugDraw); }, DEJob.DebugDraw);
                    DragonEngine.RegisterJob(delegate { Phase(DEJob.Render); }, DEJob.Render);
                    DragonEngine.RegisterJob(delegate { Phase(DEJob.Cleanup); }, DEJob.Cleanup);

                    doOnce = true;
                }
            }

            m_countEnable = toggle;
        }

        private static void CounterUpdate()
        {
            timer += DragonEngine.deltaTime;

            if(timer <= 1)
            {
                timer = 0;

                for (int i = 0; i < m_count.Length; i++)
                {
                    m_countAverage[i] = m_count[i];
                    m_count[i] = 0;
                }
            }
        }


        public static void Phase(DEJob phase)
        {
            //cant unregister delegates dont care
            if (!m_countEnable)
                return;

            int phaseNum = (int)phase;
            m_count[phaseNum] = m_count[phaseNum] + 1;
        }
    }
}
