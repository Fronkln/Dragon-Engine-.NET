using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DragonEngineLibrary;

namespace Y7MP
{
    public enum CreatedEntityType
    {
        GeneratedFighterEnemy
    }

    //WorldState is only used for dynamically created entities at runtime.
    //MPHost class is responsible for updating this information
    public static class WorldState
    {
        public static Dictionary<ushort, SimpleNetworkedEntity> DynamicEntities = new Dictionary<ushort, SimpleNetworkedEntity>();
        public static List<CreationInfo> createdEntities = new List<CreationInfo>();

        public static ushort currentID = 0;

        public struct CreationInfo
        {
            public string typeName;
            public ushort ID;
            public object[] args;
        }


        /// <summary>
        /// Will create a simple networked entity. The entity has to have static a "Create" method.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void HostCreate<T>(params object[] args) where T : SimpleNetworkedEntity
        {
            HostCreate<T>((ushort)(currentID + 1), args);
        }

        /// <summary>
        /// Will create a simple networked entity. The entity has to have a static "Create" method.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void HostCreate<T>(ushort ID, params object[] args) where T : SimpleNetworkedEntity
        {
            if (!MPManager.Connected || !MPPlayer.LocalPlayer.IsMasterClient())
                return;

            currentID++;

            NetPacket packet = new NetPacket(false);

            packet.Writer.Write((byte)PacketMessage.SimpleNetworkEntityCreation);
            packet.Writer.Write(typeof(T).Name);
            packet.Writer.Write(ID);

            if (args != null)
                foreach (object obj in args)
                    packet.Writer.WriteObject(obj);

            MPManager.SendToEveryone(packet, Steamworks.EP2PSend.k_EP2PSendReliable);
        }


        public static void HostDelete(ushort index)
        {
            NetPacket packet = new NetPacket(false);
            packet.Writer.Write((byte)PacketMessage.SimpleNetworkEntityDestroy);
            packet.Writer.Write(index);

            MPManager.SendToEveryone(packet);
        }

        public static void HostDelete(SimpleNetworkedEntity entity)
        {
            System.Diagnostics.Debug.Assert(entity != null, "Host tried to dispose a null entity");
            System.Diagnostics.Debug.Assert(DynamicEntities.ContainsKey(entity.ID), "Host tried to remove an entity not on the list");

            HostDelete(entity.ID);
        }


        public static void HandleCreationRequest(Type entityType, ushort ID, NetPacket args)
        {
            bool creationFunctionExists = ReflectionCache.CreationMethodCache.ContainsKey(entityType);

            System.Diagnostics.Debug.Assert(creationFunctionExists, "Host tried to create an entity without a creation function.");

            try
            {
                SimpleNetworkedEntity ent = (SimpleNetworkedEntity)Activator.CreateInstance(entityType);
                ent.ID = ID;

                MethodInfo creationFunc = ReflectionCache.CreationMethodCache[entityType];
                object[] readArgs = args.Reader.ReadFunctionArguments(creationFunc);

                try
                {
                    creationFunc.Invoke(ent, readArgs);
                    DynamicEntities[ID] = ent;

                    DragonEngine.Log("Network entity spawned with ID: " + ID);

                    CreationInfo inf = new CreationInfo()
                    {
                        typeName = entityType.Name,
                        ID = ID,
                        args = readArgs
                    };
                }
                catch
                {
                    DragonEngine.Log("Failure instantiating entity");
                }
            }
            catch
            {
                DragonEngine.Log("Failed to create network entity");
            }
        }
    }
}
