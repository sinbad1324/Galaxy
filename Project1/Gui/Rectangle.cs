using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Project1.Gui
{
    public class Rectangle
    {
        private float Width;
        private float Height;
        private BasicEffect effect;
        List<VertexPositionColor[]> rect;
        public Rectangle(int width, int height)
        {
            Width = width;
            Height = height;
            rect = Create2DRectangle(new Vector2(50, 50), new Vector2(Width, Height), Color.White, null);
        }

       public void LoadContent(GraphicsDevice graphicsDevice)
        {
            effect = new BasicEffect(graphicsDevice);
            effect.VertexColorEnabled = true;
            effect.World  = Matrix.Identity;
        }

        public VertexPositionColor[] Triangle(Vector2 p1, Vector2 p2, Vector2 p3, Color color)
        {
            return new VertexPositionColor[3] {
                new VertexPositionColor(new Vector3(p1, 0), color),
                new VertexPositionColor(new Vector3(p2, 0), color),
                new VertexPositionColor(new Vector3(p3, 0),color)
            };
        }

        public List<VertexPositionColor[]> Create2DRectangle(Vector2 Position, Vector2 size, Color color, Texture2D texture)
        {
            Vector2 topLeft = new Vector2(Position.X - size.X / 2, Position.Y - size.Y / 2);
            Vector2 topRight = new Vector2(Position.X + size.X / 2, Position.Y - size.Y / 2);
            Vector2 bottomLeft = new Vector2(Position.X - size.X / 2, Position.Y + size.Y / 2);
            Vector2 bottonRight = new Vector2(Position.X + size.X / 2, Position.Y + size.Y / 2);
           return new List<VertexPositionColor[]>() { 
                Triangle(Position, topLeft, topRight, color),
                Triangle(Position, bottomLeft, bottonRight, color),
                Triangle(Position, topLeft, bottomLeft, color),
                Triangle(Position, topRight, bottonRight, color) 
            };
        }
        public void Draw(GraphicsDevice device, VertexPositionColor[] vertices)
        {
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                for (global::System.Int32 i = 0; i < rect.Count; i++)
                {
                    device.DrawUserPrimitives(PrimitiveType.TriangleList, rect[i], 0, 2);   
                }
            }
        }
    }
}
