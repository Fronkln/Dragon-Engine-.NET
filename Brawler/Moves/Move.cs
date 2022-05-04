using System;
using DragonEngineLibrary;

namespace Brawler
{
    public class MoveBase
    {
        public MoveInput[] inputKeys;
        public MoveSimpleConditions skillConditions;
        public float cooldown = 0;

        public MoveBase(float attackDelay, MoveInput[] input, MoveSimpleConditions condition = MoveSimpleConditions.None)
        {
            cooldown = attackDelay;
            inputKeys = input;
            skillConditions = condition;
        }

        /// <summary>
        /// We are no longer the active attack
        /// </summary>
        public virtual void OnMoveEnd() { }

        public virtual bool MoveExecuting()
        {
            return BrawlerPlayer.m_attackCooldown > 0;
        }

        public virtual bool AllowChange()
        {
            return BrawlerPlayer.m_attackCooldown <= 0;
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
                {
                    DragonEngine.Log("we down");
                    return false;
                }

            if (skillConditions.HasFlag(MoveSimpleConditions.FighterHPCritical))
                if ((battleStatus.MaxHP * 0.4f) < battleStatus.CurrentHP)
                    return false;

            if (skillConditions.HasFlag(MoveSimpleConditions.FighterIsDown))
                if (!fighter.IsDown())
                    return false;

            return true;
        }

        public virtual bool CheckConditions(Fighter fighter, Fighter[] targets)
        {
            return CheckSimpleConditions(fighter);
        }

        public virtual bool CanUse(Fighter fighter)
        {
            if (!fighter.Character.IsValid())
                return false;

            return CheckSimpleConditions(fighter);
        }

        public virtual void Execute(Fighter attacker, Fighter[] target) { }
        public virtual void InputUpdate() { }
        public virtual void Update() { }
    }

    public class Move : MoveBase
    {
        public RPGSkillID ID;



        public Move(RPGSkillID attack, float attackDelay, MoveInput[] input, MoveSimpleConditions condition = MoveSimpleConditions.None) : base(attackDelay, input, condition)
        {
            ID = attack;
        }

        public override void Execute(Fighter attacker, Fighter[] target)
        {
            BattleTurnManager.ForceCounterCommand(attacker, target[0], ID);
        }
    }
}
