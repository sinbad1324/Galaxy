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
    }
}
