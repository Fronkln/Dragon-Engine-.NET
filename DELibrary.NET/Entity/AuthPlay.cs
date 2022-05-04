using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public class AuthPlay : AuthPlayBase
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CAUTH_PLAY_TEST", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_AuthPlay_Test(IntPtr authPlay);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CAUTH_PLAY_GETTER_TALKPARAMID", CallingConvention = CallingConvention.Cdecl)]
        internal static extern TalkParamID DELib_AuthPlay_Getter_TalkParamID(IntPtr authPlay);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CAUTH_PLAY_GETTER_REQUESTPAUSE", CallingConvention = CallingConvention.Cdecl)]
        [return:MarshalAs(UnmanagedType.U1)]
        internal static extern bool DELib_AuthPlay_Getter_RequestPause(IntPtr authPlay);
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CAUTH_PLAY_REQUESTPAUSE", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_AuthPlay_RequestPause(IntPtr authPlay, bool pause);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CAUTH_PLAY_RESET", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_AuthPlay_Restart(IntPtr authPlay);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CAUTH_PLAY_GET_GAME_TICK", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint DELib_AuthPlay_GetGameTick(IntPtr authPlay);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CAUTH_PLAY_GET_GAME_FRAME", CallingConvention = CallingConvention.Cdecl)]
        internal static extern float DELib_AuthPlay_GetGameFrame(IntPtr authPlay);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CAUTH_PLAY_SET_GAME_TICK", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_AuthPlay_SetGameTick(IntPtr authPlay, uint tick);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CAUTH_PLAY_SET_GAME_FRAME", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_AuthPlay_SetGameFrame(IntPtr authPlay, float frame);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CAUTH_PLAY_GET_END_TICK", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint DELib_AuthPlay_GetEndTick(IntPtr authPlay);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CAUTH_PLAY_GET_END_FRAME", CallingConvention = CallingConvention.Cdecl)]
        internal static extern float DELib_AuthPlay_GetEndFrame(IntPtr authPlay);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CAUTH_PLAY_SET_SPEED", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_AuthPlay_SetSpeed(IntPtr authPlay, short speed);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CAUTH_PLAY_GET_CUR_PAGE_INDEX", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int DELib_AuthPlay_GetCurrentPageIndex(IntPtr authPlay);

        public TalkParamID TalkParamID { get { return DELib_AuthPlay_Getter_TalkParamID(Pointer); } }

        ///<summary>Is pause requested for this auth. </summary>
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

        ///<summary>Convert Dragon Engine tick to frame.</summary>
        public static float TickToFrame(uint tick)
        {
            return (tick / 3000.0f) * 30.0f;
        }

        ///<summary>Convert frame to Dragon Engine tick </summary>
        public static uint FrameToTick(float frame)
        {
            return (uint)((frame * 3000.0f) / 30.0f);
        }

        ///<summary>Request pause for the auth.</summary>
        public void RequestPause(bool pause)
        {
            DELib_AuthPlay_RequestPause(_objectAddress, pause);
        }

        ///<summary>Restart the auth.</summary>
        public void Restart()
        {
            DELib_AuthPlay_Restart(_objectAddress);
        }

        ///<summary>Get the current game time of auth in frames.</summary>
        public float GetGameFrame()
        {
            return TickToFrame(GetGameTick());
        }

        ///<summary>Set the current game time of auth in frames.</summary>
        public void SetGameFrame(float frame)
        {
            SetGameTick(FrameToTick(frame));
        }

        ///<summary>Get the last frame of this auth.</summary>
        public float GetEndFrame()
        {
            return TickToFrame(GetEndTick());
        }

        ///<summary>Get the current game time of auth in ticks.</summary>
        public uint GetGameTick()
        {
            return DELib_AuthPlay_GetGameTick(_objectAddress);
        }

        ///<summary>Set the current game time of auth in ticks</summary>
        public void SetGameTick(uint tick)
        {
            DELib_AuthPlay_SetGameTick(_objectAddress, tick);
        }

        ///<summary>Get the last tick of this auth.</summary>
        public uint GetEndTick()
        {
            return DELib_AuthPlay_GetEndTick(_objectAddress);
        }

        ///<summary>Set the current speed of the auth.</summary>
        public void SetSpeed(short speed)
        {
            DELib_AuthPlay_SetSpeed(_objectAddress, speed);
        }

        ///<summary>Get the active page index of auth.</summary>
        public int GetCurrentPageIndex()
        {
            return DELib_AuthPlay_GetCurrentPageIndex(Pointer);
        }
    }
}
