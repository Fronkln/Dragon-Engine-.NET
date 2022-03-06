using System;
using System.Collections.Generic;
using System.Reflection;
using DragonEngineLibrary;

namespace Y7MP
{
    /// <summary>
    /// All simple network entities are controlled by the host
    /// </summary>
    public class SimpleNetworkedEntity
    {
        public Dictionary<Type, MethodInfo> CachedCreationMethods = new Dictionary<Type, MethodInfo>();

        public ushort ID = 0;
        public EntityHandle<EntityBase> Entity;

        public Vector3 SyncedPosition;
        public Quaternion SyncedRotation;

        public StageID world;


        public virtual void Destroy()
        {
            WorldState.DynamicEntities.Remove(ID);

            if (Entity.IsValid())
                Entity.Get().DestroyEntity();
        }

        public virtual bool ShouldBeRemoved()
        {
            return false;
        }

        public virtual void ClientUpdate()
        {

        }

        public virtual void HostUpdate()
        {

        }
    }
}
