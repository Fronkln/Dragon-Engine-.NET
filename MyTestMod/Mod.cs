using System;
using System.Threading;
using DragonEngineLibrary;

namespace MyTestMod
{
    public class Mod : DragonEngineMod
    {

        static void InputThread()
        {
            while (true)
            {
                if (DragonEngine.IsKeyDown(VirtualKey.G))
                {
                    //based on party order
                    Fighter ichi = FighterManager.GetFighter(0);
                    Fighter saeko = FighterManager.GetFighter(2);

                    ichi.Character.SetPosCenter(saeko.Character.GetPosCenter());
                    DragonEngine.Log("Counter result" + BattleTurnManager.ForceCounterCommand(ichi, saeko, RPGSkillID.boss_kiryu_legend_skl_counter_act));
                }
            }
        }

        public override void OnModInit()
        {
            DragonEngine.Log("Hello World!");
            DragonEngine.Initialize();

            Thread thread = new Thread(InputThread);
            thread.Start();
        }
    }
}
