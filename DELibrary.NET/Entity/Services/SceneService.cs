using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary.Service
{
    public static class SceneService
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CSCENE_SERVICE_GET_SCENE_INFO", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr DELib_SceneService_GetSceneInfo();

        public static EntityHandle<SceneBase> CurrentScene
        {
            get
            {
                return GetSceneInfo().ScenePlay;
            }
        }

        public static SceneHolder GetSceneInfo()
        {
            IntPtr result = DELib_SceneService_GetSceneInfo();

            if (result == IntPtr.Zero)
                return new SceneHolder();

            Internal.SceneHolderMarshal marshal = Marshal.PtrToStructure<Internal.SceneHolderMarshal>(result);

            SceneHolder resultHolder = new SceneHolder();
            resultHolder.ScenePlay = marshal.ScenePlay.ToHandle<SceneBase>();
            resultHolder.SceneChange = marshal.SceneChange.ToHandle<SceneBase>();

            return resultHolder;
        }
    }
}
