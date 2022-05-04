using System;
using System.Timers;
using System.Linq;
using System.Collections.Generic;
using DragonEngineLibrary;


namespace Brawler
{
    [Flags]
    public enum MoveSimpleConditions
    {
        None = 0x0,
        FighterIsNotDown = 0x1,
        FighterIsDown = 0x2,
        FighterHPCritical = 0x3
    }


    public class WeaponMove : Move
    {
        public WeaponMove(RPGSkillID attack, float attackDelay, MoveInput[] input, MoveSimpleConditions condition, ItemID weapon) : base(attack, attackDelay, input, condition)
        {

        }
    }

    public class Moveset
    {
        public List<MoveBase> Moves = new List<MoveBase>();

        public Moveset(params MoveBase[] moves)
        {
            Moves = moves.ToList();
        }

        public T GetMoveOfType<T>() where T : MoveBase, new()
        {
            foreach (MoveBase move in Moves)
                if (move as T != null)
                    return (T)move;

            return null;
        }
    }
}
