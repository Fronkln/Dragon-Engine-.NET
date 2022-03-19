using System;
using System.Runtime.InteropServices;
using DragonEngineLibrary;
#if YLAD
namespace Y7MP
{
    public class MGHomelessCan : EntityBase
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CMG_HOMELESS_CAN_CREATE", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint DELibrary_MGHomelessCan_Create(uint parent);

        public static EntityHandle<MGHomelessCan> Create(EntityHandle<EntityBase> Parent)
        {
            return DELibrary_MGHomelessCan_Create(Parent.UID);
        }
    }
}
#endif