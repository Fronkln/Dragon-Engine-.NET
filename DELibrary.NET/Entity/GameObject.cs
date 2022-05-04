using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public class GameObject : EntityBase
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CGAME_OBJECT_IS_VISIBLE", CallingConvention = CallingConvention.Cdecl)]
        [return:MarshalAs(UnmanagedType.U1)]
        internal static extern bool DELib_GameObject_IsVisible(IntPtr gameobject);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CGAME_OBJECT_SET_VISIBLE", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_GameObject_SetVisible(IntPtr gameobject, bool visible, bool temporarily);

        ///<summary>Is the entity visible?</summary>
        public bool IsVisible()
        {
            return DELib_GameObject_IsVisible(_objectAddress);
        }

        /// <summary>
        /// Set the visibility of the GameObject.
        /// </summary>
        public void SetVisible(bool visible, bool temporarily)
        {
            DELib_GameObject_SetVisible(_objectAddress, visible, temporarily);
        }

    }
}
