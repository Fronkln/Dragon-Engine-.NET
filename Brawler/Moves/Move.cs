using System;
using DragonEngineLibrary;

namespace Brawler
{
    public enum MoveType
    {
        MoveBase,
        MoveRPG,
        MoveGMTOnly,
        MoveHeatAction,
        MoveComboString,
        MoveSidestep
    }

    public class MoveBase
    {
        public virtual MoveType MoveType => MoveType.MoveBase;

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
                {
                    if (!Mod.Input[input.Key].Held)
                        return false;
                }
                else
                    if (Mod.Input[input.Key].LastTimeSincePressed > 0.1f)
                        return false;
            }

            return true;
        }

        public bool CheckSimpleConditions(Fighter fighter, Fighter[] targets)
        {
            if (!fighter.Character.IsValid())
                return false;

            if (skillConditions == MoveSimpleConditions.None)
                return true;

            ECBattleStatus battleStatus = fighter.Character.GetBattleStatus();
            BrawlerFighterInfo info = BrawlerFighterInfo.Infos[fighter.Character.UID];

            if (skillConditions.HasFlag(MoveSimpleConditions.FighterIsNotDown))
                if (info.IsDown)
                    return false;

            if (skillConditions.HasFlag(MoveSimpleConditions.FighterHPCritical))
                if ((battleStatus.MaxHP * 0.4f) < battleStatus.CurrentHP)
                    return false;

            if (skillConditions.HasFlag(MoveSimpleConditions.FighterIsDown))
                if (!info.IsDown || info.IsGettingUp)
                    return false;

            if(skillConditions.HasFlag(MoveSimpleConditions.LockedEnemyIsDown))
            {
                Fighter lockedEnemy = BrawlerPlayer.GetLockOnTarget(fighter, targets);

                if (!lockedEnemy.IsValid())
                    return false;
                else
                {
                    BrawlerFighterInfo enemyInfo = BrawlerFighterInfo.Infos[lockedEnemy.Character.UID];

                    if (!enemyInfo.IsDown || enemyInfo.IsGettingUp)
                        return false;
                }
            }

            return true;
        }

        public virtual bool CheckConditions(Fighter fighter, Fighter[] targets)
        {
            return CheckSimpleConditions(fighter, targets);
        }

        public virtual void Execute(Fighter attacker, Fighter[] target) { }
        public virtual void InputUpdate() { }
        public virtual void Update() { }
    }

    public class MoveRPG : MoveBase
    {
        public override MoveType MoveType => MoveType.MoveRPG;

        public RPGSkillID ID;
        private bool m_isCooldown = false;
        private float m_cooldownDur = 1;

        public MoveRPG(RPGSkillID attack, float attackDuration, MoveInput[] input, MoveSimpleConditions condition = MoveSimpleConditions.None, float cooldown = 1) : base(attackDuration, input, condition)
        {
            ID = attack;
            m_cooldownDur = cooldown;
        }


        public override void Execute(Fighter attacker, Fighter[] target)
        {
            BattleTurnManager.ForceCounterCommand(attacker, target[0], ID);
            BrawlerPlayer.m_attackCooldown = cooldown;

            m_isCooldown = true;
            new SimpleTimer(m_cooldownDur, delegate { m_isCooldown = false; });
        }

        public override bool CheckConditions(Fighter fighter, Fighter[] targets)
        {
            if (m_isCooldown)
                return false;

            return base.CheckConditions(fighter, targets);
        }
    }
}
