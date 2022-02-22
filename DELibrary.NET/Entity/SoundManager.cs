using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public static class SoundManager
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CSOUND_MANAGER_IS_CUESHEET_LOADED", CallingConvention = CallingConvention.Cdecl)]
        [return:MarshalAs(UnmanagedType.U1)]
        internal static extern bool DELib_SoundManager_IsCuesheetLoaded(SoundCuesheetID cuesheetID);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CSOUND_MANAGER_LOAD_CUESHEET", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_SoundManager_LoadCuesheet(ushort cuesheetID);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CSOUND_MANAGER_PLAY_CUE", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_SoundManager_PlayCue(ushort cuesheetID, ushort cueID, int start_msec);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CSOUND_MANAGER_PLAY_CUE2", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_SoundManager_PlayCue(uint num, int start_msec);

        /// <summary>
        /// Is it me or does this always return true?
        /// </summary>
        public static bool IsCuesheetLoaded(SoundCuesheetID cuesheetID)
        {
            return DELib_SoundManager_IsCuesheetLoaded(cuesheetID);
        }

        /// <summary>
        /// Load a cuesheet so the sounds of it can be played.
        /// </summary>
        public static void LoadCuesheet(SoundCuesheetID cuesheetID)
        {
            DELib_SoundManager_LoadCuesheet((ushort)cuesheetID);
        }


        /// <summary>
        /// Play the sound id from specified cuesheet. The cuesheet has to be loaded.
        /// </summary>
        public static void PlayCue(ushort cuesheetID, ushort cueID, int start_msec)
        {
            DELib_SoundManager_PlayCue(cuesheetID, cueID, start_msec);
        }

        /// <summary>
        /// Play the sound id from specified cuesheet. The cuesheet has to be loaded.
        /// </summary>
        public static void PlayCue(SoundCuesheetID cuesheetID, ushort cueID, int start_msec)
        {
            PlayCue((ushort)cuesheetID, cueID, 0);
        }
    }
}
