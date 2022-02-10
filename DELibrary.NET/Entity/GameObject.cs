using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public class GameObject : EntityBase
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CGAME_OBJECT_SET_VISIBLE", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_GameObject_SetVisible(IntPtr gameobject, bool visible, bool temporarily);

        /// <summary>
        /// Set the visibility of the GameObject.
        /// </summary>
        public void SetVisible(bool visible, bool temporarily)
        {
            DELib_GameObject_SetVisible(_objectAddress, visible, temporarily);
        }

    }
}
