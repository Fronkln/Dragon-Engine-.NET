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

    public class MoveSidestep : MoveGMTOnly
    {
        public MoveSidestep(float attackDelay, MoveInput[] input, MoveSimpleConditions conditions = MoveSimpleConditions.FighterIsNotDown) : base(attackDelay, input)
        {
            skillConditions = conditions;
        }

        public override void Execute(Fighter attacker, Fighter target)
        {
            attacker.Character.HumanModeManager.ToSway();

          //  if(DragonEngine.IsKeyHeld(VirtualKey.W))
             //   attacker.Character.GetMotion().RequestGMT((MotionID)12);
           // else
              //  attacker.Character.GetMotion().RequestGMT((MotionID)11);
        }
    }

    //Hacky but thats the whole brawler mod for you
    public class MoveGetUp : MoveGMTOnly
    {
        public MoveGetUp(float attackDelay, MoveInput[] input, MoveSimpleConditions condition = MoveSimpleConditions.None)
        {
            cooldown = attackDelay;
            inputKeys = input;
            skillConditions = condition;
            Motion = MotionID.C_COM_BTL_SUD_dwnB_to_dge_r;
        }

        public override void Execute(Fighter attacker, Fighter target)
        {
            attacker.Character.HumanModeManager.ToStand(HumanModeManager.RequireType.Normal);
            attacker.Character.GetMotion().RequestGMT(Motion);
        }
    }

    public class MoveGMTOnly : Move
    {
        public MotionID Motion;

        public MoveGMTOnly() { }

        public MoveGMTOnly(MotionID motion, float attackDelay, MoveInput[] input, MoveSimpleConditions condition = MoveSimpleConditions.None)
        {
            cooldown = attackDelay;
            skillConditions = condition;
            inputKeys = input;
            Motion = motion;
        }

        public MoveGMTOnly(float attackDelay, MoveInput[] input)
        {
            inputKeys = input;
            cooldown = attackDelay;
        }


        public override void Execute(Fighter attacker, Fighter target)
        {
            attacker.Character.GetMotion().RequestGMT(Motion);
        }
    }

    public class Move
    {
        public RPGSkillID ID;
        public MoveInput[] inputKeys;
        public MoveSimpleConditions skillConditions;
        public float cooldown = 0;

        public Move() { }

        public Move(RPGSkillID attack, float attackDelay, MoveInput[] input, MoveSimpleConditions condition = MoveSimpleConditions.None)
        {
            ID = attack;
            inputKeys = input;
            skillConditions = condition;
            cooldown = attackDelay;
        }

        public virtual void Execute(Fighter attacker, Fighter target)
        {
            BattleTurnManager.ForceCounterCommand(attacker, target, ID);
        }

        public bool AreInputKeysPressed()
        {
            if (inputKeys == null || inputKeys.Length <= 0)
                return false;

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

            if (skillConditions.HasFlag(MoveSimpleConditions.FighterIsNotDown))
                if (fighter.IsDown())
                    return false;

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

        public T GetMoveOfType<T>() where T : Move, new()
        {
            foreach (Move move in Moves)
                if (move as T != null)
                    return (T)move;

            return null;
        }
    }
}
