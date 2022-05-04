using System;
using DragonEngineLibrary;

namespace Brawler
{
    public class MoveGMTOnly : Move
    {
        public MotionID Motion;

        public MoveGMTOnly(MotionID motion, float attackDelay, MoveInput[] input, MoveSimpleConditions condition = MoveSimpleConditions.None) : base(RPGSkillID.invalid, attackDelay, input, condition)
        {
            Motion = motion;
        }

        public override void Execute(Fighter attacker, Fighter[] target)
        {
            attacker.Character.GetMotion().RequestGMT(Motion);
        }
    }
}
