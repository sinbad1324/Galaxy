using Galaxy.Gui.GuiInterface;
using Galaxy.modules;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Input;
using System.Buffers.Text;


namespace Galaxy.Gui.Frames
{
    public enum ScrollingFrameDirection
    {
        X,
        Y
    }
    public class ScrollingFarme : GlobalUI
    {


        public Rectangle baseUi;
        public Rectangle scrollFrame;
        // public Rectangle Frame;
        public ScrollingFrameDirection Axe
        {
            get { return _Axe; }
            set
            {
                _Axe = value;
                //  Console.WriteLine(Math.Max(bgSize.Y, baseUi.Y) + "EWFEWOFWOEF");
                CanvasSize = new Vector2(0, Math.Max(bgSize.Y, baseUi.Y));
                if (Axe == ScrollingFrameDirection.X)
                    CanvasSize = new Vector2(Math.Max(bgSize.X, baseUi.X), 0);

                if (Axe == ScrollingFrameDirection.X)
                {
                    AxeSize = new Vector2(Math.Clamp(CanvasSize.X / baseUi.X, 10, 20), 10);
                    AxePosition = new Vector2(baseUi.Location.X, baseUi.Location.Y + baseUi.Size.Y - 10);
                }
                else
                {
                    try
                    {
                        AxePosition = new Vector2(baseUi.Location.X + baseUi.Size.X - 10, baseUi.Location.Y);
                        AxeSize = new Vector2(10, Math.Clamp(CanvasSize.Y / baseUi.Y, 10, 20));

                    }
                    catch { Console.WriteLine("Errpr"); }
                }
            }
        }
        public float scrollPosition
        {
            get
            {
                if (Axe == ScrollingFrameDirection.X) return AxePosition.X;
                else return AxePosition.Y;
            }
            set
            {

                if (Axe == ScrollingFrameDirection.X)
                    AxePosition = new Vector2((float)Math.Clamp(value, baseUi.Location.X, (baseUi.Location.X + baseUi.Size.X) - AxeSize.X), baseUi.Location.Y + baseUi.Size.Y - AxeSize.Y);
                else
                    AxePosition = new Vector2(baseUi.Location.X + baseUi.Size.X - AxeSize.X, (float)Math.Clamp(value, baseUi.Location.Y, (baseUi.Location.Y + baseUi.Size.Y) - AxeSize.Y));
            }
        }
        public Color scrollColor;
        public Vector2 CanvasSize
        {
            get { return _canvasSize; }
            set
            {
                _canvasSize = new Vector2(Math.Max(value.X, baseUi.Size.X), Math.Max(value.Y, baseUi.Size.Y));
                bg.Size = _canvasSize.ToPoint();
            }
        }
        //Private
        private Vector2 _canvasSize;
        private int oldWheelValue;
        private int scrollSpeed;
        private ScrollingFrameDirection _Axe;
        private int ancienPos = 0;
        private Vector2 AxeSize;
        private Vector2 AxePosition;
        private bool isPress;
        private Texture2D textureBaseUi;
        private Texture2D textureBaseUi2;

        public ScrollingFarme(ScreenGui screenGui, GlobalUI parent, string name, Vector2 position, Vector2 size, Color color)
        {
            this.screenGui = screenGui;
            BgInit(parent);
            this.name = name;
            oldWheelValue = Mouse.GetState().ScrollWheelValue;
            bg = new Rectangle(200, 100, 100, 100);
            bgColor = color;
            scrollSpeed = 50;
            this.position = position;
            this.bgSize = size;
            baseUi = new Rectangle((int)position.X, (int)position.Y, (int)bgSize.X, (int)bgSize.Y);
        }

        public override void Initialize()
        {
            base.Initialize();
            scrollColor = Utils.GetColorDarker(bgColor, .7);
            Axe = ScrollingFrameDirection.Y;

            scrollFrame = new Rectangle((int)AxePosition.X, (int)AxePosition.Y, (int)AxeSize.X, (int)AxeSize.Y);
            bg.Location = baseUi.Location;
            bg.Size = baseUi.Size + CanvasSize.ToPoint();
            bgSize = bg.Size.ToVector2();
            rasterizerState = new RasterizerState { ScissorTestEnable = true };
        }
        public override void LoadContent(ContentManager content, GraphicsDevice device)
        {
            base.LoadContent(content, device);
            this.texture = new Texture2D(device, 1, 1);
            texture.SetData<Color>(new Color[] { bgColor });

            this.textureBaseUi = new Texture2D(device, 1, 1);
            textureBaseUi.SetData<Color>(new Color[] { Color.Transparent });
            this.textureBaseUi2 = new Texture2D(device, 1, 1);
            textureBaseUi2.SetData<Color>(new Color[] { scrollColor });
        }

        private int GetMouseWheel()
        {
            int value = 0;
            int currentWheelValue = Mouse.GetState().ScrollWheelValue;
            if (currentWheelValue < oldWheelValue)
            {
                value = 1;
            }
            else if (currentWheelValue > oldWheelValue)
            {
                value = -1;

            }
            oldWheelValue = currentWheelValue;
            return value;
        }

        public override void Update()
        {

            //base.Update();
            if (Axe == ScrollingFrameDirection.X)
                scrollSpeed = (bg.Size.X - baseUi.Size.X) <= 0 ? 0 : (bg.Size.X / baseUi.Size.X); 
            
            else
                scrollSpeed = (bg.Size.Y - baseUi.Size.Y) <= 0 ? 0 : (bg.Size.Y / baseUi.Size.Y);

            int MouseWheel = GetMouseWheel();
            scrollFrame.Location = AxePosition.ToPoint();
            scrollFrame.Size = AxeSize.ToPoint();
            if (Axe == ScrollingFrameDirection.X)
                this.bg.Location = new Point(baseUi.Location.X + Math.Max(((int)AxePosition.X - baseUi.Location.X) - 1, 0) * scrollSpeed * -1, this.bg.Location.Y);
            else
                this.bg.Location = new Point(this.bg.Location.X, baseUi.Location.Y + Math.Max(((int)AxePosition.Y - baseUi.Location.Y) - 1, 0) * scrollSpeed * -1);
            position = this.bg.Location.ToVector2();
            bgSize = this.bg.Size.ToVector2();
            MouseState mouseState = Mouse.GetState();
            if (baseUi.Contains((float)mouseState.Position.X, (float)mouseState.Position.Y))
                scrollPosition += MouseWheel * (scrollSpeed +5);
            if (scrollFrame.Contains((float)mouseState.Position.X, (float)mouseState.Position.Y) && mouseState.LeftButton == ButtonState.Pressed)
            {
                Console.WriteLine("Pressed");
            }
            childrens.Update();
        }
        public override void Draw(SpriteBatch target)
        {
            if (texture != null && textureBaseUi != null)
            {
                target.Draw(texture, baseUi, bgColor);
            }
        }

        public override void DrawChildren(SpriteBatch target)
        {
            target.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, rasterizerState);
            target.GraphicsDevice.ScissorRectangle = baseUi;
            target.Draw(textureBaseUi, bg, baseUi, Color.Transparent);
            childrens.Draw(target);
            if (bg.Size != baseUi.Size)
                target.Draw(textureBaseUi2, scrollFrame, baseUi, scrollColor);
                
            
            target.End();

            for (int i = 0; i < childrens.container.Count; i++)
                childrens.container[i].DrawChildren(target);
        }

    }
}
