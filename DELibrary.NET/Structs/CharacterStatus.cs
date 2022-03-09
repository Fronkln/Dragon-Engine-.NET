using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    [StructLayout(LayoutKind.Sequential, Size = 0x10)]
    public struct CharacterStatusFlag
    {
        //RGGS: today i will use a 128 bit enum
        internal int i0;
        internal int i1;
        internal int i2;
        internal int i3; 
    }

    [Flags]
    public enum CharacterModeFlag
    {
        SystemPreparing = 1 << 1,
        Fighter = 1 << 2,
        Sync = 1 << 3,
        SyncOwner = 1 << 4
    }

    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct CharacterModeStatus
    {
        public CharacterModeFlag flags;
        public byte e_mode;
    }

    public struct CharacterStatus
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CCHARACTER_STATUS_SET_NO_INPUT_TEMPORARY", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr DELib_CharacterStatus_SetNoInputTemporary(IntPtr charaStatus);

        internal IntPtr pointer;

        public void SetNoInputTemporary()
        {
            DELib_CharacterStatus_SetNoInputTemporary(pointer);
        }
    }
}
