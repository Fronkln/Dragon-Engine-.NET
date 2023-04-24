using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    [StructLayout(LayoutKind.Explicit, Pack = 16, Size = 0x40)]
    public struct HActRangeInfo
    {
        [FieldOffset(0x0)] public Vector4 Pos;
        [FieldOffset(0x10)] public Quaternion Rot;
        [FieldOffset(0x20)] public uint Range;
        [FieldOffset(0x24)] public uint AutoSource;
        [FieldOffset(0x28)] public ulong dynamics_handle_;
        [FieldOffset(0x30)] public byte id;
        [FieldOffset(0x31)] public bool is_large_ok;
        [FieldOffset(0x32)] public bool is_play_range_in;

        public Matrix4x4 GetMatrix()
        {
            Matrix4x4 mtx = new Matrix4x4();
            mtx.Position = Pos;
            mtx.ForwardDirection = Rot * Vector3.forward;
            mtx.UpDirection = Rot * Vector3.up;
            mtx.LeftDirection = Rot * -Vector3.right;

            return mtx;
        }
    };
}
