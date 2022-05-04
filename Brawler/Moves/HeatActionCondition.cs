using System;

namespace Brawler
{
    [Flags]
    public enum HeatActionCondition
    {
        None = 1,

        EnemyCriticalHealth = 2,
        FighterCriticalHealth = 4,

        EnemyDown = 8,
        FighterDown = 16,

        EnemyStunned = 32,
        FighterStunned = 64,

        EnemyMidAir = 128,
        FighterMidAir = 256

    }
}
