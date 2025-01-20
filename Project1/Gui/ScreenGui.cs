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
using System.Text;
using System.Xml.Linq;


namespace Galaxy.Gui
{
    public class ScreenGui : GlobalUI
    {
        static Random random = new Random();
        public int screenWidth {  get; }
        public int screenHeight {get; }
        public GameWindow window;


        public ScreenGui(int _screenWidth, int _screenHeight , GameWindow window)
        {

            childrens = new Container(this ,this);
            this.screenWidth = _screenWidth;
            this.screenHeight = _screenHeight;
            this.window = window;
            bgSize = new Vector2(screenWidth, screenHeight);
            position = new Vector2(0f, 0f);
        }

    
        //Accessors
      
     
        //Initialze
       public override void Initialize()
        {
            base.Initialize();
            TextLable Points= childrens.addTextLable( "Points" , "Points");
            Points.bgColor = Color.Black;
            Points.color = Color.White;
            CreateHealthContainer();

            TextBox textLable = childrens.addTextBox("tEXTLABLE");
            textLable.overflow = true;
            textLable.horizontalAligne = HorizontalTextAligne.left;
            textLable.bgSize = new Vector2(300, 300);
            textLable.ClearTextOnFocus = false;

            ScrollingFarme scr = childrens.addScrollingFarme("scr" , new Vector2(20 , 50 ) ,new Vector2(500,500) , Color.White);
            scr.Axe = ScrollingFrameDirection.X;
            scr.CanvasSize = new Vector2(screenWidth, 0);
            for (int i = 0; i < 50; i++)
            {
                ImageLable imgH = scr.childrens.addImageLable("health" + i, "health");
                imgH.bgSize = new Vector2(40, 40);
                imgH.position = new Vector2(((float)(40 * i) + 2f), 0);
            }
            Flex flex = scr.childrens.addFlexBox("flex" , FlexAlagniement.Horizontal , 5f);
            flex.horizontalAlagniement = Alagniement.Center;
            flex.verticalAlagniement = Alagniement.Center;
            flex.flexWrap = true;

        }
        //Load content
        //Setters
        private void CreateHealthContainer()
        {
            Frame healthContainer = childrens.addFrame("HealthContainer", new Vector2(0, 0), new Vector2(200, 40), Color.Yellow);
            healthContainer.position = new Vector2(screenWidth - 205, 5);
            healthContainer.bgSize = new Vector2(200, 40);
            healthContainer.overflow = false;
            healthContainer.bgColor = Color.Transparent;
            for (int i = 0; i < 5; i++)
            {
                ImageLable imgH = healthContainer.childrens.addImageLable("health" + i, "health");
                imgH.bgSize = new Vector2(40, 40);
                imgH.position = new Vector2(((float)(40 * i) + 2f), 0);
            }


        }
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
