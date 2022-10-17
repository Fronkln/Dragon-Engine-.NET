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
        private static UIHandleBase m_heatGauge;
        private static UIHandleBase m_canHact;
        private static UIHandleBase m_hactPrompt;

#if DO_NOT_USE_NATIVE_IMP
        private static UIHandleBase m_healthFrame;
        private static UIHandleBase m_healthGaugeNow;

        private static UIHandleBase m_heatGaugeNow;

        //recovery is simulated but damage isnt, up to me to save the day now
        private static UIHandleBase m_gaugeDamaged;

        private static bool m_isPinch = false;
        private static bool m_isDead = false;
        private static float lastPlayerHP;

        private static bool m_canHactFlag = false;

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
           
          //  else
             //   m_createdUIDoOnce = false;


            if (m_healthGauge.Handle > 0)
                Test();
#endif
        }

        private static void Create(UIEntityPlayerGauge gauge)
        {
            m_createdUIDoOnce = true;
            m_healthGauge = UI.Create(301, 1);
            //Reusing player health gauge for heat gauge for now...
            m_heatGauge = UI.Play(356, 1);
            m_canHact = UI.Create(324, 1);
            CreateHActPromptWorkaround();

            DragonEngine.Log("Health Gauge Handle: " + m_healthGauge.Handle);
            DragonEngine.Log("Heat Gauge Handle: " + m_heatGauge.Handle);

#if !DO_NOT_USE_NATIVE_IMP
            gauge.LifeGauge = m_healthGauge;
#else
            m_healthFrame = m_healthGauge.GetChild(0).GetChild(0).GetChild(0);
            m_healthGaugeNow = m_healthGauge.GetChild(0).GetChild(0).GetChild(1);
            //TEMPORARY: better than a delayed gauge.
            if (!IniSettings.PreferGreenUI)
                m_healthGaugeNow.GetChild(2).SetVisible(false);

            m_heatGauge.SetVisible(false);
            //   m_heatGauge.GetChild(0).GetChild(0).GetChild(0).SetWidth(200);


            m_heatGauge.GetChild(0).GetChild(0).SetWidth(200);
            m_heatGauge.GetChild(0).GetChild(0).GetChild(1);
            m_heatGaugeNow = m_heatGauge.GetChild(0).GetChild(0).GetChild(1);
            m_heatGaugeNow.GetChild(2).SetVisible(false);

            //CONVERT
            m_heatGauge.SetPosition(new Vector4(0, 17, 0, 0));
#endif
        }

        //Create Kiwami action UI (texture has been made so small it looks like a prompt)
        //Set to 2 seconds
        //Pause
        //Profit
        private static void CreateHActPromptWorkaround()
        {
            m_hactPrompt = UI.Play(995, 0);
            m_hactPrompt.SetVisible(false);

            m_hactPrompt.SetFrame(75);
            m_hactPrompt.Pause();
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
            HActPromptUpdate(HeatActionManager.CanHAct());

            if (!DragonEngine.GetHumanPlayer().IsValid() || !BrawlerBattleManager.Kasuga.IsValid())
            {
                m_healthGauge.SetVisible(false);
                m_heatGauge.SetVisible(false);
                m_canHact.SetVisible(false);
                return;
            }
            else
            {
                m_healthGauge.SetVisible(true);
                m_heatGauge.SetVisible(true);
                m_canHact.SetVisible(true);
            }

            int curHeat = BrawlerBattleManager.Kasuga.GetStatus().Heat;
            int maxHeat = Player.GetHeatMax(Player.ID.kasuga);

            bool canHeat = curHeat >= (maxHeat * 0.3f);

            if (!m_canHactFlag)
            {
                if (canHeat)
                {
                    m_canHactFlag = true;
                    m_canHact.PlayAnimationSet(6056);
                }
            }
            else
            {
                if (!canHeat)
                {
                    m_canHactFlag = false;
                    m_canHact.PlayAnimationSet(6057);
                }
            }

            long playerHP = Player.GetHPNow(Player.ID.kasuga);
            long playerMaxHP = Player.GetHPMax(Player.ID.kasuga);

            float gaugeWidth = CalcHealthGaugeWidth(400, 800, Player.GetHPMax(Player.ID.kasuga));

            m_healthGaugeNow.SetWidth(gaugeWidth);
            m_healthFrame.SetWidth(gaugeWidth);

            if (lastPlayerHP <= (int)(playerMaxHP * Mod.CriticalHPRatio))
            {
                if (BrawlerBattleManager.Kasuga.IsDead())
                {
                    if (!m_isDead)
                    {
                        m_healthGauge.PlayAnimationSet(0x241);
                        m_isDead = true;
                    }
                }
                else
                {
                    if (m_isDead)
                    {
                        m_healthGauge.PlayAnimationSet(0x23A);
                        m_isDead = false;
                    }
                }

                if (!m_isPinch)
                {
                    m_healthGauge.PlayAnimationSet(0x23E);
                    m_isPinch = true;
                }
            }
            else
            {
                if (m_isPinch)
                {
                    m_healthGauge.PlayAnimationSet(0x23F);
                    m_isPinch = false;
                }
            }

            if (playerHP != lastPlayerHP)
            {
                if (playerHP < lastPlayerHP)
                    m_healthGauge.PlayAnimationSet(576);
            }

            m_heatGaugeNow.SetValue((float)curHeat / (float)maxHeat);
            m_healthGaugeNow.SetValue((float)playerHP / (float)playerMaxHP);

            lastPlayerHP = playerHP;
#endif
        }

        public static void HealthGaugeUpdate()
        {

        }

        public static void HActPromptUpdate(bool canHact)
        {
            m_hactPrompt.SetVisible(canHact);
        }
    }
}
