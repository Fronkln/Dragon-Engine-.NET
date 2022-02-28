using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public class EntityComponent : CTask
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_ENTITY_COMPONENT_GETTER_OWNER", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr DELib_Entity_Component_Getter_Owner(IntPtr component);

        public virtual EntityBase Owner
        {
            get
            {
                EntityBase ent = new EntityBase();
                ent._objectAddress = DELib_Entity_Component_Getter_Owner(_objectAddress);

                return ent;
            }
        }
    }
}
