using Galaxy.Gui.GuiInterface;
using Galaxy.modules;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;

namespace Galaxy.Gui.Frames
{
    public class Frame:GlobalUI , IGlobal
    {
        public bool des;
        public Frame(ScreenGui screenGui, GlobalUI parent, string name  , Vector2 position, Vector2 size, Color color) {
            this.screenGui = screenGui;
            base.name = name;
            base.position = position;
            base.bgSize = size;
            base.bgColor = color;
            base.isLoaded = true;
            des = true;
            base.BgInit(parent);
        }

        public override void LoadContent(ContentManager content, GraphicsDevice device) {
            base.LoadContent(content, device);
            if (isLoaded)
            {
                this.texture = new Texture2D(device, 1, 1);
                isLoaded = false;
            }
        }
        public override void Update() {
            if (texture != null )
                texture.SetData<Color>(new Color[] { bgColor });
        
            base.Update();
        }
        public override void Draw(SpriteBatch target) {
            if (texture != null && des)
                target.Draw(texture, bg, bgColor); 
        }

        public void Destroy()
        {
            base.Bdestroy();
        }
    }
}
