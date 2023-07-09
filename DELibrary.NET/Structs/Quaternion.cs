using System;
using System.Runtime.InteropServices;
using System.Xml.Linq;

namespace DragonEngineLibrary
{
    [StructLayout(LayoutKind.Sequential, Size = 16)]
    public struct Quaternion
    {
        public float x;
        public float y;
        public float z;
        public float w;

        public static readonly Quaternion Identity = new Quaternion();

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

        public Vector3 ToEulerAngles()
        {
            Vector3 euler;

            // if the input quaternion is normalized, this is exactly one. Otherwise, this acts as a correction factor for the quaternion's not-normalizedness
            float unit = (x * x) + (y * y) + (z * z) + (w * w);

            // this will have a magnitude of 0.5 or greater if and only if this is a singularity case
            float test = x * w - y * z;

            if (test > 0.4995f * unit) // singularity at north pole
            {
                euler.x = (float)Math.PI / 2;
                euler.y = (float)(2f * Math.Atan2(y, x));
                euler.z = 0;
            }
            else if (test < -0.4995f * unit) // singularity at south pole
            {
                euler.x = (float)-Math.PI / 2;
                euler.y = -2f * (float)Math.Atan2(y, x);
                euler.z = 0;
            }
            else // no singularity - this is the majority of cases
            {
                euler.x = (float)Math.Asin(2f * (w * x - y * z));
                euler.y = (float)Math.Atan2(2f * w * y + 2f * z * x, 1 - 2f * (x * x + y * y));
                euler.z = (float)Math.Atan2(2f * w * z + 2f * x * y, 1 - 2f * (z * z + x * x));
            }

            // all the math so far has been done in radians. Before returning, we convert to degrees...
            euler *= 360 / (float)(Math.PI * 2);
            

            //...and then ensure the degree values are between 0 and 360
            euler.x %= 360;
            euler.y %= 360;
            euler.z %= 360;

            return euler;
        }


        public override string ToString()
        {
            return $"({x.ToString("0.00")} {y.ToString("0.00")} {z.ToString("0.00")} {w.ToString("0.00")})";
        }
    }
}
