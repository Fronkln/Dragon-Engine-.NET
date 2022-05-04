using System;
using System.Timers;
using DragonEngineLibrary;

namespace Brawler
{

    public class MoveSidestep : MoveGMTOnly
    {
        public MoveSidestep(float attackDelay, MoveInput[] input, MoveSimpleConditions conditions = MoveSimpleConditions.FighterIsNotDown) : base(MotionID.invalid, attackDelay, input, conditions)
        {

        }

        public override void Execute(Fighter attacker, Fighter[] target)
        {
            attacker.Character.HumanModeManager.ToSway();


            Timer timer = new Timer()
            {
                AutoReset = false,
                Enabled = true,
                Interval = 500
            };

            if (BrawlerPlayer.attackCancelTimer != null)
                BrawlerPlayer.attackCancelTimer.Enabled = false;

            timer.Elapsed += delegate { attacker.Character.HumanModeManager.ToEndReady(); };
            BrawlerPlayer.attackCancelTimer = timer;

            //     attacker.Character.HumanModeManager.ToEndReady();

            //  if(DragonEngine.IsKeyHeld(VirtualKey.W))
            //   attacker.Character.GetMotion().RequestGMT((MotionID)12);
            // else
            //  attacker.Character.GetMotion().RequestGMT((MotionID)11);
        }
    }
}
