using System;
using DragonEngineLibrary;

namespace Brawler
{
    public class MoveGMTOnly : MoveBase
    {
        public override MoveType MoveType => MoveType.MoveGMTOnly;

        public MotionID Motion;

        public MoveGMTOnly(MotionID motion, float attackDelay, MoveInput[] input, MoveSimpleConditions condition = MoveSimpleConditions.None) : base(attackDelay, input, condition)
        {
            Motion = motion;
        }

        public override void Execute(Fighter attacker, Fighter[] target)
        {
            attacker.Character.GetMotion().RequestGMT(Motion);
        }
    }
}
