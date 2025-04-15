using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Galaxy.modules
{
    public  class Colors
    {
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
    }
}
