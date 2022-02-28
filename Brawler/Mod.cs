using System;
using System.Runtime.InteropServices;
using System.Threading;
using DragonEngineLibrary;

namespace Brawler
{
    public class Mod : DragonEngineMod
    {
        //0000000140085CF0 basemode change on debug build find the same place on retail and name it
        //used for getting up from attack after X interval
        [DllImport("Y7Internal.dll", EntryPoint = "VFUNC_TEST", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void TEST_FUNC();

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

                if (FighterManager.IsBrawlerMode())
                    BrawlerPlayer.InputUpdate();
            }
        }


        public override void OnModInit()
        {
            DragonEngine.Initialize();

            BrawlerPlayer.Init();
            DragonEngine.RegisterJob(BrawlerPlayer.GameUpdate, DEJob.Update);

            BattleTurnManager.OverrideAttackerSelection(OnAttackerSelect);
            FighterManager.ForceBrawlerMode(true);

            Thread thread = new Thread(InputThread);
            thread.Start();
        }

        public static Fighter OnAttackerSelect(bool readOnly, bool getNextFighter)
        {
            if (!FighterManager.IsBrawlerMode())
                return null;

            Fighter[] allEnemies = FighterManager.GetAllEnemies();

            if (allEnemies.Length <= 0)
                return null;

            Random rnd = new Random();

            return allEnemies[rnd.Next(0, allEnemies.Length)];
        }

    }
}
