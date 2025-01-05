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

namespace Galaxy.workspace
{
    public class ObjectContainer : IGlobal
    {
        public List<GlobalObject> container;
        Workspace workspace;
        IGlobalParentObj parent;
        private GraphicsDevice device;
        private ContentManager content;

        public ObjectContainer(Workspace workspace , IGlobalParentObj parent) { 
            this.workspace = workspace; container = new List<GlobalObject>();
            this.parent = parent;
        }

        public void Initialize()
        {
            /*Initialize codes */

        }

        public Part addPart( string name = "Part", string textureName = "block")
        {
            Part part = new Part(this.workspace, parent, name, textureName , container.Count+1);
            if (content != null && device!=null)
                part.LoadContent(content , device);
            
            container.Add(part);
            return part;
        }

        public Flipbook addFlipbook(string path= "8x8-Explosion", int colums = 8, string name = "Flipbook" )
        {
            Flipbook flipbook = new Flipbook(this.workspace, colums , path, name , container.Count+1 , parent);
            if (content != null && device != null)
                flipbook.LoadContent(content, device);
            
            container.Add(flipbook);
            return flipbook;
        }
        public T FindChildren<T>(string name) where T : class
        {
            T textLable = default(T);
            foreach (var item in container)
            {
                if (item.name == name && textLable == null)
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
        public void LoadContent(ContentManager content, GraphicsDevice device)
        {
            this.content = content;
            this.device = device;
            foreach (var item in container.ToList())
                item.LoadContent(content, device);
        }
        //update
        public void Update()
        {
            foreach (var item in container.ToList())
                if (item != null)
                    item.Update();
        }
        //render
        public void Draw(SpriteBatch target)
        {
            foreach (var item in container.ToList())
                if (item != null)
                    item.Draw(target);
        }
    }
}
