using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public class SceneBase : EntityBase
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CSCENE_BASE_GETTER_STAGE", CallingConvention = CallingConvention.Cdecl)]
        internal static extern StageID DELibrary_SceneBase_Getter_Stage(IntPtr scenebase);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CSCENE_BASE_GET_SCENE_ENTITY", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint DELibrary_CSCENE_BASE_GET_SCENE_ENTITY(IntPtr scenebase, SceneEntity sceneEnt);

        public override EntityHandle<EntityBase> GetSceneEntity(SceneEntity sceneEnt)
        {
            return DELibrary_CSCENE_BASE_GET_SCENE_ENTITY(_objectAddress, sceneEnt);
        }

        public StageID StageID
        {
            get
            {
                return DELibrary_SceneBase_Getter_Stage(_objectAddress);
            }
        }
    }
}
