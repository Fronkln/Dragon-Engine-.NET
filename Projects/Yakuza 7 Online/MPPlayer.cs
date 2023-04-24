using System;
using Steamworks;
using DragonEngineLibrary;
using DragonEngineLibrary.Service;

namespace Y7MP
{
    public class MPPlayer
    {
        public static MPPlayer LocalPlayer;

        public static bool DebugLocalGhostVisible = false;

        public const CharacterID DefaultPlayerModel = CharacterID.m_ichiban_23; //Normal ichiban
        public const uint DefaultPvpEnemyID = 0x3D1E;

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
            public long playerHealth;
            public long playerMaxHealth;

            public uint level;

            public bool isPVP;
        };

        public CSteamID Owner;
        public EntityHandle<Character> Character;
        public UIEntityComponentEnemyLifeGauge LifeGauge;
        public MPPlayerInfo PlayerInfo;


        public bool IsFakePlayer = false;

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

            SendPlayerInfo();

            MPManager.SendToEveryone(coordUpdate);
            MPManager.SendToEveryone(motionUpdate);
        }



        public void SendPlayerInfo()
        {
            if (!IsLocalPlayer() && !IsFakePlayer)
                return;

            NetPacket packet = new NetPacket(false);

            if (!IsFakePlayer)
            {
                packet.Writer.Write((byte)PacketMessage.PlayerFullInfoUpdate);
                packet.Writer.Write((uint)LocalPlayer.PlayerInfo.last_playermodel);
                packet.Writer.Write(Player.GetHPNow(Player.ID.kasuga));
                packet.Writer.Write(Player.GetHPMax(Player.ID.kasuga));
                packet.Writer.Write(Player.GetLevel(Player.ID.kasuga));

                MPManager.SendToEveryone(packet, EP2PSend.k_EP2PSendReliable);
            }
        }

        public void Update()
        {
            Character chara = Character.Get();
            Character localPlayer = DragonEngine.GetHumanPlayer();
            ECBattleStatus status = chara.GetBattleStatus();

            status.CurrentHP = PlayerInfo.playerHealth;
            status.MaxHP = PlayerInfo.playerMaxHealth;
            status.Level = PlayerInfo.level;
            status.DefensePower = 9999999;


           if(!IsLocalPlayer())
            {
                if (ShouldTransitionToPvP())
                    CreateCharPvP();
                else
                    if (ShouldTransitionToNormal())
                    CreateCharNormal();
            }


            if (!PlayerInfo.isPVP)
            {
                if (PlayerInfo.last_playermodel != CharacterID.invalid && chara.Attributes.chara_id != PlayerInfo.last_playermodel)
                    CreateCharNormal();
            }
            else
            {
                if(Character.IsValid())
                {
                    LifeGauge = new EntityComponentHandle<UIEntityComponentEnemyLifeGauge>(Character.Get().EntityComponentMap.GetComponent(ECSlotID.ui_enemy_life_gauge).UID);
                    LifeGauge.SetCategoryName(Owner.Name() + $"({PlayerInfo.playerHealth}/{PlayerInfo.playerMaxHealth})");
                }
              //  if(LifeGauge.IsValid())
            }

            if (IsLocalPlayer())
            {
                PlayerInfo.last_playermodel = localPlayer.Attributes.chara_id;

                if (MPManager.MPTime >= m_nextPositionUpdate)
                {
                    UpdateNetworkPlayer();
                    m_nextPositionUpdate += PLAYER_STATUS_UPDATE_RATE;
                }


                if (!DebugLocalGhostVisible)
                {
                    if (!HActManager.IsPlaying())
                        if (chara.IsVisible())
                            chara.SetVisible(false, false);
                }
                else
                    chara.SetVisible(true, false);

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
            bool hadModelBefore = Character.IsValid();
            Character.Get().DestroyEntity();

            if (PlayerInfo.last_playermodel == CharacterID.invalid)
            {
                DragonEngine.Log("Player had no ID or was invalid, setting to default: CreateCharAny()");
                PlayerInfo.last_playermodel = DefaultPlayerModel;
            }

            //character never existed before
            if(!Character.IsValid() || !hadModelBefore)
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
                else if(ShouldTransitionToPvP())
                    CreateCharPvP();
            }

        }

        public bool ShouldTransitionToNormal()
        {
            if (!Character.IsValid())
                return false;

            if (!PlayerInfo.isPVP && Character.Get().Attributes.enemy_id != BattleRPGEnemyID.invalid)
                return true;

            return false;
        }

        public bool ShouldTransitionToPvP()
        {
            if (!Character.IsValid())
                return false;

            return PlayerInfo.isPVP && Character.Get().Attributes.enemy_id == BattleRPGEnemyID.invalid;
        }

        public void CreateCharNormal()
        {


            if (Character.IsValid())
            {
                DragonEngine.Log("Changing from " + Character.Get().Attributes.chara_id + " to: " + PlayerInfo.last_playermodel);
                Character.Get().DestroyEntity();
            }

            NPCRequestMaterial material = new NPCRequestMaterial();
            material.Material = new NPCMaterial();
            material.Material.pos_ = Vector4.zero;

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

            if (!Character.IsValid())
                DragonEngine.Log("CREATED CHARACTER IS NOT VALID! THE HOOK HAS FAILED! ABANDON ALL HOPE!");

            if (IsLocalPlayer())
                Character.Get().SetVisible(false, false);

            LifeGauge = UIEntityComponentEnemyLifeGauge.Attach(Character);
            LifeGauge.SetCategoryName(Owner.Name());

            ECBattleStatus.Attach(Character);

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
