#define DO_NOT_USE_NATIVE_IMP
using DragonEngineLibrary;
using DragonEngineLibrary.Service;

namespace Brawler
{

    //Hooking into constructor would crash unless debugging.
    //This is the only way for now!
    internal static class BrawlerGaugeRestore
    {
        private static bool m_createdUIDoOnce = false;
        private static UIHandleBase m_healthGauge;

#if DO_NOT_USE_NATIVE_IMP
        private static UIHandleBase m_healthFrame;
        private static UIHandleBase m_gaugeNow;

        //recovery is simulated but damage isnt, up to me to save the day now
        private static UIHandleBase m_gaugeDamaged;

        private static bool m_isPinch = false;
        private static bool m_isDead = false;
        private static float lastPlayerHP;

#endif

        public static void Update()
        {
#if !DO_NOT_USE_NATIVE_IMP
            EntityHandle<UIEntityPlayerGauge> gauge = SceneService.CurrentScene.Get().GetSceneEntity<UIEntityPlayerGauge>(SceneEntity.ui_entity_player_gauge);

            if (gauge.IsValid())
            {
                if (!m_createdUIDoOnce)
                        Create(gauge.Get());
            }
            else
                m_createdUIDoOnce = false;

            Test();
#else

            if (DragonEngine.GetHumanPlayer().IsValid())
            {
                if (!m_createdUIDoOnce)
                    Create(null);
            }
            else
                m_createdUIDoOnce = false;


            if (m_healthGauge.Handle > 0)
                Test();
#endif
        }

        private static void Create(UIEntityPlayerGauge gauge)
        {
            m_createdUIDoOnce = true;
            m_healthGauge = UI.Create(301, 1);

            DragonEngine.Log("Gauge Handle: " + m_healthGauge.Handle);

#if !DO_NOT_USE_NATIVE_IMP
            gauge.LifeGauge = m_healthGauge;
#else
            m_healthFrame = m_healthGauge.GetChild(0).GetChild(0).GetChild(0);
            m_gaugeNow = m_healthGauge.GetChild(0).GetChild(0).GetChild(1);
#endif
        }

        public static float CalcHealthGaugeWidth(float min, float max, float maxHP)
        {
            if (maxHP < min)
                return min;
            if (maxHP > max)
                return max;

            return maxHP;
        }


        public static void Test()
        {

#if !DO_NOT_USE_NATIVE_IMP
            UIEntityPlayerGauge gauge = SceneService.CurrentScene.Get().GetSceneEntity<UIEntityPlayerGauge>(SceneEntity.ui_entity_player_gauge);

            if (gauge.IsValid())
            {
                gauge.GaugeNow.SetValue(1);
                gauge.GaugeNow.SetWidth(800);
                gauge.GaugeFrame.SetWidth(800);
            }
#else
            if (!DragonEngine.GetHumanPlayer().IsValid() || !BrawlerBattleManager.Kasuga.IsValid())
                m_healthGauge.SetVisible(false);
            else
                m_healthGauge.SetVisible(true);


            long playerHP = Player.GetHPNow(Player.ID.kasuga);
            long playerMaxHP = Player.GetHPMax(Player.ID.kasuga);


            float gaugeWidth = CalcHealthGaugeWidth(400, 800, Player.GetHPMax(Player.ID.kasuga));
           
            m_gaugeNow.SetWidth(gaugeWidth);
            m_healthFrame.SetWidth(gaugeWidth);


            if (lastPlayerHP <= (int)(playerMaxHP * Mod.CriticalHPRatio))
            {
                if(BrawlerBattleManager.Kasuga.IsDead())
                {
                    if(!m_isDead)
                    {
                        m_healthGauge.PlayAnimationSet(0x241);
                        m_isDead = true;
                    }
                }
                else
                {
                    if(m_isDead)
                    {
                        m_healthGauge.PlayAnimationSet(0x23A);
                        m_isDead = false;
                    }
                }

                if(!m_isPinch)
                {
                    m_healthGauge.PlayAnimationSet(0x23E);
                    m_isPinch = true;
                }
            }
            else
            {
                if(m_isPinch)
                {
                    m_healthGauge.PlayAnimationSet(0x23F);
                    m_isPinch = false;
                }
            }

            if (playerHP != lastPlayerHP)
            {
                if(playerHP < lastPlayerHP)
                    m_healthGauge.PlayAnimationSet(576);              
            }

            m_gaugeNow.SetValue((float)playerHP / (float)playerMaxHP);

            lastPlayerHP = playerHP;
#endif
        }
    }
}
