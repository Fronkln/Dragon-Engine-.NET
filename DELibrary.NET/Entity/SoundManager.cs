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

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CSOUND_MANAGER_UNLOAD_CUESHEET", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_SoundManager_UnloadCuesheet(ushort cuesheetID);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CSOUND_MANAGER_PLAY_CUE", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_SoundManager_PlayCue(ushort cuesheetID, ushort cueID, int start_msec);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CSOUND_MANAGER_PLAY_CUE2", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_SoundManager_PlayCue(uint num, int start_msec);


        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CSOUND_MANAGER_PLAY_BGM", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_SoundManager_PlayBGM(ushort cuesheetID, ushort cueID, uint start_msec);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CSOUND_MANAGER_PLAY_BGM2", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_SoundManager_PlayBGM2(uint id, uint start_msec);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CSOUND_MANAGER_GET_BGM_SEID", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint DELib_SoundManager_GetBGMSeID(uint bgmSlot);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CSOUND_MANAGER_GET_BGM_PLAYTIME_SEC", CallingConvention = CallingConvention.Cdecl)]
        internal static extern float DELib_SoundManager_GetBGMPlaytimeSec(uint bgmSlot);

        /// <summary>
        /// Is it me or does this always return true?
        /// </summary>
        [DECompatibility(DEGames.YLAD)]
        public static bool IsCuesheetLoaded(SoundCuesheetID cuesheetID)
        {
            return DELib_SoundManager_IsCuesheetLoaded(cuesheetID);
        }

        /// <summary>
        /// Load a cuesheet so the sounds of it can be played.
        /// </summary>
        [DECompatibility(DEGames.YLAD)]
        public static void LoadCuesheet(SoundCuesheetID cuesheetID)
        {
            DELib_SoundManager_LoadCuesheet((ushort)cuesheetID);
        }

        /// <summary>
        /// Load a cuesheet so the sounds of it can be played.
        /// </summary>
        [DECompatibility(DEGames.YLAD)]
        public static void LoadCuesheet(ushort cuesheetID)
        {
            DELib_SoundManager_LoadCuesheet(cuesheetID);
        }

        /// <summary>
        /// Load a cuesheet so the sounds of it can be played.
        /// </summary>
        [DECompatibility(DEGames.YLAD)]
        public static void UnloadCuesheet(SoundCuesheetID cuesheetID)
        {
            DELib_SoundManager_UnloadCuesheet((ushort)cuesheetID);
        }


        /// <summary>
        /// Play the sound id from specified cuesheet. The cuesheet has to be loaded.
        /// </summary>
        [DECompatibility(DEGames.YLAD)]
        public static void PlayCue(ushort cuesheetID, ushort cueID, int start_msec)
        {
            DELib_SoundManager_PlayCue(cuesheetID, cueID, start_msec);
        }

        /// <summary>
        /// Get the ID of the current BGM 
        /// </summary>
        [DECompatibility(DEGames.YLAD)]
        public static uint GetBGMSeID(uint bgmSlot = 1)
        {
            return DELib_SoundManager_GetBGMSeID(bgmSlot);
        }

        /// <summary>
        /// Get the ID of the current BGM 
        /// </summary>
        [DECompatibility(DEGames.YLAD)]
        public static float GetBGMPlaytimeSec(uint bgmSlot = 1)
        {
            return DELib_SoundManager_GetBGMPlaytimeSec(bgmSlot);
        }

        /// <summary>
        /// Play the sound id from specified cuesheet. The cuesheet has to be loaded.
        /// </summary>
        [DECompatibility(DEGames.YLAD)]
        ///<summary>Play cue.</summary>
        public static void PlayCue(SoundCuesheetID cuesheetID, ushort cueID, int start_msec)
        {
            PlayCue((ushort)cuesheetID, cueID, 0);
        }

        [DECompatibility(DEGames.YLAD)]
        ///<summary>Play BGM music.</summary>
        public static void PlayBGM(ushort cuesheetID, ushort cueID, uint start_msec = 0)
        {
            DELib_SoundManager_PlayBGM(cuesheetID, cueID, start_msec);
        }

        public static void PlayBGM(uint id, uint start_msec = 0)
        {
            DELib_SoundManager_PlayBGM2(id, start_msec);
        }
    }
}
