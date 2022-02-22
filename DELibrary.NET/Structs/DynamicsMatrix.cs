using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    /// <summary>
    /// Only bytes for the time being
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = 0x70)]
    public struct DynamicsMatrix
    {
        [FieldOffset(0x0)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x70)]
        public byte[] temp;
    }
}
