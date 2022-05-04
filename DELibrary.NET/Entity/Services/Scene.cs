using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public class Scene : SceneBase
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CSCENE_GETTER_CONFIG_ID", CallingConvention = CallingConvention.Cdecl)]
        internal static extern SceneConfigID DELibrary_Scene_Getter_ConfigID(IntPtr scene);

        public SceneConfigID ConfigID { get { return DELibrary_Scene_Getter_ConfigID(Pointer); } }
    }
}
