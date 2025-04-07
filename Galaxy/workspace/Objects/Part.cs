using Galaxy.Gui.GuiInterface;
using Galaxy.modules;
using LearnMatrix;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using System;


namespace Galaxy.workspace.Objects
{
    public class Part : GlobalObject, IGlobal 
    {
        private string texturePath ;
        public Part( IGlobalParentObj parent , string name , string textureName , int id) {

            this.id = id;   
            this.name = name;
            this.texturePath = textureName;
            base.SpriteInit(parent);
            
        }

        public override void Update()
        {
            base.Update(); 
        }



        public override void LoadContent() {
            if (texturePath != null)
            {
                texture =  GlobalParams.Content.Load<Texture2D>(texturePath);
            }
        }
        public override void Draw() {
            if (texture != null)
            {                Vector2 scale = new Vector2(Sprite.Width / (float)texture.Width, Sprite.Height / (float)texture.Height);
                GlobalParams.spriteBatch.Draw(texture,
                         position,
                         null, this.SpriteColor,
                         rotation,
                         new Vector2(0, 0),
                         scale,
                         SpriteEffects.None, 0);
            }

        }

        public void Destroy()
        {
            base.Bdestroy();
            GC.SuppressFinalize(this);
        }

    }
}
