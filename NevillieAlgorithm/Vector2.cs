using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NevillieAlgorithm
{
    public class Vector2
    {
        public float x, y;

        public Vector2()
        {
            x = 0.0f;
            y = 0.0f;
        }

        public Vector2(float inX, float inY)
        {
            x = inX;
            y = inY;
        }

        public static Vector2 operator *(Vector2 v, float t)
            => new Vector2(v.x * t, v.y * t);

        public static Vector2 operator /(Vector2 v, float t)
            => new Vector2(v.x / t, v.y / t);


        public static Vector2 operator +(Vector2 v1, Vector2 v2)
            => new Vector2(v1.x + v2.x, v1.y + v2.y);
    }
}
