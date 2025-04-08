using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework;

namespace Galaxy.modules
{
    public struct KeyPoints
    {
        private float _time;
        public float time { get { return _time; } set { _time = Math.Clamp(0, value, 1); } }        public double value;
        public double env;
    }
      public struct MinMax
    {
        private double _min;
        private double _max;
        public double min {set{if (value > _max) throw new ArgumentOutOfRangeException(nameof(min) , "Cette (min) valeur est plus grand que le max"); _min = value;}  get{return _min;}}
        public double max {set{if (value < _min) throw new ArgumentOutOfRangeException(nameof(max) , "Cette (max) valeur est plus petit que le min"); _max = value;}  get{return _max;}}

       public MinMax(double min , double max)
        {
            this._min = min;
            this._max = max;
            if (_min > _max) throw new ArgumentOutOfRangeException(nameof(min), "Cette (min) valeur est plus grand que le max");

        }
    }
    public class Utils
    {
        //public static bool Intersection(Vector2 Position , Vector2 Size , Vector2 PointCompare) {
        //     float start = Position.Length();     
        //     float disance = PointCompare.Length() - start;
        //     if (disance >= 0 && PointCompare.X <= (Position.X+Size.X) && PointCompare.Y <= (Position.Y + Size.Y))
        //     {
        //     return true;

        //     }
        //     return false;
        // }

        public static void Print(string separator, params object[] args)
        {
            string affichage = "";
            for (int i = 0; i < args.Length - 1; i++)
                if (i > 0)
                    affichage += "_" + affichage;
            Console.WriteLine(affichage);
        }


        public static Vector2 GetDirectionSpeed(Vector2 a, Vector2 b, float totalTime, float currentTime)
        {
            //float clampedTime = Math.Min(currentTime, totalTime);

            Vector2 dir = Vector2.Normalize(b - a);

            float distance = (b - a).Length() / totalTime;
            Vector2 newPos = (a + dir * (distance * currentTime));

            return float.IsNaN(newPos.X) == true || float.IsNaN(newPos.Y) == true ? b : newPos;
        }



        public static float Lerp(float start, float end, float time)
        {
            return start + (end - start) * time;
        }
         public static double Lerp(double start, double end, double time)
        {
            return start + (end - start) * time;
        }

        public static Color GetColorDarker(Color color, double factor)
        {
            if (factor < 0 || factor > 1)
                return color;
            int r = (int)(factor * color.R);
            int g = (int)(factor * color.G);
            int b = (int)(factor * color.B);
            return new Color(r, g, b);
        }

        static public Random random = new Random();

        static public Color randomColor()
        {
            return new Color(random.Next(0, 256), random.Next(0, 256), random.Next(0, 256));
        }
        static public Dictionary<string, float> GetSegements(Vector2 Position, Vector2 size)
        {
            return new Dictionary<string, float>{
                { "left", Position.X - size.X / 2 },
                { "right", Position.X + size.X / 2 },
                { "top", Position.Y - size.Y / 2 },
                { "bottom", Position.Y + size.Y / 2 }
            };
        }
        static public Vector2 CubicBezier(float t, Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
        {
            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;
            float uuu = uu * u;
            float ttt = tt * t;

            Vector2 result = (uuu * p0) + (3 * uu * t * p1) + (3 * u * tt * p2) + (ttt * p3);
            return result;
        }

    }
}
