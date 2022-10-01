using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DragonEngineLibrary;

namespace Brawler
{
    internal class DETaskTime : DETask
    {
        private float m_time = 0;
        private float m_targetTime = 0;

        public DETaskTime(float time, Action onFinish, bool autoStart = true) : base(null, onFinish, autoStart)
        {
            m_targetTime = time;
            m_Func = delegate { return m_time >= m_targetTime; };
        }

        public override void Run()
        {
            m_time += DragonEngine.deltaTime;

            base.Run();
        }
    }
}
