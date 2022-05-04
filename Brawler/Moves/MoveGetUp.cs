using System;
using DragonEngineLibrary;

namespace Brawler
{
    //Hacky but thats the whole brawler mod for you
    public class MoveGetUp : MoveGMTOnly
    {
        public MoveGetUp(float attackDelay, MoveInput[] input, MoveSimpleConditions condition = MoveSimpleConditions.None) : base(MotionID.C_COM_BTL_SUD_dwnB_to_dge_r, attackDelay, input, condition)
        {
        }

        public override void Execute(Fighter attacker, Fighter[] target)
        {
            attacker.Character.HumanModeManager.ToStand(HumanModeManager.RequireType.Normal);
            attacker.Character.GetMotion().RequestGMT(Motion);
        }
    }

}
