using Galaxy.Gui.Frames;
using Galaxy.Gui.GuiInterface;
using Galaxy.Gui.Images;
using Galaxy.Gui.Texts;
using Galaxy.workspace.Objects;
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
        public int screenWidth {  get; }
        public int screenHeight {get; }
        public GameWindow window;
        public GalaxyMotor motor;

        public ScreenGui(GalaxyMotor motor, int _screenWidth, int _screenHeight , GameWindow window)
        {
            childrens = new Container(this ,this);
            this.screenWidth = _screenWidth;
            this.screenHeight = _screenHeight;
            this.window = window;
            screenGui = this;
            bgSize = new Vector2(screenWidth, screenHeight);
            position = new Vector2(0f, 0f);
            this.motor = motor;
        }

    
        //Accessors
      
     
        //Initialze
       public override void Initialize()
        {
            base.Initialize();
            TextLable Points= childrens.addTextLable( "Points" , "Points");
            Points.bgColor = Color.Black;
            Points.color = Color.White;
     
        }
        //Load content
        //Setters

        public override void LoadContent(ContentManager content, GraphicsDevice device)
        {
            this.childrens.LoadContent(content, device);
        }
        //update
        public override void Update()
        {

            this.childrens.Update();
        }
        //render
        public override void Draw(SpriteBatch target) {

            target.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null);
            childrens.Draw(target);
            target.End();

            for (int i = 0; i < childrens.container.Count; i++)
                childrens.container[i].DrawChildren(target);
            
        }
    }
}
