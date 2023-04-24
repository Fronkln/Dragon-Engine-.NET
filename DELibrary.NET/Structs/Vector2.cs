using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonEngineLibrary
{
    public struct Vector2
    {
        public float x;
        public float y;

        public Vector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public static implicit operator Vector3(Vector2 vec)
        {
            return new Vector3(vec.x, vec.y, 0);
        }

        public static implicit operator Vector4(Vector2 vec)
        {
            return new Vector4(vec.x, vec.y, 0, 0);
        }
    }
}
