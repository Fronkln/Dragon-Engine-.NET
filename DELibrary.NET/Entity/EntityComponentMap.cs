using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public class EntityComponentMap
    {
        public IntPtr Pointer { get; internal set; }

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_ENTITY_COMPONENT_MAP_GET_COMPONENT", CallingConvention = CallingConvention.Cdecl)]
        private static extern uint DELib_EntityComponentMap_GetComponent(IntPtr componentmap, ECSlotID slot);

        ///<summary>Gets the specified entity component if it exists.</summary>
        public EntityHandle<EntityComponent> GetComponent(ECSlotID slot)
        {
            return DELib_EntityComponentMap_GetComponent(Pointer, slot);
        }
    }
}
