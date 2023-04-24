using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonEngineLibrary
{
    public static class Screen
    {
        public static Vector2 ConvertResolution(Vector2 value, Vector2 sourceRes, Vector2 targetRes)
        {
            Vector2 vec = new Vector2();
            vec.x = (value.x * targetRes.x) / sourceRes.x;
            vec.y = (value.y * targetRes.y) / sourceRes.y;

            return vec;
        }
    }
}
