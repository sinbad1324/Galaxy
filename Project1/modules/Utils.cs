using Microsoft.Xna.Framework;

namespace Galaxy.modules
{
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

        public static Color GetColorDarker(Color color, double factor)
        {
            if (factor < 0 || factor > 1)
                return color;

            int r = (int)(factor * color.R);
            int g = (int)(factor * color.G);
            int b = (int)(factor * color.B);
            return new Color(r, g, b);
        }

       

    }
}
