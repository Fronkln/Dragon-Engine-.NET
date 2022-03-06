using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    [StructLayout(LayoutKind.Explicit, Pack = 8, Size = 16)]
    public struct EntityUID
    {
        [FieldOffset(0x0)]
        public ulong UID;

        //union real
        [FieldOffset(0x0)]
        public uint Serial;
        [FieldOffset(0x4)]
        public EUIDKind Kind;
        [FieldOffset(0x6)]
        public ushort User;

        [FieldOffset(0x8)]
        public ushort KindGroupBits;
    }
}
