using Galaxy.Gui.Frames;
using Galaxy.Gui.GuiInterface;
using Galaxy.Gui.Images;
using Galaxy.Gui.Texts;
using LearnMatrix;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Project1.InstancePlugin.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml.Linq;


namespace Galaxy.Gui
{
    public class ScreenGui : GlobalUI
    {
        static Random random = new Random();


        public ScreenGui()
        {
            childrens = new Container(this);
            //BgInit(this);
            bgSize = new Vector2(GlobalParams.WINDOW_WIDHT, GlobalParams.WINDOW_HEIGTH);
            position = new Vector2(0f, 0f);
        }
        //Initialze
       public override void Initialize()
        {
            base.Initialize();
            TextLable Points = childrens.addTextLable("Points", "Points");
            Points.bgColor = Color.Transparent;
            Points.color = Color.White;

            //Frame fback = childrens.addFrame("background" , position,new Vector2(GlobalParams.WINDOW_WIDHT , GlobalParams.WINDOW_HEIGTH) , Color.Transparent);
            //fback.zIndex = 0;
        }
        //Load content
        //Setters

        public override void LoadContent()
        {
            this.childrens.LoadContent();
            base.LoadContent();
        }
        //update
        public override void Update()
        {
            this.childrens.Update();
            base.Update();
        }
        //render
        public override void Draw( ) {
            base.Draw( );
            //GlobalParams.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null);
            childrens.Draw();
            //GlobalParams.spriteBatch.End();

            for (int i = 0; i < childrens.container.Count; i++)
                childrens.container[i].DrawChildren();
            
        }
    }
}
