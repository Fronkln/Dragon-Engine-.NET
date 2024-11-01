using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    [StructLayout(LayoutKind.Explicit, Size = 0x4)]
    public struct FighterCommandID
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_FIGHTERCOMMANDID_GET_INFO", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr DELib_FighterCommandID_GetInfo(FighterCommandID input);

        [FieldOffset(0)]
        public ushort set_;
        [FieldOffset(2)]
        public ushort cmd;

        public FighterCommandID(ushort set, ushort cmd)
        {
            set_ = set;
            this.cmd = cmd;
        }

        public FighterCommandID(ushort set, short cmd)
        {
            set_ = set;
            this.cmd = (ushort)cmd;
        }

        public FighterCommandID(BattleCommandSetID set, ushort cmd)
        {
            set_ = (ushort)set;
            this.cmd = cmd;
        }


        public FighterCommandInfo GetInfo()
        {
            IntPtr inf = DELib_FighterCommandID_GetInfo(this);

            if (inf == IntPtr.Zero)
                return new FighterCommandInfo();
            else
                return Marshal.PtrToStructure<FighterCommandInfo>(inf);
        }
    }
}
