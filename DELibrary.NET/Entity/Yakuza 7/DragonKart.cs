using System;
using System.Runtime.InteropServices;

#if YLAD
namespace DragonEngineLibrary
{
    public class DragonKart : EntityBase
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CDRAGON_CART_CREATE", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint DELibrary_DragonKart_Create(uint parent);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CDRAGON_CART_INITIALIZE", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint DELibrary_DragonKart_Initialize(IntPtr kart, MinigameDragonKartDriverID driver, MinigameDragonKartCourseID course);

        public static EntityHandle<DragonKart> Create(EntityHandle<EntityBase> Parent)
        {
            return DELibrary_DragonKart_Create(Parent.UID);
        }

        public void Initialize(MinigameDragonKartDriverID driver, MinigameDragonKartCourseID course)
        {
            DELibrary_DragonKart_Initialize(Pointer, driver, course);
        }
    }
}
#endif