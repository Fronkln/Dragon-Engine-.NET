using System;
using System.Threading;
using DragonEngineLibrary;

namespace Brawler
{
    public class Mod : DragonEngineMod
    {
        private bool m_fightStartedDoOnce = false;

        public void InputThread()
        {
            while (true)
            {

                if (DragonEngine.IsKeyDown(VirtualKey.Numpad7))
                {
                    bool toggle = !FighterManager.IsBrawlerMode();
                    FighterManager.ForceBrawlerMode(toggle);

                    DragonEngine.Log("Brawler Mode: " + toggle);
                }


                Fighter kasuga = FighterManager.GetPlayer();

                if (!kasuga.Character.IsValid())
                    continue;

                Fighter[] enemies = FighterManager.GetAllEnemies();

                if (enemies.Length <= 0)
                    continue;

                if (DragonEngine.IsKeyDown(VirtualKey.Numpad5))
                {
                    FighterManager.GetPlayer().Character.GetConstructor().StartAgent();
                }

                if (DragonEngine.IsKeyDown(VirtualKey.Numpad6))
                {
                    FighterManager.GetPlayer().Character.GetConstructor().StopAgent();
                }

                if (DragonEngine.IsKeyDown(VirtualKey.F))
                    BattleTurnManager.ForceCounterCommand(kasuga, enemies[0], RPGSkillID.boss_kiryu_atk_c);
                else
                    if (DragonEngine.IsKeyDown(VirtualKey.G))
                    BattleTurnManager.ForceCounterCommand(kasuga, enemies[0], RPGSkillID.boss_kiryu_atk_a);

                if (DragonEngine.IsKeyDown(VirtualKey.H))
                    BattleTurnManager.ForceCounterCommand(kasuga, enemies[0], RPGSkillID.boss_kiryu_crash_atk_a);
            }
        }

        public override void OnModInit()
        {
            DragonEngine.Initialize();
            DragonEngine.RegisterJob(Update, DEJob.Update);

            FighterManager.ForceBrawlerMode(true);

            Thread thread = new Thread(InputThread);
            thread.Start();
        }

        public void Update()
        {
            if (!FighterManager.IsBrawlerMode())
                return;

            Fighter kasuga = FighterManager.GetPlayer();

            if (kasuga.Character.IsValid())
            {
                if (!m_fightStartedDoOnce)
                {
                    m_fightStartedDoOnce = true;
                    OnFightStart();
                }

                BrawlerUpdate();
            }
            else
                m_fightStartedDoOnce = false;
        }

        public void BrawlerUpdate()
        {
           // BattleTurnManager.ReleaseMenu();
        }

        public static void OnFightStart()
        {
            DragonEngine.Log("Brawler fight start");
        }
    }
}
