    using Galaxy.Events;
using Galaxy.Gui.GuiInterface;
using Galaxy.Gui.Texts;
using Galaxy.modules;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galaxy.Gui.Images
{
    public class ImageButton : ImageLable
    {
        private bool pressed;
        private bool isHover;
        public mouseButton1Event Button1Click;

        public ImageButton(GlobalUI parent, string name, string ImagePath): base( parent, name, ImagePath) 
        {
            initVariables();
            
        }
        private void initVariables()
        {
            Button1Click = new mouseButton1Event();
        }
        private void Hover(object seder, HoverEventArgs args)
        {
            Mouse.SetCursor(MouseCursor.Hand);
            isHover = true;
        }

        private void HoverLeave(object seder, EventArgs args)
        {
            isHover = false;
            Mouse.SetCursor(MouseCursor.Arrow);

        }

        private void initEvent()
        {
            // event
            base.hover.Hover += Hover;
            base.hover.HoverLeave += HoverLeave;
        }

        public override void DestroyVariables()
        {
            base.hover.Hover -= Hover;
            base.hover.HoverLeave -= HoverLeave;
            if (isHover)
                Mouse.SetCursor(MouseCursor.Arrow);

            base.DestroyVariables();
            Button1Click = null;
            pressed = false;
            isHover = false;

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

        private void Event()
        {
            MouseState mgs = Mouse.GetState();
            bool LeftClicked = mgs.LeftButton == ButtonState.Pressed;
            if (LeftClicked && !pressed)
            {
                pressed = true;
                if (mgs.Position.ToVector2().X >= 0 && mgs.Position.ToVector2().Y >= 0)
                {
                    if (background.Contains(mgs.Position.ToVector2()))
                    {
                        Button1Click.PressedAction();
                    }
                }
            }

            if (pressed && mgs.LeftButton == ButtonState.Released)
            {
                pressed = false;
                if (mgs.Position.ToVector2().X >= 0 && mgs.Position.ToVector2().Y >= 0)
                {
                    if (background.Contains(mgs.Position.ToVector2()))
                    {
                        Button1Click.UnPressedAction();
                    }
                }
            }
        }


        public override void Update()
        {
            base.Update();
            Event();

        }

    }
}
