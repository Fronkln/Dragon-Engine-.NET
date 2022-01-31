using System;
using Steamworks;
using DragonEngineLibrary;

namespace Y7MP
{
    public class MPPlayer
    {
        public static MPPlayer LocalPlayer;

        public const CharacterID DefaultPlayerModel = (CharacterID)15287; //Normal ichiban
        public const float PLAYER_STATUS_UPDATE_RATE = 0.1f;
        public const float PLAYER_POS_LERP_RATE = 10f;
        public const float PLAYER_ROT_LERP_RATE = 6;

        public struct MPPlayerInfo
        {
            public Vector4 last_position;
            public Quaternion last_orient;
            public float last_rot_y;
            public CharacterID last_playermodel;
            //public stage::e_id last_stage;
            public uint last_stage;

            //updated less often
            public int playerHealth;
            public int playerMaxHealth;

            public int level;
        };

        public SteamId Owner;
        public EntityHandle<Character> Character;
        //ec_handle<cui_entity_component_enemy_life_gauge> NameDisplay;
        public MPPlayerInfo PlayerInfo;

        private float m_nextPositionUpdate;


        public bool IsMasterClient()
        {
            if (!MPManager.Connected)
                return false;

            return MPManager.CurrentLobby.IsOwnedBy(Owner);
        }

        bool IsLocalPlayer()
        {
            return Owner == SteamClient.SteamId;
        }
    }
}
