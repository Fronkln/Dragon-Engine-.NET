using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public class AuthPlay : AuthPlayBase
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CAUTH_PLAY_TEST", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_AuthPlay_Test(IntPtr authPlay);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CAUTH_PLAY_GETTER_REQUESTPAUSE", CallingConvention = CallingConvention.Cdecl)]
        [return:MarshalAs(UnmanagedType.U1)]
        internal static extern bool DELib_AuthPlay_Getter_RequestPause(IntPtr authPlay);
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CAUTH_PLAY_REQUESTPAUSE", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_AuthPlay_RequestPause(IntPtr authPlay, bool pause);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CAUTH_PLAY_RESET", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_AuthPlay_Restart(IntPtr authPlay);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CAUTH_PLAY_GET_GAME_TICK", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint DELib_AuthPlay_GetGameTick(IntPtr authPlay);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CAUTH_PLAY_GET_END_TICK", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint DELib_AuthPlay_GetEndTick(IntPtr authPlay);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CAUTH_PLAY_SET_SPEED", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_AuthPlay_SetSpeed(IntPtr authPlay, short speed);

        public bool PauseRequested
        {
            get
            {
                return DELib_AuthPlay_Getter_RequestPause(_objectAddress);
            }
            set
            {
                RequestPause(value);
            }
        }

        public void Test()
        {
            DELib_AuthPlay_Test(_objectAddress);
        }

        public void RequestPause(bool pause)
        {
            DELib_AuthPlay_RequestPause(_objectAddress, pause);
        }

        public void Restart()
        {
            DELib_AuthPlay_Restart(_objectAddress);
        }

        public uint GetGameTick()
        {
            return DELib_AuthPlay_GetGameTick(_objectAddress);
        }

        public uint GetEndTick()
        {
            return DELib_AuthPlay_GetEndTick(_objectAddress);
        }

        public void SetSpeed(short speed)
        {
            DELib_AuthPlay_SetSpeed(_objectAddress, speed);
        }
    }
}
