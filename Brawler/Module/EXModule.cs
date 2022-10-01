using DragonEngineLibrary;

namespace Brawler
{
    public static class EXModule
    {
        private static RepeatingTask m_exHeatDecay = new RepeatingTask(
            delegate
            {
                ECBattleStatus status = BrawlerBattleManager.Kasuga.GetStatus();
                status.Heat -= 1;
            }, 0.1f
            );

        public static void Update()
        {
            if (!BrawlerPlayer.IsEXGamer || HActManager.IsPlaying())
            {
                m_exHeatDecay.Paused = true;
                return;
            }

            m_exHeatDecay.Paused = false;

            if (BrawlerPlayer.IsEXGamer && !HActManager.IsPlaying())
            {
                int heat = BrawlerBattleManager.Kasuga.GetStatus().Heat;

                if (heat <= 0)
                {
                    BrawlerPlayer.IsEXGamer = false;
                    BrawlerPlayer.WantTransform = true;
                }
            }
        }
    }
}
