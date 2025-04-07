using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace LearnMatrix
{
    public struct BorderSize
    {
        public event EventHandler<EventArgs> changed;
        private void LuncheEvent()
        {
            if (changed != null) changed(this, EventArgs.Empty);
        }
        private float _stroke;
        private float _left;
        private float _right;
        private float _top;
        private float _bottom;

        public float stroke { get { return _stroke; } set { _stroke = value; _left = value; _right = value; _top = value; _bottom = value; LuncheEvent(); } }
        public float left { get { return _left; } set { _left = value; LuncheEvent(); } }
        public float right { get { return _right; } set { _right = value; LuncheEvent(); } }
        public float top { get { return _top; } set { _top = value; LuncheEvent(); } }
        public float bottom { get { return _bottom; } set { _bottom = value; LuncheEvent(); } } 
    }
    public class RectBorder
    {
        public RoundedRectangle background;
        public RoundedRectangle border;
        public Texture2D BorderTexture;
        public Texture2D  BgTexture;
        public BorderSize borderSize;

        public Vector2 position { get { return background.position; } set { background.position = value; border.position = background.position - new Vector2(borderSize.left, borderSize.top); } }
        public Vector2 size { get { return background.size; } set { background.size = value; UpdateBorder(); } }
        // Private
        private void UpdateBorder()
        {
            border.position = background.position - new Vector2(borderSize.left, borderSize.top);
            border.size = background.size + new Vector2(borderSize.left + borderSize.right, borderSize.top + borderSize.bottom);
        }

        public RectBorder(Vector2 position, Vector2 size) {
            BorderTexture = new Texture2D(GlobalParams.Device, 1, 1);
            BorderTexture.SetData(new Color[] { Color.Black });
            BgTexture = new Texture2D(GlobalParams.Device, 1, 1);
            BgTexture.SetData(new Color[] { Color.White });
            background = new RoundedRectangle(position, size);
            border = new RoundedRectangle(position, size);
            border.SetNewTexture(BorderTexture);
           }
        public void SetBgTexture(Texture2D texture)
        {
            background.SetNewTexture(texture);
        }

        public void Initialize()
        {
            background.Initialize();
            border.Initialize();
            border.overflow =false;
            borderSize.changed += (sender, e) => UpdateBorder();
        }
        public void LoadContent()
        {

            background.LoadContent();
            border.LoadContent();
        }
        public void Update()
        {
            background.Update();
            border.Update();



        }
        public void Draw()
        {
            border.Draw();
            background.Draw();
        }
    }
}
