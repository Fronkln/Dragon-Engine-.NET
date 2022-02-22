using System;
using System.Linq;
using System.Collections.Generic;
using DragonEngineLibrary;


namespace Brawler
{
    [Flags]
    public enum MoveSimpleConditions
    {
        None = 0x0,
        FighterIsDown = 0x1,
        FighterHPCritical = 0x2
    }


    public class WeaponMove : Move
    {
        public WeaponMove(RPGSkillID attack, float attackDelay, MoveInput[] input, MoveSimpleConditions condition, ItemID weapon) : base(attack, attackDelay, input, condition)
        {

        }
    }

    public struct MoveInput
    {
        public VirtualKey Key;
        public bool Hold;

        public MoveInput(VirtualKey key, bool hold)
        {
            Key = key;
            Hold = hold;
        }
    }

    public class Move
    {
        public RPGSkillID ID;
        public MoveInput[] inputKeys;
        public MoveSimpleConditions skillConditions;
        public float cooldown = 0;

        public Move(RPGSkillID attack, float attackDelay, MoveInput[] input, MoveSimpleConditions condition = MoveSimpleConditions.None)
        {
            ID = attack;
            inputKeys = input;
            skillConditions = condition;
            cooldown = attackDelay;
        }

        public bool AreInputKeysPressed()
        {
            foreach (MoveInput input in inputKeys)
            {
                if (input.Hold)
                    if (!DragonEngine.IsKeyHeld(input.Key))
                        return false;
                if (!input.Hold)
                    if (!DragonEngine.IsKeyDown(input.Key))
                        return false;
            }

            return true;
        }

        public bool CheckSimpleConditions(Fighter fighter)
        {
            if (!fighter.Character.IsValid())
                return false;

            if (skillConditions == MoveSimpleConditions.None)
                return true;

            ECBattleStatus battleStatus = fighter.Character.GetBattleStatus();

            if (skillConditions.HasFlag(MoveSimpleConditions.FighterHPCritical))
                if ((battleStatus.MaxHP * 0.4f) < battleStatus.CurrentHP)
                    return false;

            if (skillConditions.HasFlag(MoveSimpleConditions.FighterIsDown))
                if (!fighter.IsDown())
                    return false;

            return true;
        }

        public virtual bool CanUse(Fighter fighter)
        {
            if (!fighter.Character.IsValid())
                return false;

            return CheckSimpleConditions(fighter);
        }
    }

    public class Moveset
    {
        public List<Move> Moves = new List<Move>();

        public Moveset(params Move[] moves)
        {
            Moves = moves.ToList();
        }
    }
}
