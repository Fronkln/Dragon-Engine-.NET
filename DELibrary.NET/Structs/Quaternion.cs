using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Quaternion
    {
        public float x;
        public float y;
        public float z;
        public float w;

        public static Quaternion identity
        {
            get
            {
                return new Quaternion();
            }
        }

        public Quaternion(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public override string ToString()
        {
            return $"({x.ToString("0.00")} {y.ToString("0.00")} {z.ToString("0.00")} {w.ToString("0.00")})";
        }
    }
}
