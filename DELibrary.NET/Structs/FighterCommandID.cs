using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    [StructLayout(LayoutKind.Sequential, Size = 0x4)]
    public struct FighterCommandID
    {
        public ushort set_;
        public ushort cmd;

        public FighterCommandID(ushort set, ushort cmd)
        {
            set_ = set;
            this.cmd = cmd;
        }

        public FighterCommandID(BattleCommandSetID set, ushort cmd)
        {
            set_ = (ushort)set;
            this.cmd = cmd;
        }
    };
}
