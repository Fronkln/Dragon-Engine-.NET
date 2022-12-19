using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    [StructLayout(LayoutKind.Sequential, Size = 12)]
    public struct Vector3
    {
        public float x;
        public float y;
        public float z;

        public Vector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        /// <summary>
        /// Vector3(0,0,0,)
        /// </summary>
        public static Vector3 zero
        {
            get
            {
                return new Vector3();
            }
        }

        /// <summary>
        /// Vector4(1,1,1,1)
        /// </summary>
        public static Vector3 one
        {
            get
            {
                return new Vector3(1, 1, 1);
            }
        }

        /// <summary>
        /// Up direction.
        /// </summary>
        public static Vector3 up
        {
            get
            {
                return new Vector3(0, 1, 0);
            }
        }

        /// <summary>
        /// Up direction.
        /// </summary>
        public static Vector3 forward
        {
            get
            {
                return new Vector3(0, 0, 1);
            }
        }

        /// <summary>
        /// Right direction.
        /// </summary>
        public static Vector3 right
        {
            get
            {
                return new Vector3(-1, 0, 0);
            }
        }

        public float magnitude
        {
            get 
            { 
                return (float)Math.Sqrt(x * x + y * y + z * z); 
            }
        }

        public Vector3 normalized
        {
            get { return Normalize(this); }
        }

        public override string ToString()
        {
            return $"({x.ToString("0.00")} {y.ToString("0.00")} {z.ToString("0.00")})";
        }

        public static explicit operator Vector3(Vector4 vec4)
        {
            return new Vector3(vec4.x, vec4.y, vec4.z);
        }

        public static Vector3 Lerp(Vector3 a, Vector3 b, float t)
        {
            return new Vector3(
                a.x + (b.x - a.x) * t,
                a.y + (b.y - a.y) * t,
                a.z + (b.z - a.z) * t
            );
        }

        public static Vector3 operator +(Vector3 a, Vector3 b)
        {
            Vector3 outVec;

            outVec.x = a.x + b.x;
            outVec.y = a.y + b.y;
            outVec.z = a.z + b.z;

            return outVec;
        }
        public static Vector3 operator -(Vector3 a, Vector3 b)
        {
            Vector3 outVec;

            outVec.x = a.x - b.x;
            outVec.y = a.y - b.y;
            outVec.z = a.z - b.z;

            return outVec;
        }

        public static Vector3 operator -(Vector3 a)
        {
            Vector3 outVec;

            outVec.x = -a.x;
            outVec.y = -a.y;
            outVec.z = -a.z;

            return outVec;
        }

        public static Vector3 operator *(Vector3 a, Vector3 b)
        {
            Vector3 outVec;

            outVec.x = a.x * b.x;
            outVec.y = a.y * b.y;
            outVec.z = a.z * b.z;

            return outVec;
        }

        public static Vector3 operator *(Vector3 a, float f)
        {
            Vector3 outVec;

            outVec.x = a.x * f;
            outVec.y = a.y * f;
            outVec.z = a.z * f;

            return outVec;
        }

        public static Vector3 operator /(Vector3 a, float d)
        {
            return new Vector3(a.x / d, a.y / d, a.z / d);
        }

        public static bool operator ==(Vector3 v1, Vector3 v2)
        {
            return (v1.x == v2.x && v1.y == v2.y  && v1.z == v2.z);
        }

        public static bool operator !=(Vector3 v1, Vector3 v2)
        {
            return (v1.x != v2.x || v1.y != v2.y || v1.z != v2.z);
        }

        public static float Distance(Vector3 a, Vector3 b)
        {
            float diff_x = a.x - b.x;
            float diff_y = a.y - b.y;
            float diff_z = a.z - b.z;

            return (float)Math.Sqrt(diff_x * diff_x + diff_y * diff_y + diff_z * diff_z);
        }

        public static float Dot(Vector3 lhs, Vector3 rhs)
        {
            return lhs.x * rhs.x + lhs.y * rhs.y + lhs.z * rhs.z;
        }

        public static Vector3 Normalize(Vector3 vec)
        {
            float mag = vec.magnitude;

            if (mag > 0.00001f)
                return vec / mag;
            else
                return zero;
        }



    }
}
