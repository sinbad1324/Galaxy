using Galaxy.modules;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Galaxy.workspace.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Galaxy.Gui.GuiInterface;
using System.Reflection.Metadata;
using Galaxy.VFX;
using System.IO;
using LearnMatrix;

namespace Galaxy.workspace
{
    public class ObjectContainer : IGlobal
    {
        public List<GlobalObject> container;
        IGlobalParentObj parent;


        public ObjectContainer( IGlobalParentObj parent) { 
       container = new List<GlobalObject>();
            this.parent = parent;
        }

        public void Initialize()
        {
            /*Initialize codes */

        }

        public Part addPart( string name = "Part", string textureName = "block")
        {
            Part part = new Part( parent, name, textureName , container.Count+1);
            part.LoadContent();
            container.Add(part);
            
            return part;
        }

        public Flipbook addFlipbook(string path= "8x8-Explosion", int colums = 8, string name = "Flipbook" )
        {
            Flipbook flipbook = new Flipbook( colums , path, name , container.Count+1 , parent);
                flipbook.LoadContent();

            container.Add(flipbook);
            return flipbook;
        }
        public T FindChildren<T>(string name) where T : class
        {
            T textLable = default(T);
            foreach (var item in container)
            {
                if (item != null &&item.name == name && textLable == null)
                {
                    textLable = item as T;
                    if (textLable != null)
                    {
                        break;
                    }
                }
            }
            return textLable;
        }

        //Load content
        public void LoadContent()
        {
            foreach (var item in container.ToList())
                item.LoadContent();
        }
        //update
        public void Update()
        {
            foreach (var item in container.ToList())
                if (item != null)
                    item.Update();
        }
        //render
        public void Draw()
        {
            foreach (var item in container.ToList())
                if (item != null)
                    item.Draw();
        }
        public void Destroy()
        {
            container.Clear();
            GC.SuppressFinalize(this);
        }
    }
}
