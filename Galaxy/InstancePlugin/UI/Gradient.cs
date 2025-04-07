using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using LearnMatrix;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Debuger.InstancePlugin.UI
{
    public struct ColorPoint
    {
        public Color color;
        private float _time;
        public float time { get { return _time; } set { _time = Math.Clamp(0, value, 1); } }
    }
    public class Gradient
    {
        //Constructor
        Vector2 startPoint = Vector2.Zero;
         static float width=200;
        static float height =200;

        static public Texture2D Linear(List<ColorPoint> colorsList,float angle)
        {
        Texture2D texture;
        //SOURCE -> CHAT GPT
        // Tableau de couleurs pour stocker les pixels de la texture
        Color[] color = new Color[(int)width * (int)height];

            // Centre de la texture pour faire pivoter autour du milieu de l'image
            float centerX = width / 2f;
            float centerY = height / 2f;

            // Pré-calcul des valeurs trigonométriques pour la rotation
            float cosA = MathF.Cos(angle);
            float sinA = MathF.Sin(angle);

            // Parcours de tous les pixels de l'image
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    // --- 1️⃣ Appliquer la rotation sur le point (x, y) ---
                    // Translation du point par rapport au centre de l'image
                    float relX = x - centerX;
                    float relY = y - centerY;

                    // Application de la matrice de rotation :
                    // x' = x * cos(angle) - y * sin(angle)
                    // y' = x * sin(angle) + y * cos(angle)
                    float rotatedX = relX * cosA - relY * sinA;
                    float rotatedY = relX * sinA + relY * cosA;

                    // --- 2️⃣ Convertir la coordonnée Y transformée en valeur normalisée t ---
                    // `rotatedY + centerY` recentre la valeur après la rotation
                    // Ensuite on divise par `(height - 1)` pour normaliser t entre 0 et 1
                    float t = (rotatedY + centerY) / (height - 1);

                    // --- 3️⃣ Récupérer les couleurs qui encadrent la valeur `t` ---
                    ColorPoint lastColorPoint = GetSmaller(colorsList, t);  // Plus petit point de couleur trouvé
                    ColorPoint currentColorPoint = GetBigger(colorsList,t); // Plus grand point de couleur trouvé

                    // --- 4️⃣ Calculer `localT`, un facteur de mélange entre les deux couleurs ---
                    float range = currentColorPoint.time - lastColorPoint.time;
                    float localT = (t - lastColorPoint.time) / range;

                    // --- 5️⃣ Interpoler entre les deux couleurs trouvées ---
                    Color newColor = Color.Lerp(lastColorPoint.color, currentColorPoint.color,
                                                MathHelper.Clamp(localT, 0, 1)); // Clamp pour éviter les valeurs hors limites

                    // --- 6️⃣ Appliquer la couleur au pixel correspondant ---
                    color[y * (int)width + x] = newColor;
                }
            }

            // --- 7️⃣ Création de la texture et application des couleurs ---
            texture = new Texture2D(GlobalParams.Device, (int)width, (int)height);
            texture.SetData(color);

            return texture;
        }


       static public ColorPoint GetSmaller(List<ColorPoint> colorsList, float t)
        {
            ColorPoint smaller = colorsList[0];
            float min = float.MaxValue;
            foreach (ColorPoint color in colorsList)
                if (color.time <= t&&Math.Abs(color.time - t) < min )
                {
                    min = Math.Abs(color.time - t);
                    smaller = color;
                }
            return smaller;
        }
        static public ColorPoint GetBigger(List<ColorPoint> colorsList, float t)
        {
            ColorPoint smaller = colorsList[colorsList.Count-1];
            float min = float.MaxValue;
            foreach (ColorPoint color in colorsList)
                if (color.time > t && Math.Abs(t - color.time) < min)
                {
                    min = Math.Abs(t - color.time);
                    smaller = color;
                }
            return smaller;
        }
        //Destructor
        public void Destroy()
        {

        }
    }
}
