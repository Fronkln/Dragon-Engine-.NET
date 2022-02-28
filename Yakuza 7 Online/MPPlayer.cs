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
        public const uint DefaultPvpEnemyID = 0x3D1C;
        public const float PLAYER_STATUS_UPDATE_RATE = 0.1f;
        public const float PLAYER_POS_LERP_RATE = 0.15f;
        public const float PLAYER_ROT_LERP_RATE = 6;
        public const float PLAYER_TELEPORT_POS_DIST = 10;

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

            public bool isPVP;
        };

        public CSteamID Owner;
        public EntityHandle<Character> Character;
        //ec_handle<cui_entity_component_enemy_life_gauge> NameDisplay;
        public MPPlayerInfo PlayerInfo;

        private float m_nextPositionUpdate = 0;

        private void UpdateNetworkPlayer()
        {
            Character playerEntity = DragonEngine.GetHumanPlayer();

            if (!playerEntity.IsValid())
                return;

            NetPacket coordUpdate = new NetPacket(false);
            NetPacket motionUpdate = new NetPacket(false);

            coordUpdate.Writer.Write((byte)PacketMessage.CharacterPositionUpdate);
            coordUpdate.Writer.Write((Vector3)playerEntity.Transform.Position);
            coordUpdate.Writer.Write(playerEntity.GetAngleY());

            ECMotion motion = playerEntity.GetMotion();

            motionUpdate.Writer.Write((byte)PacketMessage.CharacterAnimationUpdate);
            motionUpdate.Writer.WriteObject(motion.PlayInfo);
            motionUpdate.Writer.WriteObject(motion.BhvPartsInfo);

            MPManager.SendToEveryone(coordUpdate);
            MPManager.SendToEveryone(motionUpdate);
        }

        //These two functions are not local
        //isPVP should be synced across the network and other players will process this input

        public bool ShouldTransitionToNormal()
        {
            if (!Character.IsValid())
                return false;

            //PVP mode off but we are a soldier, change fast!
            if (Character.Get().Attributes.is_soldier)
                return true;

            //Pvp mode off, and we are normal, no need for transforming
            return false;
        }

        public bool ShouldTransitionToPvP()
        {
            if (!Character.IsValid())
                return false;

            //PVP mode on but not soldier, change fast!
            if (Character.Get().Attributes.enemy_id == BattleRPGEnemyID.invalid)
                return true;

            //Pvp mode on, and we are a soldier, no need for transforming
            return false;
        }

        public void Update()
        {
            Character chara = Character.Get();


            if(PlayerInfo.last_playermodel != CharacterID.invalid)
                if(chara.Attributes.chara_id != PlayerInfo.last_playermodel)
                {
#if DEBUG
                    DragonEngine.Log("Changing from " + chara.Attributes.chara_id + " to " + PlayerInfo.last_playermodel);
#endif
                    if (!IsLocalPlayer())
                        CreateCharAny();
                    else
                        CreateCharNormal();

                    return;
                }

            if (IsLocalPlayer())
            {
                if (MPManager.MPTime >= m_nextPositionUpdate)
                {
                    UpdateNetworkPlayer();
                    m_nextPositionUpdate += PLAYER_STATUS_UPDATE_RATE;
                }

                if (!HActManager.IsPlaying())
                    if (chara.IsVisible())
                        chara.SetVisible(false, false);
  
            }
            else
            {
                if (!PlayerInfo.isPVP)
                {
                    if(ShouldTransitionToNormal())
                        DragonEngine.Log("Transition to normal");
                }
                else
                {
                    if (ShouldTransitionToPvP())
                        DragonEngine.Log("Transition to PVP");
                }
            }

            float dist = Vector3.Distance((Vector3)chara.Transform.Position, PlayerInfo.last_position);

            if (dist < PLAYER_TELEPORT_POS_DIST)
            {
                chara.Transform.Position = Vector4.Lerp(chara.Transform.Position, PlayerInfo.last_position, PLAYER_POS_LERP_RATE);
                chara.SetAngleY(PlayerInfo.last_rot_y); //no lerp for rotation yet
            }
            else
            {
                chara.RequestWarpPose(new PoseInfo(PlayerInfo.last_position, PlayerInfo.last_rot_y));
            }
        }


        public void CreateCharAny()
        {
            if (!Character.IsValid())
            {
                if (PlayerInfo.isPVP)
                    CreateCharPvP();
                else
                    CreateCharNormal();
            }
            else
            {
                if (ShouldTransitionToNormal())
                    CreateCharNormal();
                else
                    CreateCharPvP();
            }
        }

        public void CreateCharNormal()
        {
            if (Character.IsValid())
                Character.Get().DestroyEntity();

            NPCRequestMaterial material = new NPCRequestMaterial();
            material.Material = new NPCMaterial();
            material.Material.pos_ = Vector4.zero;

            if (IsLocalPlayer())
                if (PlayerInfo.last_playermodel == CharacterID.invalid)
                    PlayerInfo.last_playermodel = DragonEngine.GetHumanPlayer().Attributes.chara_id;

            material.Material.character_id_ = PlayerInfo.last_playermodel;
            material.Material.is_eternal_life_ = true;
            material.Material.height_scale_id_ = CharacterHeightID.invalid;
            material.Material.is_minimum_mode_ = false;
            material.Material.is_force_create_ = true;
            material.Material.is_force_visible_ = true;
            material.Material.behavior_set_id_ = BehaviorSetID.m_human_npc_base;
            material.Material.voicer_id_ = CharacterVoicerID.invalid;
            material.Material.parent_ = SceneService.CurrentScene.Get().GetSceneEntity<EntityBase>(SceneEntity.character_manager).UID;
            material.Material.npc_setup_id_ = CharacterNPCSetup.no_collision_ever_fix;
            material.Material.map_icon_id_ = MapIconID.enemy;

            Character = NPCFactory.RequestCreate(material);

            if (IsLocalPlayer())
                Character.Get().SetVisible(false, false);

            //wake up the animation component
            Character.Get().GetMotion().RequestGMT(MotionID.test_dance);

            DragonEngine.Log("Created character for " + Owner.Name());
        }

        public void CreateCharPvP()
        {

            if (IsLocalPlayer())
            {
                DragonEngine.Log("CREATECHARPVP ATTEMPTED TO CALL ON LOCAL PLAYER! THIS SHOULD NOT HAPPEN!");
                return;
            }

            if (Character.IsValid())
                Character.Get().DestroyEntity();

            Character = FighterManager.GenerateEnemyFighter(new PoseInfo(PlayerInfo.last_position, PlayerInfo.last_rot_y), DefaultPvpEnemyID, PlayerInfo.last_playermodel);

            if (!Character.IsValid())
                DragonEngine.Log("Character invalid!");

            DragonEngine.Log("Created PVP character for " + Owner.Name());
        }

        public bool IsMasterClient()
        {
            if (!MPManager.Connected)
                return false;

            return SteamMatchmaking.GetLobbyOwner(MPManager.CurrentLobby) == Owner;
        }

        public bool IsLocalPlayer()
        {
            return Owner == SteamUser.GetSteamID();
        }
    }
}
