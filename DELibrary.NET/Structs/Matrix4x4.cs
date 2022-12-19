using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    [StructLayout(LayoutKind.Sequential, Size = 0x40)]
    public unsafe struct Matrix4x4
    {
        public Vector4 m_vm0;
        public Vector4 m_vm1;
        public Vector4 m_vm2;
        public Vector4 m_vm3;

        public Vector4 Position { get { return m_vm3; } }

        public Vector4 LeftDirection { get { return m_vm0; } set { m_vm0 = value; } }
        public Vector4 UpDirection { get { return m_vm1; } set { m_vm1 = value; } }
        public Vector4 ForwardDirection { get { return m_vm2; } set { m_vm2 = value; } }

        public override string ToString()
        {
            string outp = "";

            outp += $"[{m_vm0.ToString()}]\n";
            outp += $"[{m_vm1.ToString()}]\n";
            outp += $"[{m_vm2.ToString()}]\n";
            outp += $"[{m_vm3.ToString()}]";

            return outp;
        }
    }
}
