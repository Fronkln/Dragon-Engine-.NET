using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    [StructLayout(LayoutKind.Sequential, Size = 16)]
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
                return  new Quaternion();
            }
        }

        public Quaternion(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }
        public static Vector3 operator *(Quaternion rotation, Vector3 point)
        {
            float x = rotation.x * 2F;
            float y = rotation.y * 2F;
            float z = rotation.z * 2F;
            float xx = rotation.x * x;
            float yy = rotation.y * y;
            float zz = rotation.z * z;
            float xy = rotation.x * y;
            float xz = rotation.x * z;
            float yz = rotation.y * z;
            float wx = rotation.w * x;
            float wy = rotation.w * y;
            float wz = rotation.w * z;

            Vector3 res;
            res.x = (1F - (yy + zz)) * point.x + (xy - wz) * point.y + (xz + wy) * point.z;
            res.y = (xy + wz) * point.x + (1F - (xx + zz)) * point.y + (yz - wx) * point.z;
            res.z = (xz - wy) * point.x + (yz + wx) * point.y + (1F - (xx + yy)) * point.z;
            return res;
        }


        public override string ToString()
        {
            return $"({x.ToString("0.00")} {y.ToString("0.00")} {z.ToString("0.00")} {w.ToString("0.00")})";
        }
    }
}
