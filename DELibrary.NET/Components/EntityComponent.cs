using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public class EntityComponent : CTask
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_ENTITY_COMPONENT_GETTER_OWNER", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr DELib_Entity_Component_Getter_Owner(IntPtr component);


        [DllImport("Y7Internal.dll", EntryPoint = "LIB_ENTITY_COMPONENT_RELEASE_ENTITY_COMPONENT", CallingConvention = CallingConvention.Cdecl)]
        [return:MarshalAs(UnmanagedType.U1)]
        private static extern bool DELib_Entity_Component_ReleaseEntityComponent(IntPtr component, bool b_remove_parent_handle, bool b_immediate);

        public virtual EntityBase Owner
        {
            get
            {
                EntityBase ent = new EntityBase();
                ent._objectAddress = DELib_Entity_Component_Getter_Owner(_objectAddress);

                return ent;
            }
        }

        public bool DestroyComponent(bool b_remove_parent_handle, bool b_immediate)
        {
            return DELib_Entity_Component_ReleaseEntityComponent(Pointer, b_remove_parent_handle, b_immediate);
        }
    }
}
