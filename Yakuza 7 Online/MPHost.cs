using System;
using DragonEngineLibrary;

namespace Y7MP
{
    public class MPHost
    {
        public static void RaiseRPC(SimpleNetworkedEntity entity, RPCEvent eventID, params object[] arguments)
        {
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
           if(DragonEngine.IsKeyHeld(VirtualKey.H))
            {
                WorldState.HostCreate<GeneratedEnemy>(DragonEngine.GetHumanPlayer().Transform.Position, 31.005f); 
              
                
                // RaiseRPC(null, RPCEvent.Create_FighterManagerGeneratedEnemy, (ushort)31, new Vector3(90.5f, 100, 255), 1337);
            }
        }
    }
}
