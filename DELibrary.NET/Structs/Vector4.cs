using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    
    [StructLayout(LayoutKind.Sequential, Size = 0x10)]
    public struct Vector4
    {
        public float x;
        public float y;
        public float z;
        public float w;

        public Vector4(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            w = 0;
        }

        public Vector4(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        /// <summary>
        /// Vector4(0,0,0,0)
        /// </summary>
        public static Vector4 zero
        {
            get
            {
                return new Vector4();
            }
        }

        /// <summary>
        /// Vector4(1,1,1,1)
        /// </summary>
        public static Vector4 one
        {
            get
            {
                return new Vector4(1, 1, 1, 1);
            }
        }

        /// <summary>
        /// Up direction.
        /// </summary>
        public static Vector4 up
        {
            get
            {
                return new Vector4(0, 1, 0);
            }
        }

        public override string ToString()
        {
            return $"({x.ToString("0.00")} {y.ToString("0.00")} {z.ToString("0.00")} {w.ToString("0.00")})";
        }

        public static implicit operator Vector4(Vector3 vec3)
        {
            return new Vector4(vec3.x, vec3.y, vec3.z);
        }

        public static Vector4 Lerp(Vector4 a, Vector4 b, float t)
        {
            return new Vector4(
                a.x + (b.x - a.x) * t,
                a.y + (b.y - a.y) * t,
                a.z + (b.z - a.z) * t,
                a.w + (b.w - a.w) * t
            );
        }

        public static Vector4 operator +(Vector4 a, Vector4 b)
        {
            Vector4 outVec;

            outVec.x = a.x + b.x;
            outVec.y = a.y + b.y;
            outVec.z = a.z + b.z;
            outVec.w = a.w + b.w;

            return outVec;
        }

        public static Vector4 operator -(Vector4 a, Vector4 b)
        {
            Vector4 outVec;

            outVec.x = a.x - b.x;
            outVec.y = a.y - b.y;
            outVec.z = a.z - b.z;
            outVec.w = a.w - b.w;

            return outVec;
        }

        public static Vector4 operator *(Vector4 a, Vector4 b)
        {
            Vector4 outVec;

            outVec.x = a.x * b.x;
            outVec.y = a.y * b.y;
            outVec.z = a.z * b.z;
            outVec.w = a.w * b.w;

            return outVec;
        }

        public static Vector4 operator *(Vector4 a, float f)
        {
            Vector4 outVec;

            outVec.x = a.x * f;
            outVec.y = a.y * f;
            outVec.z = a.z * f;
            outVec.w = a.w * f;

            return outVec;
        }

        public static float Distance(Vector4 a, Vector4 b)
        {
            float diff_x = a.x - b.x;
            float diff_y = a.y - b.y;
            float diff_z = a.z - b.z;
            float diff_w = a.w - b.w;
            return (float)Math.Sqrt(diff_x * diff_x + diff_y * diff_y + diff_z * diff_z + diff_w * diff_w);
        }
    }
}
