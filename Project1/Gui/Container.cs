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
        GlobalUI parent;


        public Container( GlobalUI parent)
        {
            container = new List<GlobalUI>();
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
            ImageButton btnimg = new ImageButton( parent, name, imagePath);
            btnimg.Initialize();

            btnimg.LoadContent();

            container.Add(btnimg);
            btnimg.zIndex = 0;
            return btnimg;
        }

        //ImageLable
        public ImageLable addImageLable(string name = "ImageLable", string imagePath = "Logo")
        {
            ImageLable textLable = new ImageLable(parent, name, imagePath);
            textLable.Initialize();
            //Prob

            textLable.LoadContent();

            container.Add(textLable);
            textLable.zIndex = 0;
            return textLable;
        }

        //TextButton
        public TextButton addTextButton(string name = "TextButton", string text = "Text...", string font = "arial")
        {
            TextButton btn = new TextButton( parent, name, text, font);
            btn.Initialize();
            //Prob

            btn.LoadContent();

            container.Add(btn);
            btn.zIndex = 0;
            return btn;
        }

        //TextBox
        public TextBox addTextBox(string name = "TextBox", string text = "Text...", string font = "arial")
        {
            TextBox textBox = new TextBox( parent, name, text, font);
            textBox.zIndex = 1;
            textBox.Initialize();

            textBox.LoadContent();

            container.Add(textBox);
            textBox.zIndex = 0;
            return textBox;
        }

        //TextLable
        public TextLable addTextLable(string name = "TextLable", string text = "Text...", string font = "arial")
        {
            TextLable textLable = new TextLable(parent, name, text, font);
            textLable.Initialize();

            textLable.LoadContent();

            container.Add(textLable);
            textLable.zIndex = 0;
            return textLable;
        }

        //Frame
        public Frame addFrame(string name, Vector2 position, Vector2 size, Color color)
        {
            Frame frame = new Frame( parent, name, position, size, color);

            frame.Initialize();
            frame.LoadContent();
            Console.WriteLine();
            this.container.Add(frame);
            frame.bgColor = color;
            //frame.zIndex = 0;
            return frame;
        }

        public ScrollingFarme addScrollingFarme(string name, Vector2 position, Vector2 size, Color color)
        {
            ScrollingFarme frame = new ScrollingFarme( parent, name, position, size, color);
            frame.Initialize();

            frame.LoadContent();

            this.container.Add(frame);
            frame.zIndex = 0;
            return frame;
        }

        public Flex addFlexBox(string name, FlexAlagniement flexAlagniement, float padding)
        {

            Flex flex = new Flex(parent, name, flexAlagniement, padding);
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
                        child = item as T;
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
        public void LoadContent()
        {


            for (int i = 0; i < container.Count; i++)
                container[i].LoadContent();
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
        public void Draw()
        {
            for (int i = 0; i < container.Count; i++)
                container[i].Draw();
        }

    }
}
