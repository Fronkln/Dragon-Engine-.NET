using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    [StructLayout(LayoutKind.Sequential, Size = 0x50)]
    public struct DynamicsPoseInfo
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x50)]
        private byte[] _temporary;
    }
}
