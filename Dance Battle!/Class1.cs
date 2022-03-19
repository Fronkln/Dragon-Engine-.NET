using System;
using System.Threading;
using DragonEngineLibrary;

namespace DanceBattle
{
    
    public class Mod : DragonEngineMod
    {
        public static void InputThread()
        {
            while(true)
            {
                if(DragonEngine.IsKeyHeld(VirtualKey.Shift))
                {
                    if(DragonEngine.IsKeyDown(VirtualKey.G))
                    {
                        DragonEngine.Log("Press");

                        HActRequestOptions opts = new HActRequestOptions();
                        opts.id = (TalkParamID)12879; // (TalkParamID)12879;
                        opts.can_skip = false;
                        opts.Register(HActReplaceID.hu_player1, DragonEngine.GetHumanPlayer().UID);
                        opts.is_force_play = true;
                        opts.base_mtx.matrix = DragonEngine.GetHumanPlayer().GetPosture().GetRootMatrix();

                        HActManager.RequestHAct(opts);
                       
                    }
                }
            }
        }

        public override void OnModInit()
        {
            DragonEngine.Initialize();

            Thread inputThread = new Thread(InputThread);
            inputThread.Start();
        }
    }
}
