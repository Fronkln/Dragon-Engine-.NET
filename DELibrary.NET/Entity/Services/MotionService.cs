using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary.Service
{

    public static class MotionService
    {
        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct TimingResult
        {
            public int Start;
            public int End;
            public uint Param;

            public bool IsValid()
            {
                return Start >= 0; 
            }

            private static bool _isEqual(TimingResult a, TimingResult b)
            {
                return a.Start == b.Start && a.End == b.End && a.Param == b.Param;
            }

            public static bool operator ==(TimingResult a,  TimingResult b)
            {
                return _isEqual(a, b);
            }

            public static bool operator !=(TimingResult a, TimingResult b)
            {
                return !_isEqual(a, b);
            }
        }

        /// <summary>
        /// Not implemented yet sorry trolled
        /// </summary>
        /// <param name="motion"></param>
        public static void GetFrames(MotionID motion)
        {

        }

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CSERVICE_MOTION_SEARCH_TIMING", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool SearchTiming(uint start_frame, uint bepID, uint type, IntPtr callback);


        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CSERVICE_MOTION_SEARCH_TIMING_DETAIL", CallingConvention = CallingConvention.Cdecl)]
        internal static extern TimingResult DELib_MotionService_SearchTimingDetail(uint start_frame, uint bepID, uint type);


        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CSERVICE_MOTION_SEARCH_TIMING_DETAIL", CallingConvention = CallingConvention.Cdecl)]
        public static extern TimingResult SearchTimingDetail(uint start_frame, uint bepID, uint type);


        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CSERVICE_MOTION_GET_BEP_ID", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint GetBepID(MotionID motion);
    }
}
