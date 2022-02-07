using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using DragonEngineLibrary;
using Steamworks;
using Steamworks.Data;

namespace Y7MP
{
    public static class MPManager
    {
        public const int MaxPlayers = 16;

        public static bool Connected = false;
        public static Lobby CurrentLobby;

        public static Dictionary<ulong, MPPlayer> playerList = new Dictionary<ulong, MPPlayer>();

        public static float MPTime = 0;

        public static bool SendPacket(SteamId target, NetPacket data, P2PSend mode = P2PSend.Unreliable)
        {
            if (!Connected)
                return false;

            bool status = SteamNetworking.SendP2PPacket(target, data.Stream.GetBuffer(), (int)data.Stream.Length, 0, mode);

            if (!status)
                DragonEngine.Log("Failed to send packet to " + target);

            return status;
        }

        public static bool SendToServer(NetPacket data, P2PSend mode = P2PSend.Unreliable)
        {
            if (!Connected)
                return false;

            return SendPacket(CurrentLobby.Owner.Id, data, mode);
        }

        public static bool SendToEveryone(NetPacket data, P2PSend mode = P2PSend.Unreliable)
        {
            if (!Connected)
                return false;

            int playerCount = CurrentLobby.MemberCount;

            for (int i = 0; i < playerCount; i++)
                SendPacket(CurrentLobby.Members.ElementAt(i).Id, data, mode);

            return true;
        }

        public static void OnLobbyCreated(Result res, Lobby lobbyInfo)
        {
            if (res != Result.OK)
            {
                DragonEngine.Log("Lobby creation failed!");
                return;
            }

            DragonEngine.Log("Lobby created. ID: " + lobbyInfo.Id);

            lobbyInfo.SetPublic();
        }

        /// <summary>
        /// Called locally, others will recieve a "lobby chat message" instead
        /// </summary>
        public static void OnLobbyEnter(Lobby lobbyInfo)
        {
            Connected = true;
            CurrentLobby = lobbyInfo;
            MPTime = 0;

            DragonEngine.Log("Lobby entered.");
        }

        public static void OnInvitedToLobby(Friend inviter, Lobby lobby)
        {
            DragonEngine.Log($"Got invited by {inviter.Name}. Attempting join");
            SteamMatchmaking.JoinLobbyAsync(inviter.Id);
        }

        public static void OnP2PRequest(SteamId requester)
        {
            DragonEngine.Log("Accepting P2P request with " + requester);
            SteamNetworking.AcceptP2PSessionWithUser(requester);
        }

        public static void LobbyBugTest(Lobby lobby, Friend friend)
        {
            CurrentLobby = lobby;
        }

        public static MPPlayer CreatePlayer(Friend id)
        {
            MPPlayer player = new MPPlayer();
            player.Owner = id;
            playerList.Add(id.Id.Value, player);

            DragonEngine.Log("Created MPPlayer object for " + id.Name);

            return player;
        }

        public static void HandleP2PMessage(PacketMessage type, SteamId sender, NetPacket packet)
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
            while (SteamNetworking.IsP2PPacketAvailable(0))
            {
                P2Packet packet = SteamNetworking.ReadP2PPacket().Value;

                if (packet.Data.Length <= 0)
                    continue;

                NetPacket netPacket = new NetPacket(packet.Data);
                HandleP2PMessage((PacketMessage)netPacket.Reader.ReadByte(), packet.SteamId, netPacket);
            }
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
            for (int i = 0; i < CurrentLobby.MemberCount; i++)
            {
                Friend lobbyPlr = CurrentLobby.Members.ElementAt(i);

                if (!playerList.ContainsKey(lobbyPlr.Id))
                    CreatePlayer(lobbyPlr);
                else
                {
                    //Character does not exist/got deleted
                    //Let's create it
                    if (!playerList[lobbyPlr.Id].Character.IsValid())
                        playerList[lobbyPlr.Id].CreateChar();
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
