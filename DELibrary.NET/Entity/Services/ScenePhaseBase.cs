using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public class ScenePhaseBase : Scene
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CSCENE_PHASE_BASE_ALTER_ACT_SWITCH_SCENE_ID", CallingConvention = CallingConvention.Cdecl)]
        [return:MarshalAs(UnmanagedType.U1)]
        internal static extern bool DELibrary_ScenePhaseBase_AlterActSwitchSceneID(IntPtr scene, SceneConfigID config, uint posID, Vector4 pos);

        public bool AlterActSwitchSceneID(SceneConfigID config, uint posID, Vector4 pos)
        {
            return DELibrary_ScenePhaseBase_AlterActSwitchSceneID(Pointer, config, posID, pos);
        }
    }
}
