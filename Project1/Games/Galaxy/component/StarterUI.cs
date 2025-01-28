using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Galaxy.Events;
using Galaxy.Gui.GuiInterface;
using Galaxy.Gui.Texts;
using Galaxy.Gui;
using Microsoft.Xna.Framework;
using Galaxy.workspace;
using System.Threading;
using Galaxy.Gui.Images;
using Galaxy.Gui.Frames;
using Project1.InstancePlugin.UI;
namespace Project1.Games.Galaxy.component
{
    internal class StarterUI
    {
        public static TextButton StartBtn(GlobalUI parent)
        {

            TextButton StartBtn = parent.childrens.addTextButton("StartBtn", "Start");
            StartBtn.color = Color.Black;
            StartBtn.bgColor = Color.White;
            StartBtn.bgSize = new Vector2(200f, 50f);
            StartBtn.position = new Vector2((parent.screenGui.screenWidth / 2) - 100, parent.screenGui.screenHeight / 2 + 30);
            StartBtn.verticalAligne = VerticalTextAligne.verticalCenter;
            StartBtn.horizontalAligne = HorizontalTextAligne.harizontalCenter;
            StartBtn.zIndex = 0;
            float scale = 1.2f;
            StartBtn.hover.Hover += (object obj, HoverEventArgs e) =>
            {
                scale = 1.2f;
                StartBtn.bgSize = new Vector2(200f * scale, 50f * scale);
                StartBtn.position = new Vector2((parent.screenGui.screenWidth / 2) - (100f * scale), parent.screenGui.screenHeight / 2 + 30 * scale);
            };
            StartBtn.hover.HoverLeave += (object obj, EventArgs e) =>
            {
                scale = 1f;
                StartBtn.bgSize = new Vector2(200f * scale, 50f * scale);
                StartBtn.position = new Vector2((parent.screenGui.screenWidth / 2) - 100, parent.screenGui.screenHeight / 2 + 30 * scale);
            };

            return StartBtn;
  
        }
        public static TextButton RestartBtnComp(GlobalUI parent)
        {
            TextButton btn = parent.childrens.addTextButton("RestartButton", "Re start");
            btn.bgSize = new Vector2(300, 50);
            btn.position = new Vector2(parent.screenGui.screenWidth / 2 - 150, parent.screenGui.screenHeight / 2 - 25);
            btn.color = Color.Black;
            btn.horizontalAligne = HorizontalTextAligne.harizontalCenter;
            btn.verticalAligne = VerticalTextAligne.verticalCenter;
            return btn;
        }

        public static  Dictionary<string,string> Form(GlobalUI parent) {
            Dictionary<string,string> data = new Dictionary<string,string>();
            Vector2 parentSize = new Vector2(400,400);
            Frame parentDiv = parent.childrens.addFrame("UserForm" , parent.bgSize/2- parentSize/2 , parentSize , Color.AliceBlue);
            parentDiv.zIndex = 5;
            // Legend
            TextLable Title = parentDiv.childrens.addTextLable("Title", "User Form", "Font/CSMSG");
            Title.bgSize = new Vector2(300, 75);
            Title.horizontalAligne = HorizontalTextAligne.harizontalCenter;
            Title.verticalAligne = VerticalTextAligne.verticalCenter;
            Title.bgColor = Color.Transparent;
            // User Name
            TextBox userName = parentDiv.childrens.addTextBox("tbx user name" , "User Name", "Font/CSMSG");
            userName.bgSize =new Vector2(300 , 75);
            userName.horizontalAligne = HorizontalTextAligne.harizontalCenter;
            userName.verticalAligne = VerticalTextAligne.verticalCenter;
            userName.overflow = true;
            // Password
            TextBox userPassword = parentDiv.childrens.addTextBox("tbx user password", "Password", "Font/CSMSG");
            userPassword.bgSize = new Vector2(300, 75);
            userPassword.horizontalAligne = HorizontalTextAligne.harizontalCenter;
            userPassword.verticalAligne = VerticalTextAligne.verticalCenter;
            userPassword.overflow = true;
            // Submit
            TextButton submit = parentDiv.childrens.addTextButton("btn submit", "Submit" , "Font/CSMSG");
            submit.bgSize = new Vector2(300, 75);
            submit.horizontalAligne = HorizontalTextAligne.harizontalCenter;
            submit.verticalAligne = VerticalTextAligne.verticalCenter;
            submit.overflow = true;
            submit.Button1Click.Pressed += (object seder, EventArgs e) =>
            {
                data["name"] = userName.text;
                data["password"] = userPassword.text;
                Console.WriteLine(userName.text);
                userName.Destroy();
                userPassword.Destroy();
                submit.Destroy();
            };
            Flex flexBox = parentDiv.childrens.addFlexBox("ParentDivFlex", FlexAlagniement.Vertical, 10f);
            flexBox.flexWrap = false;
            flexBox.verticalAlagniement = Alagniement.Center;
            flexBox.horizontalAlagniement = Alagniement.Center;

            return data;
        }

        static public void CreateHealthContainer(GlobalUI parent , int nbHp)
        {
            float size = 40;
            float x = size * nbHp;
            Frame healthContainer = parent.childrens.addFrame("HealthContainer", new Vector2(0, 0), new Vector2(x, size), Color.Yellow);
            healthContainer.position = new Vector2(parent.screenGui.screenWidth - (x + 5), 5);
            healthContainer.bgSize = new Vector2(300, size);
            healthContainer.overflow = false;
            healthContainer.bgColor = Color.Transparent;
            for (int i = 0; i < nbHp; i++)
            {
                ImageLable imgH = healthContainer.childrens.addImageLable("health" + i, "health");
                imgH.bgSize = new Vector2(size, size);
            }
            Flex flex = healthContainer.childrens.addFlexBox("flex", FlexAlagniement.Horizontal, 0);
            flex.horizontalAlagniement = Alagniement.Start;
            flex.verticalAlagniement = Alagniement.Start;
        }
    }
}
