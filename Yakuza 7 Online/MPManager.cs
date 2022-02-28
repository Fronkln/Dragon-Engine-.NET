using System;
using System.Timers;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using DragonEngineLibrary;
using Steamworks;

namespace Y7MP
{
    public static class MPManager
    {
        public const int MaxPlayers = 16;

        public static bool Connected = false;
        public static CSteamID CurrentLobby;

        public static Dictionary<CSteamID, MPPlayer> playerList = new Dictionary<CSteamID, MPPlayer>();

        public static float MPTime = 0;

        public static bool SendPacket(CSteamID target, NetPacket data, EP2PSend mode = EP2PSend.k_EP2PSendUnreliable)
        {
            if (!Connected)
                return false;

            bool status = SteamNetworking.SendP2PPacket(target, data.Stream.GetBuffer(), (uint)data.Stream.Length, mode, 0);

            if (!status)
                DragonEngine.Log("Failed to send packet to " + target);

            return status;
        }

        public static bool SendToServer(NetPacket data, EP2PSend mode = EP2PSend.k_EP2PSendUnreliable)
        {
            if (!Connected)
                return false;

            return SendPacket(SteamMatchmaking.GetLobbyOwner(CurrentLobby), data, mode);
        }

        public static bool SendToEveryone(NetPacket data, EP2PSend mode = EP2PSend.k_EP2PSendUnreliable)
        {
            if (!Connected)
                return false;

            int playerCount = SteamMatchmaking.GetNumLobbyMembers(CurrentLobby);

            for (int i = 0; i < playerCount; i++)
                SendPacket(SteamMatchmaking.GetLobbyMemberByIndex(CurrentLobby, i), data, mode);

            return true;
        }

        public static void OnLobbyCreated(LobbyCreated_t lobby)
        {
            if (lobby.m_eResult != EResult.k_EResultOK)
            {
                DragonEngine.Log("Lobby creation failed!");
                return;
            }

            DragonEngine.Log("Lobby created. ID: " + lobby.m_ulSteamIDLobby);
        }

        /// <summary>
        /// Called locally, others will recieve a "lobby chat message" instead
        /// </summary>
        public static void OnLobbyEnter(LobbyEnter_t entrance)
        {
            Connected = true;
            CurrentLobby = (CSteamID)entrance.m_ulSteamIDLobby;
            MPTime = 0;

            BattleTurnManager.OverrideAttackerSelection(MPBattle.HandleAttackerSelection);

            DragonEngine.Log("Lobby entered.");
        }

        public static void OnInvitedToLobby(LobbyInvite_t invite)
        {
            DragonEngine.Log($"Got invited by {invite.m_ulSteamIDUser}. Attempting join");
            SteamMatchmaking.JoinLobby((CSteamID)invite.m_ulSteamIDLobby);
        }

        public static void OnP2PRequest(P2PSessionRequest_t request)
        {
            DragonEngine.Log("Accepting P2P request with " + request.m_steamIDRemote.Name());
            SteamNetworking.AcceptP2PSessionWithUser(request.m_steamIDRemote);
        }

        public static void OnLobbyChatUpdate(LobbyChatUpdate_t update)
        {
            CSteamID id = new CSteamID(update.m_ulSteamIDMakingChange);

            if (update.m_rgfChatMemberStateChange == 1)
            {
                CreatePlayer(id);

                Timer timer = new Timer()
                {
                    AutoReset = false,
                    Enabled = true,
                    Interval = 1000
                };

                timer.Elapsed += delegate
                {
                    NetPacket packet = new NetPacket(false);
                    packet.Writer.Write((byte)PacketMessage.PlayerFullInfoUpdate);
                    packet.Writer.Write((uint)MPPlayer.LocalPlayer.PlayerInfo.last_playermodel);

                    SendPacket(id, packet, EP2PSend.k_EP2PSendReliable);
                    DragonEngine.Log("Sent our player information to the newcomer");
                };
            }
            else if (update.m_ulSteamIDMakingChange == 2 || update.m_ulSteamIDMakingChange == 4)
            {
                playerList[id].Character.Get().DestroyEntity();
                playerList.Remove(id);
            }

        }

        public static MPPlayer CreatePlayer(CSteamID id)
        {
            MPPlayer player = new MPPlayer();
            player.Owner = id;
            playerList.Add(id, player);

            if (player.IsLocalPlayer())
                MPPlayer.LocalPlayer = player;

            DragonEngine.Log("Created MPPlayer object for " + id.Name());

            return player;
        }

        public static MPPlayer CreateFakePlayer()
        {
            Random rnd = new Random();

            CharacterID[] charas = new CharacterID[]
            {
                CharacterID.n_akiyama,
                CharacterID.m_kiryu,
                CharacterID.m_ichiban_23,
                CharacterID.m_adachi_mou,
                CharacterID.w_saeko_haruka
            };


            CSteamID fakeID = new CSteamID((ulong)rnd.Next(1000000000, int.MaxValue));
            MPPlayer fakePlayer = CreatePlayer(fakeID);

            fakePlayer.PlayerInfo.last_playermodel = charas[rnd.Next(0, charas.Length)];
            fakePlayer.PlayerInfo.last_position = (Vector3)MPPlayer.LocalPlayer.Character.Get().Transform.Position + MPPlayer.LocalPlayer.Character.Get().Transform.forwardDirection * 2;
            fakePlayer.PlayerInfo.last_rot_y = MPPlayer.LocalPlayer.Character.Get().GetAngleY();

            DragonEngine.Log("Created fake MPPlayer");

            return fakePlayer;
        }


        public static void HandleP2PMessage(PacketMessage type, CSteamID sender, NetPacket packet)
        {
            //Ignore any packets sent by this player until MPManager creates their player object.
            if (!playerList.ContainsKey(sender))
                return;

            MPPlayer senderPlayer = playerList[sender];
            Character senderChara = senderPlayer.Character.Get();

            switch (type)
            {
                case PacketMessage.CharacterPositionUpdate:
                    if (senderChara.IsValid())
                    {
                        senderPlayer.PlayerInfo.last_position = packet.Reader.ReadVector3();
                        senderPlayer.PlayerInfo.last_rot_y = packet.Reader.ReadSingle();
                    }
                    break;

                case PacketMessage.CharacterAnimationUpdate:
                    if (senderChara.IsValid())
                    {
                        ECMotion charaMot = senderChara.GetMotion();

                        MotionPlayInfo playInf = charaMot.PlayInfo;
                        MotionPlayInfo bhvInf = charaMot.BhvPartsInfo;

                        //necessary evil to prevent crash
                        //ignore the invalid handle that got automatically sent by NetPacketWriter
                        uint handle1 = playInf.bep_handle_;
                        uint handle2 = bhvInf.bep_handle_;

                        playInf = (packet.Reader.ReadObject<MotionPlayInfo>());
                        bhvInf = (packet.Reader.ReadObject<MotionPlayInfo>());

                        playInf.bep_handle_ = handle1;
                        bhvInf.bep_handle_ = handle2;

                        charaMot.PlayInfo = playInf;
                        charaMot.BhvPartsInfo = bhvInf;
                    }
                    break;

                case PacketMessage.CharacterPlayGMT:
                    if (senderChara.IsValid())
                    {
                        uint gmt = packet.Reader.ReadUInt32();
                        senderChara.GetMotion().RequestGMT(gmt);
                    }
                    break;

                case PacketMessage.PlayerOnPlayerHAct:
                    CSteamID attacker = new CSteamID(packet.Reader.ReadUInt64());
                    CSteamID victim = new CSteamID(packet.Reader.ReadUInt64());
                    TalkParamID hact = (TalkParamID)packet.Reader.ReadUInt32();
                    bool next = packet.Reader.ReadBoolean();

                    if (!playerList.ContainsKey(attacker) || !playerList.ContainsKey(victim))
                        break;

                    HActRequestOptions hactOpt = new HActRequestOptions();
                    hactOpt.id = hact;
                    hactOpt.is_force_play = (!next ? true : false);
                    hactOpt.can_skip = false;

                    hactOpt.Register(HActReplaceID.hu_player1, playerList[attacker].Character.UID);
                    hactOpt.Register(HActReplaceID.hu_enemy_00, playerList[victim].Character.UID);

                    if (!next)
                        HActManager.RequestHAct(hactOpt);
                    else
                        HActManager.RequestNextHAct(hactOpt);

                    break;

                case PacketMessage.PlayerFullInfoUpdate:
                    senderPlayer.PlayerInfo.last_playermodel = (CharacterID)packet.Reader.ReadUInt32();
                    break;
                case PacketMessage.PlayerChatMessage:
                    string text = packet.Reader.ReadString();
                   // string text = System.Text.Encoding.UTF8.GetString(packet.Stream.ToArray(), (int)packet.Reader.BaseStream.Position, packet.Reader.ReadInt32());
                    MPChat.AddMessage($"{senderPlayer.Owner.Name()}: {text}");
                    break;
            }

            //We are done with it
            packet.Reader.Dispose();
        }

        public static void ReadNetworkData()
        {
            uint packetSize;

            while (SteamNetworking.IsP2PPacketAvailable(out packetSize, 0))
            {
                byte[] buffer = new byte[packetSize];
                uint readBytes = 0;
                CSteamID sender;

                if (SteamNetworking.ReadP2PPacket(buffer, packetSize, out readBytes, out sender))
                {
                    NetPacket netPacket = new NetPacket(buffer);
                    HandleP2PMessage((PacketMessage)netPacket.Reader.ReadByte(), sender, netPacket);
                }
            }
        }

        public static void ClearPlayers()
        {
            foreach (var kv in playerList)
                kv.Value.Character.Get().DestroyEntity();

            playerList.Clear();
        }

        public static void Leave()
        {
            SteamMatchmaking.LeaveLobby(CurrentLobby);
            Connected = false;

            ClearPlayers();
        }

        public static void Update()
        {
            Character player = DragonEngine.GetHumanPlayer();

            //Player does not exist at the moment
            if (!player.IsValid())
                return;

            if (!Connected)
                return;

            MPTime += DragonEngine.deltaTime;

            //Check if any lobby members are uncreated
            for (int i = 0; i < SteamMatchmaking.GetNumLobbyMembers(CurrentLobby); i++)
            {
                CSteamID lobbyPlr = SteamMatchmaking.GetLobbyMemberByIndex(CurrentLobby, i);

                if (!playerList.ContainsKey(lobbyPlr))
                    CreatePlayer(lobbyPlr);
            }

            foreach (var kv in playerList)
                if (!kv.Value.Character.IsValid())
                    kv.Value.CreateCharAny();

            ReadNetworkData();


            foreach (var kv in playerList)
            {
                MPPlayer plr = kv.Value;

                if (plr.Character.IsValid())
                    plr.Update();
            }

        }
    }
}
