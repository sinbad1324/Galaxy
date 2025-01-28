using Galaxy.Gui.GuiInterface;
using System;
using System.Collections.Generic;
using Galaxy.modules;
using Galaxy.Gui.Frames;
using Galaxy.Gui.Images;
using Galaxy.Gui.Texts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Reflection.Metadata;
using Project1.InstancePlugin.UI;
using System.Reflection;

namespace Galaxy.Gui
{
    public class Container : IGlobal
    {
        public List<GlobalUI> container;
        public List<Flex> Flexs;
        ScreenGui ScreenGui;
        GlobalUI parent;
        public GraphicsDevice device;
        private ContentManager Content;

        public Container(ScreenGui ScreenGui, GlobalUI parent)
        {
            this.ScreenGui = ScreenGui;
            container = new List<GlobalUI >();
            Flexs = new List<Flex>();
            this.parent = parent;
        }

        public void Initialize()
        {

            //for (int i = 0; i < container.Count; i++)
            //    container[i].Initialize();
            //for (int i = 0; i < length; i++)
            //{
                
            //}
        }
        //Setters
        //ImageButton
        public ImageButton addImageButton(string name = "ImageButton", string imagePath = "Logo")
        {
            ImageButton btnimg = new ImageButton(ScreenGui, parent, name, imagePath);
            btnimg.Initialize();
            if (Content != null && device != null)
            {
                btnimg.LoadContent(this.Content, device);
            }
            container.Add(btnimg);
            return btnimg;
        }

        //ImageLable
        public ImageLable addImageLable(string name = "ImageLable", string imagePath = "Logo")
        {
            ImageLable textLable = new ImageLable(ScreenGui, parent, name, imagePath);
            textLable.Initialize();
            //Prob
            if (Content != null && device != null)
            {
                textLable.LoadContent(this.Content, device);
            }
            container.Add(textLable);
            return textLable;
        }

        //TextButton
        public TextButton addTextButton(string name = "TextButton", string text = "Text...", string font = "arial")
        {
            TextButton btn = new TextButton(ScreenGui, parent, name, text, font);
            btn.Initialize();
            //Prob
            if (Content != null && device != null)
            {
                btn.LoadContent(this.Content, device);
            }
            container.Add(btn);
            return btn;
        }

        //TextBox
        public TextBox addTextBox(string name = "TextBox", string text = "Text...", string font = "arial")
        {
            TextBox textBox = new TextBox(ScreenGui, parent, name, text, font);
            textBox.zIndex = 1;
            textBox.Initialize();
            if (Content != null && device != null)
            {
                textBox.LoadContent(this.Content, device);
            }
            container.Add(textBox);
            return textBox;
        }

        //TextLable
        public TextLable addTextLable(string name = "TextLable", string text = "Text...", string font = "arial")
        {
            TextLable textLable = new TextLable(ScreenGui, parent, name, text, font);
            textLable.Initialize();
            if (Content != null && device != null)
            {
                textLable.LoadContent(this.Content, device);
            }
            container.Add(textLable);
            return textLable;
        }

        //Frame
        public Frame addFrame(string name, Vector2 position, Vector2 size, Color color)
        {
            Frame frame = new Frame(this.ScreenGui, parent, name, position, size, color);
            frame.bgColor = color;
            frame.bgSize = size;
            frame.position = position;
            if (Content != null && device != null)
            {
                frame.LoadContent(this.Content, device);
            }
            frame.Initialize();
            this.container.Add(frame);
            return frame;
        }

        public ScrollingFarme addScrollingFarme(string name, Vector2 position, Vector2 size, Color color)
        {
            ScrollingFarme frame = new ScrollingFarme(this.ScreenGui, parent, name, position, size, color);
            frame.Initialize();
            if (Content != null && device != null)
            {
                frame.LoadContent(this.Content, device);
            }
            this.container.Add(frame);
            return frame;
        }

        public Flex addFlexBox(string name , FlexAlagniement flexAlagniement , float padding)
        {

            Flex flex = new Flex(parent ,name, flexAlagniement , padding);
            flex.Initialize();
            this.Flexs.Add(flex);
            return flex;
        }

        public T FindChildren<T>(string name) where T : class
        {
            T child = default(T);
            if (typeof(T) == typeof(Flex))
            {
                foreach (var item in Flexs)
                {
                    if (item.name == name && child == null)
                    {
                        child =  item as T;
                        if (child != null)
                        {
                            break;
                        }
                    }
                }
            }
            else
            {
                foreach (var item in container)
                {
                    if (item.name == name && child == null)
                    {
                        child = item as T;
                        if (child != null)
                        {
                            break;
                        }
                    }
                }
            }
            return child;
        }
        //Load content
        public void LoadContent(ContentManager content, GraphicsDevice device)
        {
            this.Content = content;
            this.device = device;

            for (int i = 0; i < container.Count; i++)           
                container[i].LoadContent(content, device);
            
        }
        //update
        public void Update()
        {
            for (int i = 0; i < Flexs.Count; i++)
            {
                Flexs[i].Update();

            }
            for (int i = 0; i < container.Count; i++)
            {
                container[i].Update();
            }
        }
        //render
        public void Draw(SpriteBatch target)
        {
            for (int i = 0; i < container.Count; i++)
                container[i].Draw(target);
        }

    }
}
