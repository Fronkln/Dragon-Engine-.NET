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
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x30)]
        public byte[] temp;
        [FieldOffset(0x30)]
        public Matrix4x4 matrix;
    }
}
