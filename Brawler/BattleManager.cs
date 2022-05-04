using System;
using DragonEngineLibrary;

namespace Brawler
{
    public static class BattleManager
    {
        public const float BattleStartTime = 2.8f;
        public static bool BattleStartDoOnce = false;
        public static float BattleTime = 0;

        public static bool AllowPlayerTransformationDoOnce = false;

        private static bool ShouldShowHeatAura()
        {
            if (BrawlerPlayer.IsEXGamer)
                return true;

            if (HActManager.IsPlaying())
                return false;

            if (!DragonEngine.GetHumanPlayer().GetFighter().IsValid() || !BattleStartDoOnce)
                return false;
            else
                return FighterManager.GetFighter(0).GetStatus().Heat == Player.GetHeatMax(Player.ID.kasuga);
        }

        public static void Update()
        {
            BrawlerPlayer.GameUpdate();

            if(!ShouldShowHeatAura())
                DragonEngine.GetHumanPlayer().Components.EffectEvent.Get().StopEvent(EffectEventCharaID.OgrefHeatAuraKr01, false);
            else
                DragonEngine.GetHumanPlayer().Components.EffectEvent.Get().PlayEvent(EffectEventCharaID.OgrefHeatAuraKr01);


            if (!DragonEngine.GetHumanPlayer().GetFighter().IsValid())
            {
                BattleStartDoOnce = false;
                BattleTime = 0;
                BrawlerPlayer.IsEXGamer = false;

                BrawlerPlayer.m_lastMove = null;
                BrawlerPlayer.m_attackCooldown = 0;

                return;
            }

            if (!BattleStartDoOnce)
            {
                if (BattleTime == 0)
                    FighterManager.GetFighter(0).Character.SetCommandSet(RPG.GetJobCommandSetID(Player.ID.kasuga, RPGJobID.kasuga_freeter));

                if (BattleTime > BattleStartTime)
                {      
                    BrawlerPlayer.ChangeStyle(null, true);
                    BattleStartDoOnce = true;
                }
            }

            MortalStateManager.Update();
            BattleTurnManager.RPGCamera.Get().Sleep();

            BattleTime += DragonEngine.deltaTime;
        }
    }
}
