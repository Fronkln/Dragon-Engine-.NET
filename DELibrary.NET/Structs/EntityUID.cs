using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    [StructLayout(LayoutKind.Explicit, Pack = 8)]
    public struct EntityUID
    {
        [FieldOffset(0x0)]
        public ulong UID;
        [FieldOffset(0x8)]
        public ushort KindGroupBits;
    }
}
