using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MotionTimingStatus
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x150)]
        private byte[] _temporary;
    }
}
