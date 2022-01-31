using System;
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

        public static bool SendPacket(SteamId target, MemoryStream data, P2PSend mode)
        {
            if (!Connected)
                return false;

            bool status = SteamNetworking.SendP2PPacket(target, data.GetBuffer(), (int)data.Length, 0, mode);

            if (!status)
                DragonEngine.Log("Failed to send packet to " + target);

            return status;
        }

        public static bool SendToServer(MemoryStream data, P2PSend mode)
        {
            if (!Connected)
                return false;

            return SendPacket(CurrentLobby.Owner.Id, data, mode);
        }

        public static bool SendToEveryone(MemoryStream data, P2PSend mode)
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
        }
    }
}
