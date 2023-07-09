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
            return new Vector2(
                (value.x * targetRes.x) / sourceRes.x, 
                (value.y * targetRes.y) / sourceRes.y
            );
        }
    }
}
