using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public static class SoundManager
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CSOUND_MANAGER_PLAY_CUE", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_SoundManager_PlayCue(ushort cuesheetID, ushort cueID, int start_msec);


        public static void PlayCue(ushort cuesheetID, ushort cueID, int start_msec)
        {
            DELib_SoundManager_PlayCue(cuesheetID, cueID, 0);
        }
    }
}
