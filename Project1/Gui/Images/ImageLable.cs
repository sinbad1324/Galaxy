using Galaxy.Gui.GuiInterface;
using Galaxy.Gui.Texts;
using Galaxy.modules;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;


namespace Galaxy.Gui.Images
{
   public class ImageLable : GlobalUI, IGlobal
    {
        public string imagePath;
        public ImageLable(ScreenGui screenGui , GlobalUI parent , string name , string ImagePath ) {
            base.name = name;
            imagePath = ImagePath;
            base.screenGui = screenGui;
            isLoaded = false;
            base.BgInit(parent);
        }

        public override void LoadContent(ContentManager content, GraphicsDevice device) {
            base.LoadContent(content, device);
            Console.WriteLine("Loaded"+this.name);
            if (!isLoaded)
            {
                texture = content.Load<Texture2D>(imagePath);
            //    texture.SetData<Color>(new Color[] { Color.White });
                isLoaded = true;
            }
        }


        public override void Update()
        {
  
            base.Update();
           // Console.WriteLine(bg.Location);
        }
        public override void Draw(SpriteBatch target) {
            if (!isLoaded) Console.WriteLine("Not loaded");
            if (texture == null) return;
            target.Draw(texture, bg, bgColor);
        }

        public virtual void DestroyVariables()
        {
            base.Bdestroy();  
        }
        public virtual void Destroy()
        {
            if (parent == null) return;
            int index = parent.childrens.container.FindIndex(a => a.name == this.name);
            if (parent != null && parent.childrens.container.ElementAt(index) != null)
            {
                Console.WriteLine(name + " destroyed");
                parent.childrens.container.RemoveAt(index);
                DestroyVariables();
                GC.SuppressFinalize(this);
            }
          
        }
    }
}
