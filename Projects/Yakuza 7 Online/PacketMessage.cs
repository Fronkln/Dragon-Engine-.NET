using System;


namespace Y7MP
{
    public enum PacketMessage : byte
    {
        CharacterPositionUpdate,
        CharacterPlayGMT,
        CharacterAnimationUpdate,

        FighterManagerGenerateEnemy,

        PlayerChatMessage,
        PlayerOnPlayerHAct,
        PlayerFullInfoUpdate,
        PlayerCombatUpdate,

        TURNBASED_ForceCounterCommand,

        SimpleNetworkEntityRPC,
        SimpleNetworkEntityCreation,
        SimpleNetworkEntityDestroy,

        TEST_EveryoneBecomesHostile,
        TEST_EveryoneBecomesFriendly
    }
}
