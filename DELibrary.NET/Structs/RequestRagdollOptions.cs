using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    [StructLayout(LayoutKind.Sequential)]
    public struct RequestRagdollOptions
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x30)]
        public byte[] buf;
    }
}
