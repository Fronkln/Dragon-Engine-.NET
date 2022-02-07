using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DragonEngineLibrary
{
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
    }
}
