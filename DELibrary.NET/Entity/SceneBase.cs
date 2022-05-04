using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public class SceneBase : EntityBase
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CSCENE_BASE_GETTER_SCENE", CallingConvention = CallingConvention.Cdecl)]
        internal static extern SceneID DELibrary_SceneBase_Getter_Scene(IntPtr scenebase);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CSCENE_BASE_GETTER_STAGE", CallingConvention = CallingConvention.Cdecl)]
        internal static extern StageID DELibrary_SceneBase_Getter_Stage(IntPtr scenebase);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CSCENE_BASE_GET_SCENE_ENTITY", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint DELibrary_SceneBase_GetSceneEntity(IntPtr scenebase, SceneEntity sceneEnt);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CSCENE_BASE_SET_SCENE_ENTITY", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELibrary_SceneBase_SetSceneEntity(IntPtr scenebase, SceneEntity sceneEnt, uint handle);


        public override EntityHandle<EntityBase> GetSceneEntity(SceneEntity sceneEnt)
        {
            return DELibrary_SceneBase_GetSceneEntity(_objectAddress, sceneEnt);
        }

        public void SetSceneEntity(SceneEntity sceneEnt, EntityHandle<EntityBase> handle)
        {
            DELibrary_SceneBase_SetSceneEntity(_objectAddress, sceneEnt, handle.UID);
        }

        ///<summary>Scene ID of this scene.</summary>
        public SceneID SceneID { get { return DELibrary_SceneBase_Getter_Scene(_objectAddress); } }
        ///<summary>Stage ID of this scene.</summary>
        public StageID StageID { get { return DELibrary_SceneBase_Getter_Stage(_objectAddress); } }
    }
}
