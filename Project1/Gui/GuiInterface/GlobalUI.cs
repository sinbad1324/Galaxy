
using System;
using System.Linq;
using System.Threading;
using Galaxy.Events;
using Galaxy.Gui.Frames;
using Galaxy.modules;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Microsoft.Xna.Framework.Input;


namespace Galaxy.Gui.GuiInterface
{
    public class GlobalUI :  global::Galaxy.modules.IGlobal
    {
        public string name;

        private int _zIndex;
        private int _transparency;
        private bool _overflow;
        public bool overflow
        {
            get { return _overflow; }
            set { rasterizerState = new RasterizerState { ScissorTestEnable = value }; _overflow = value; }
        }
        public Vector2 bgSize { get; set; }
        public Container childrens { get; set; }
        Vector2 basePosition;
        public Vector2 position { get { return basePosition + (parent != null ? parent.position : new Vector2(0,0)) ; } set { basePosition = value; } }
        public int transparency
        {
            get { return _transparency; }
            set
            {
                _transparency = value;
                if (_transparency < 0)
                    _transparency = 0;
                if (_transparency > 255)
                    _transparency = 255;

                this.bgColor = new Color(bgColor.R, bgColor.G, bgColor.B, _transparency);
            }
        }

        public Color bgColor;
        public Rectangle bg;

        public int border;
        public Color borderColor;
        public bool isLoaded;
        public ScreenGui screenGui;
        public GlobalUI parent;
        public int zIndex
        {
            get { return _zIndex; }
            set
            {
                _zIndex = value;
                    ZIndexFunc();
                new Thread(() =>
                {
                    Thread.Sleep(1000);
                    for (int i = 0; i < parent.childrens.container.Count; i++)
                    {
                        Console.WriteLine(parent.childrens.container[i].name + "__" + i);
                    }
                }).Start();
            }
        }
   
        //events
        public Events.BoundaryColl boundaryColl;
        public Events.HoverEvent hover;
        public Vector2 boundaryCollEventLunched;
        protected Texture2D texture;
        private bool hoverIsLeave;
        private bool isHover;
        protected RasterizerState rasterizerState;

        //setters 
        public void Move(float x, float y)
        {
            void lunchAction(Vector2 pos)
            {
                if (pos != boundaryCollEventLunched)
                {
                    boundaryColl.EventAction();
                    boundaryCollEventLunched = pos;
                }
            }
            float Xpos = position.X + x;
            float Ypos = position.Y + y;
            //   Console.WriteLine(x);

            //   Console.Write(x+ " " + Xpos + " " + bgSize.X + " " + screenGui.screenWidth +"\n" );
            if (Xpos + bgSize.X >= screenGui.screenWidth)
            {
                Xpos = (float)screenGui.screenWidth - bgSize.X;
                lunchAction(new Vector2(Xpos, position.Y));
            }
            else if (Xpos <= 0)
            {
                Xpos = 0f;
                lunchAction(new Vector2(Xpos, position.Y));
            }
            if (Ypos + bgSize.Y > screenGui.screenHeight)
            {
                Ypos = (float)screenGui.screenHeight - bgSize.Y;
                lunchAction(new Vector2(position.Y, Ypos));
            }
            else if (Ypos < 0)
            {
                Ypos = 0f;
                lunchAction(new Vector2(position.Y, Ypos));
            }
            //   Console.WriteLine(Xpos);
            ///    Console.WriteLine(Ypos);
            position = new Vector2(Xpos, Ypos);
        }
        private void ZIndexFunc()
        {
            if (parent != null && parent.childrens.container.Count >= 2)
                parent.childrens.container = screenGui.childrens.container.OrderBy(i => i.zIndex).ToList();
        }
        //Update
        public virtual void Update()
        {
           
            if (texture != null && bg != Rectangle.Empty)
            {      
                if (bg.Contains(Mouse.GetState().Position.ToVector2()))
                {
                    isHover = true;
                    hoverIsLeave = false;
                    if (Mouse.GetState().Position.ToVector2().X >= 0 && Mouse.GetState().Position.ToVector2().Y >= 0)
                        hover.EventAction(Mouse.GetState().Position.ToVector2());
                }
                else if (!hoverIsLeave && isHover)
                {
                    hoverIsLeave = true;
                    isHover = false;
                    hover.EventHoverLeaveAction();
                }
                float Diff = new Vector2(screenGui.screenWidth, screenGui.screenHeight).Length() - position.Length();
                if (Diff <= 1)              
                    boundaryCollEventLunched = new Vector2(0, 0);     
                this.bg.Size = new Point((int)this.bgSize.X, (int)this.bgSize.Y);
                this.bg.Location = position.ToPoint(); 
                childrens.Update();
            }

        }
        public virtual void LoadContent(ContentManager content, GraphicsDevice device)
        {
            childrens.LoadContent(content, device);
        }

        public virtual void Initialize()
        {
            childrens.Initialize();

        }
        //draw
        public virtual void Draw(SpriteBatch target) { }
        public virtual void DrawChildren(SpriteBatch target)
        {
            target.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, this.rasterizerState);
            target.GraphicsDevice.ScissorRectangle = bg;
            childrens.Draw(target);
            target.End();

            for (int i = 0; i < childrens.container.Count; i++)
                childrens.container[i].DrawChildren(target);
        }

        // setter


        protected void BgInit(GlobalUI parent)
        {
            overflow = false;
            this.parent = parent;
            isHover = false;
            boundaryCollEventLunched = new Vector2(0, 0);
            boundaryColl = new BoundaryColl();
            hover = new HoverEvent();
            bgSize = new Vector2(50f, 50f);
            position =new Vector2(50f, 50f);
            transparency = 0;
            bgColor = Color.White;
            border = 0;
            borderColor = Color.Black;
            bg = new Rectangle((int)position.X, (int)position.Y, (int)bgSize.X, (int)bgSize.Y);
            childrens = new Container(screenGui, this);
            this.rasterizerState = new RasterizerState { ScissorTestEnable = overflow };
            //  zIndex = 1;
            _zIndex = 1;
         //   ZIndexFunc();
        }


        public void Bdestroy()
        {
            boundaryColl = null;
            screenGui = null;
            hover = null;
            texture = null;
            parent = null;
        }

    }

    public enum VerticalTextAligne
    {
        top,
        verticalCenter,
        bottom
    }
    public enum HorizontalTextAligne
    {
        left,
        harizontalCenter,
        right
    }


    public interface IGlobalParentUI
    {
        public Vector2 bgSize { get; set; }
        public Vector2 position { get; set; }
        public Container childrens { get; set; }
    }
}


