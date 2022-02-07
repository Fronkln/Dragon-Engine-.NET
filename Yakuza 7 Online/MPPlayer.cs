using System;
using Steamworks;
using DragonEngineLibrary;
using DragonEngineLibrary.Service;

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
            public Vector3 last_position;
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

        public Friend Owner;
        public EntityHandle<Character> Character;
        //ec_handle<cui_entity_component_enemy_life_gauge> NameDisplay;
        public MPPlayerInfo PlayerInfo;

        private float m_nextPositionUpdate = 0;

        private void UpdateNetworkPlayer()
        {
            Character playerEntity = DragonEngine.GetHumanPlayer().Get();

            if (!playerEntity.IsValid())
                return;

            NetPacket coordUpdate = new NetPacket(false);

            coordUpdate.Writer.Write((byte)PacketMessage.CharacterPositionUpdate);
            coordUpdate.Writer.Write((Vector3)playerEntity.PosCenter);
            coordUpdate.Writer.Write(playerEntity.GetAngleY());

            MPManager.SendToEveryone(coordUpdate);
        }

        public void Update()
        {
            Character chara = Character.Get();

            if (IsLocalPlayer())
                if (MPManager.MPTime >= m_nextPositionUpdate)
                {
                    UpdateNetworkPlayer();
                    m_nextPositionUpdate += PLAYER_STATUS_UPDATE_RATE;
                }

            chara.PosCenter = Vector4.Lerp(chara.PosCenter, PlayerInfo.last_position, PLAYER_POS_LERP_RATE * DragonEngine.deltaTime);
            chara.SetAngleY(PlayerInfo.last_rot_y); //no lerp for rotation yet

            // chara.SetPosCenter(DragonEngine.GetHumanPlayer().Get().GetPosCenter());
            //  Character.Get().SetAngleY(DragonEngine.GetHumanPlayer().Get().GetAngleY());
        }

        public void CreateChar()
        {
            if (Character.IsValid())
                Character.Get().DestroyEntity();

            NPCRequestMaterial material = new NPCRequestMaterial();
            material.Material = new NPCMaterial();
            material.Material.pos_ = DragonEngine.GetHumanPlayer().Get().GetPosCenter();
            material.Material.character_id_ = CharacterID.w_sonhi;
            material.Material.collision_type_ = 0;
            material.Material.is_eternal_life_ = true;
            material.Material.height_scale_id_ = CharacterHeightID.height_185;
            material.Material.is_encounter_ = true;
            material.Material.is_encount_btl_type_ = true;
            material.Material.enemy_id_ = BattleRPGEnemyID.yazawa_boss_nanba;
            material.Material.is_force_create_ = true;
            material.Material.is_force_visible_ = true;
            material.Material.behavior_set_id_ = BehaviorSetID.encounter;
            material.Material.voicer_id_ = CharacterVoicerID.kiryu;
            material.Material.parent_ = SceneService.CurrentScene.Get().GetSceneEntity<EntityBase>(SceneEntity.character_manager).UID;
            material.Material.npc_setup_id_ = CharacterNPCSetup.no_collision_ever_fix;
            material.Material.map_icon_id_ = MapIconID.enemy;

            Character = NPCFactory.RequestCreate(material);

            DragonEngine.Log("Created character for " + Owner.Name);
        }

        public bool IsMasterClient()
        {
            if (!MPManager.Connected)
                return false;

            return MPManager.CurrentLobby.IsOwnedBy(Owner.Id);
        }

        bool IsLocalPlayer()
        {
            return Owner.Id == SteamClient.SteamId;
        }
    }
}
