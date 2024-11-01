using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    [StructLayout(LayoutKind.Sequential)]
    public struct ExEffectInfo
    {
        public int effID;
        public int idk;
        public int effSetID;
        public int idk2;
        public int category;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x24)]
        public byte[] whatever;
        public bool bKeepInfinity;
        public byte nKeepTurn;
        public byte nKeepAtk;
        public byte nKeepDamage;
        public byte nKeepHeavyDamage;
        public byte nKeepRandomDamage;
        public bool bLastStep;
        public bool bBootOnTurn;
        public bool bTempIgnoreDamage;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x24)]
        public byte[] whatever2;
    }
}
