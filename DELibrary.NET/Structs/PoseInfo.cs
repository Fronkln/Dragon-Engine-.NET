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
        Vector4 Position;
        [FieldOffset(0x10)]
        float Angle;
        //required for CPP matching
       // private byte[] _padding0;

        public PoseInfo(Vector4 pos, float angle)
        {
            Position = pos;
            Angle = angle;

            //_padding0 = new byte[0xC];
        }
    }
}
