using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Security.Cryptography;
using LearnMatrix;
using System.Threading;

namespace Galaxy.modules.Maths
{
    public class Formules
    {
        public static double InterpolationLinaire(KeyPoints k1, KeyPoints k2, double t)
        {
            return k1.value + (t - k1.time) / (k2.time - k1.time) * (k2.value - k1.value);
        }
        public static float Lerp(float start, float end, float time)
        {
            return start + (end - start) * time;
        }
        public static double Lerp(double start, double end, double time)
        {
            return start + (end - start) * time;
        }
        static public Vector2 CubicBezier(float t, Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
        {
            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;
            float uuu = uu * u;
            float ttt = tt * t;

            Vector2 result = uuu * p0 + 3 * uu * t * p1 + 3 * u * tt * p2 + ttt * p3;
            return result;
        }

        public static Vector2 GetDirectionSpeed(Vector2 a, Vector2 b, float totalTime, float currentTime)
        {
            //float clampedTime = Math.Min(currentTime, totalTime);

            Vector2 dir = Vector2.Normalize(b - a);

            float distance = (b - a).Length() / totalTime;
            Vector2 newPos = a + dir * (distance * currentTime);

            return float.IsNaN(newPos.X) == true || float.IsNaN(newPos.Y) == true ? b : newPos;
        }

    }
}
