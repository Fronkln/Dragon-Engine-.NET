using System;
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

        public static MPPlayer CreatePlayer(CSteamID id)
        {
            MPPlayer player = new MPPlayer();
            player.Owner = id;
            playerList.Add(id, player);

            DragonEngine.Log("Created MPPlayer object for " + id.Name());

            return player;
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
                    if(senderChara.IsValid())
                    {
                        senderPlayer.PlayerInfo.last_position = packet.Reader.ReadVector3(); 
                        senderPlayer.PlayerInfo.last_rot_y = packet.Reader.ReadSingle();
                    }
                    break;


                case PacketMessage.CharacterPlayGMT:
                    if (senderChara.IsValid())
                    {
                        uint gmt = packet.Reader.ReadUInt32();
                        senderChara.GetMotion().RequestGMT(gmt);
                    }

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
            foreach(var kv in playerList)
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
            Character player = DragonEngine.GetHumanPlayer().Get();

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
                else
                {
                    //Character does not exist/got deleted
                    //Let's create it
                    if (!playerList[lobbyPlr].Character.IsValid())
                        playerList[lobbyPlr].CreateChar();
                }

            }

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
