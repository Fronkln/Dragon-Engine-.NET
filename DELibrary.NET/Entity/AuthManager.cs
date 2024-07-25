using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public unsafe static class AuthManager
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CAUTH_PLAY_GETTER_PLAYING_SCENE", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint DELib_AuthPlay_Getter_PlayingScene();

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CAUTH_MANAGER_GETTER_POINTER", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr DELib_AuthPlay_Getter_Pointer();

        public static IntPtr Pointer { get { return DELib_AuthPlay_Getter_Pointer(); } }

        ///<summary>Get the current auth that is playing.</summary>
        public static EntityHandle<AuthPlay> PlayingScene
        {
            get
            {
                return DELib_AuthPlay_Getter_PlayingScene();
            }
        }
    }
}
