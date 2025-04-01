using Galaxy.Gui.GuiInterface;
using Galaxy.modules;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using LearnMatrix;

namespace Galaxy.Gui.Frames
{
    
    public class Frame:GlobalUI , IGlobal
    {

        public bool des;
        public Frame( GlobalUI parent, string name  , Vector2 position, Vector2 size, Color color) {
            base.name = name;
            base.isLoaded = true;
            des = true;
            base.BgInit(parent);
            base.position = position;
            base.bgSize = size;
            base.bgColor = color;
        } 

        public override void LoadContent() {
            base.LoadContent();
            if (isLoaded)
            {
                this.background.texture = new Texture2D(GlobalParams.Device, 1, 1);
                isLoaded = false;
            }
        }
        public override void Update() {
            base.Update();
        }
        public override void Draw() {
            base.Draw();
        }

        public override void Destroy()
        {
            base.Destroy();
        }
    }
}
