using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    [StructLayout(LayoutKind.Explicit, Pack = 16)]
    public struct PoseInfo
    {
        [FieldOffset(0)]
        public Vector4 Position;
        [FieldOffset(0x10)]
        public float Angle;

        public PoseInfo(Vector4 pos, float angle)
        {
            Position = pos;
            Angle = angle;
        }
    }
}
