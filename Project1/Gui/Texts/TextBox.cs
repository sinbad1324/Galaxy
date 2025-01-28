using Galaxy.Events;
using Galaxy.modules;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

using System;
using System.Text;
using Galaxy.Gui.GuiInterface;
using System.Linq;


namespace Galaxy.Gui.Texts
{
    public class TextBox : TextLable
    {
        //Private
        private bool pressed;
        private bool clicked;
        private bool isHover;
        private StringBuilder NewText;

        //public
        public bool ClearTextOnFocus;
        //Event
        public mouseButton1Event Button1Click;
        public LostFocused FocusedEvent;
        //Constructor
        public TextBox(ScreenGui screen, GlobalUI parent, string name, string text, string fontName) : base(screen, parent, name, text, fontName)
        {
            initVariables();
            initEvent();

        }

        //Events
        private void TextInput(object seder, TextInputEventArgs e)
        {
            if (clicked)
            {
                if (e.Key != Keys.Back && e.Key != Keys.Tab)
                {
                    NewText.Append(e.Character);
                }
                if (e.Key == Keys.Back)
                {
                    if (NewText.Length > 0)
                    {
                        NewText.Remove(NewText.Length - 1, 1);
                    }
                }
                base.text = NewText.ToString();
            }
        }

        private void Hover(object seder, HoverEventArgs e)
        {
            isHover = true;
            Mouse.SetCursor(MouseCursor.IBeam);
        }
        private void HoverLeave(object seder, EventArgs e)
        {
            Mouse.SetCursor(MouseCursor.Arrow);
            isHover = false;
        }

        //Functions

        //Private
        private void initEvent()
        {
            // event
            base.hover.Hover += Hover;
            base.hover.HoverLeave += HoverLeave;

            Button1Click.Pressed += (sender, e) =>
            {
                clicked = true;
                base.text = "";
            };
            screenGui.window.TextInput += TextInput;
        }

        private void initVariables()
        {
            FocusedEvent = new LostFocused();
            isHover = false;
            NewText = new StringBuilder();
            pressed = false;
            clicked = false;
            ClearTextOnFocus = true;
            Button1Click = new mouseButton1Event();

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
                    if (bg.Contains(mgs.Position.ToVector2()))
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
                    if (bg.Contains(mgs.Position.ToVector2()))
                    {
                        Button1Click.UnPressedAction();
                    }
                }
            }
            //Key
            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Enter))
            {
                if (clicked)
                {
                    clicked = false;
                    FocusedEvent.Action(NewText.ToString());
                    if (ClearTextOnFocus)
                        NewText = new StringBuilder();
                }
            }
            if (mgs.LeftButton == ButtonState.Pressed || mgs.RightButton == ButtonState.Pressed)
                if (!bg.Contains(mgs.Position))
                    if (clicked)
                    {
                        clicked = false;
                        FocusedEvent.Action(NewText.ToString());
                        if (ClearTextOnFocus)
                            NewText = new StringBuilder();
                    }
        }
        //Public
        public override void DestroyVariables()
        {
            try
            {
                screenGui.window.TextInput -= TextInput;
                base.hover.Hover -= Hover;
                base.hover.HoverLeave -= HoverLeave;
                if (isHover)
                    Mouse.SetCursor(MouseCursor.Arrow);

                base.DestroyVariables();
                Button1Click = null;
                clicked = false;
                pressed = false;
                NewText = null;
                isHover = false;
            }
            catch { }
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
        public override void Update()
        {
            base.Update();
            Event();

        }
    }
}
