using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DragonEngineLibrary
{
    public static class TimelineManager
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CTIMELINE_MANAGER_CHECK_CLOCK_ACHIEVEMENT", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool CheckClockAchievement(uint timeline_id, uint sheet_id, uint clock_id);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CTIMELINE_MANAGER_GET_CURRENT_CLOCK", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetCurrentClock(uint timeline_id, uint sheet_id);
    }
}
