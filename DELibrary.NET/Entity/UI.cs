using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public class UI
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_UI_CREATE", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong DELib_UI_Create(uint sceneID, uint targetID);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_UI_GET", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong DELib_UI_Get(uint sceneID);


        ///<summary>Create UI.</summary>
        public static UIHandleBase Create(uint sceneID, uint targetID)
        {
            return new UIHandleBase() { Handle = DELib_UI_Create(sceneID, targetID) };
        }

        public static UIHandleBase Get(uint sceneID)
        {
            return new UIHandleBase() { Handle = DELib_UI_Get(sceneID) };
        }


        /// <summary>
        /// Loads the UI. Does not create it.
        /// </summary>
        /// <param name="sceneID"></param>
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_UI_SCENE_LOAD_BY_REF", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Load(uint sceneID);

        /// <summary>
        /// Unloads the UI.
        /// </summary>
        /// <param name="sceneID"></param>
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_UI_SCENE_UNLOAD_BY_REF", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Unload(uint sceneID);
    }
}
