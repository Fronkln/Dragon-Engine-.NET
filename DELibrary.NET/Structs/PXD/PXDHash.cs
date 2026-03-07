using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    [StructLayout(LayoutKind.Sequential, Size = 32)]
    public struct PXDHash
    {
        public ushort checksum; //0x0000
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
        char[] str; //0x0002

        public PXDHash(string value)
        {
            checksum = 0;
            str = new char[0];

            Set(value);
        }

        public override string ToString()
        {
            return str.ToNullTerminatedString();
        }

        public char[] GetChars()
        {
            return str;
        }

        public void Set(string val)
        {
            if (str == null || str.Length <= 0)
                str = new char[30];

            char[] valChar = val.ToCharArray();

            int len = (valChar.Length <= 30 ? valChar.Length : 30);

            for (int i = 0; i < len; i++)
                str[i] = valChar[i];

            checksum = 0;

            foreach (char c in str)
                checksum += (byte)c;
        }
    }
}
