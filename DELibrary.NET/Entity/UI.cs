using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public class UI
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_UI_CREATE", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr DELib_UI_Create(uint sceneID, uint targetID);


        ///<summary>Create UI.</summary>
        public static UIHandleBase Create(uint sceneID, uint targetID)
        {
            return new UIHandleBase() { _ptr = DELib_UI_Create(sceneID, targetID) };
        }
    }
}
