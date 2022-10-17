using System;

namespace Brawler
{
    [Flags]
    public enum HeatActionCondition : ulong
    {
        None = 1,

        EnemyCriticalHealth = 2,
        FighterCriticalHealth = 4,

        EnemyDown = 8,
        EnemyNotDown = 16,

        FighterDown = 32,

        EnemyStunned = 64,
        FighterStunned = 128,

        EnemyMidAir = 256,
        FighterMidAir = 1024,

        IsExHero = 2048,

        EnemyStandingUp = 4096,
        FighterStandingUp = 8192,

        FighterHealthNotCritical = 16384,
        EnemyHealthNotCritical = 32768,

        FighterGrabbed = 65536,
        EnemyGrabbed = 131072,

    }
}
