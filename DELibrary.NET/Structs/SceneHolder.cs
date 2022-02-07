using System;
using System.Runtime.InteropServices;


namespace DragonEngineLibrary
{

    namespace Internal
    {
        //CPP Marshal version before the struct gets converted into a more user friendly one
        [StructLayout(LayoutKind.Sequential)]
        internal struct SceneHolderMarshal
        {
            private long vfptr;

            ///<summary>EntityHandle SceneBase</summary>
            public DEHandleMarshal ScenePlay;
            public DEHandleMarshal SceneChange;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SceneHolder
    {
        public EntityHandle<SceneBase> ScenePlay;
        public EntityHandle<SceneBase> SceneChange;
    }
}
