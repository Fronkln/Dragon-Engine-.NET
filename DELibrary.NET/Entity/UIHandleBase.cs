using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public class UIHandleBase
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_UI_HANDLE_CBASE_PLAY_ANIMATION_SET", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_UIHandleBase_PlayAnimationSet(IntPtr handlebase, uint id);

        internal IntPtr _ptr;

        ///<summary>Play animation set on UI.</summary>
        public void PlayAnimationSet(uint id)
        {
            DELib_UIHandleBase_PlayAnimationSet(_ptr, id);
        }
    }
}
