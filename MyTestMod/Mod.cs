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
                if (DragonEngine.IsKeyDown(VirtualKey.F))
                {
                    DragonEngine.Log(FighterManager.GetFighter(0).Character.Attributes.player_id);
                    DragonEngine.Log(FighterManager.GetFighter(1).Character.Attributes.player_id);
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
