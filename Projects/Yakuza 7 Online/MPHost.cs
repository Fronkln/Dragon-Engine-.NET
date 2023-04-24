using System;
using DragonEngineLibrary;

namespace Y7MP
{
    public class MPHost
    {
        public static void RaiseRPC(SimpleNetworkedEntity entity, RPCEvent eventID, params object[] arguments)
        {
            if (entity.ID == 0)
                return;

            bool entityValid = entity != null && entity.ID > 0;

            NetPacket packet = new NetPacket(false);
            packet.Writer.Write((byte)PacketMessage.SimpleNetworkEntityRPC);

            packet.Writer.Write((entityValid ? entity.ID : 0));
            packet.Writer.Write((ushort)eventID);

            foreach (object obj in arguments)
                packet.Writer.WriteObject(obj);

            MPManager.SendToEveryone(packet, Steamworks.EP2PSend.k_EP2PSendReliable);
        }

        //Only gets called on host
        public static void Update()
        {
        }
    }
}
