using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary.Structs
{
    [StructLayout(LayoutKind.Sequential, Size = 0x40)]
    public struct Matrix4x4
    {
        Vector4 m_vm0;
        Vector4 m_vm1;
        Vector4 m_vm2;
        Vector4 m_vm3;
    }
}
