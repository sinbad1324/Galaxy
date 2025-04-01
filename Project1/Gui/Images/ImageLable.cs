using Galaxy.Gui.GuiInterface;
using Galaxy.Gui.Texts;
using Galaxy.modules;
using LearnMatrix;
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
        public ImageLable( GlobalUI parent , string name , string ImagePath ) {
            base.name = name;
            imagePath = ImagePath;
            isLoaded = false;
            base.BgInit(parent);
        }

        public override void LoadContent() {
            base.LoadContent();
            if (!isLoaded)
            {
                background.texture = GlobalParams.Content.Load<Texture2D>(imagePath);
            //    texture.SetData<Color>(new Color[] { Color.White });
                isLoaded = true;
            }
        }


        public override void Update()
        {
            base.Update();
           // Console.WriteLine(bg.Location);
        }
        public override void Draw() {
            if (!isLoaded) Console.WriteLine("Not loaded");
            if (background.texture == null) return;
            base.Draw();
        }

        public virtual void DestroyVariables()
        {
            base.Destroy();  
        }
        public override void Destroy()
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
