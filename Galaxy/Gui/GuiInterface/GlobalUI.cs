
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Galaxy.Events;
using Galaxy.Gui.Frames;
using Galaxy.modules;
using LearnMatrix;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Microsoft.Xna.Framework.Input;


namespace Galaxy.Gui.GuiInterface
{
    public abstract class GlobalUI : global::Galaxy.modules.IGlobal
    {
        //Config
        private int _zIndex;
        private bool hoverIsLeave;
        private bool isHover;
        public string name;
        public bool isLoaded;
        public bool overflow { get { return background.overflow; } set { background.overflow = value; } }
        //Children
        public Container childrens { get; set; }
        //Position/color/size
        public Vector2 position
        {
            get { if (background == null) return Vector2.Zero; return background.position; }
            set
            {
                if (background == null) return;
                UpdateChildPos();
                background.position = (value + (parent != null ? parent.position : new Vector2(0, 0)));
                border.position = background.position - new Vector2(borderSize.left, borderSize.top);
            }
        }
        public Vector2 bgSize { get { if (background == null) return Vector2.Zero; return background.size; } set { if (background == null) return; background.size = value; UpdateBorder(); } }
        public Color bgColor { get { return background.color; } set { if (background == null) return; background.color = value;  background?.SetNewTextureColor(value); } }
        public Color borderColor { get { return border.color; } set { if (border == null) return; border.color = value; border?.SetNewTextureColor(value); } }

        //Parent
        public GlobalUI parent;
        // Index    
        public int zIndex
        {
            get { return _zIndex; }
            set
            {
                _zIndex = value;
                if (parent != null)
                    parent.UpdateChildrenIndex();
            }
        }
        //events
        public Events.BoundaryColl boundaryColl;
        public Events.HoverEvent hover;
        public Vector2 boundaryCollEventLunched;
        //Textures
        //BACKGROUND VARIABLES
        public RoundedRectangle background;
        //border
        public BorderSize borderSize;
        public RoundedRectangle border;
        private void UpdateBorder()
        {
            border.position = background.position - new Vector2(borderSize.left, borderSize.top);
            border.size = background.size + new Vector2(borderSize.left + borderSize.right, borderSize.top + borderSize.bottom);
        }
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
            if (Xpos + bgSize.X >= GlobalParams.WINDOW_WIDHT)
            {
                Xpos = (float)GlobalParams.WINDOW_WIDHT - bgSize.X;
                lunchAction(new Vector2(Xpos, position.Y));
            }
            else if (Xpos <= 0)
            {
                Xpos = 0f;
                lunchAction(new Vector2(Xpos, position.Y));
            }
            if (Ypos + bgSize.Y > GlobalParams.WINDOW_HEIGTH)
            {
                Ypos = (float)GlobalParams.WINDOW_HEIGTH - bgSize.Y;
                lunchAction(new Vector2(position.Y, Ypos));
            }
            else if (Ypos < 0)
            {
                Ypos = 0f;
                lunchAction(new Vector2(position.Y, Ypos));
            }
            position = new Vector2(Xpos, Ypos);
        }
        //Update
        public virtual void Update()
        {
            background?.Update();
            border?.Update();
            if (background != null && border != null && border.texture != null && background.texture != null && hover != null)
            {
                if (background.Contains(Mouse.GetState().Position.ToVector2()))
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
                float Diff = new Vector2(GlobalParams.WINDOW_WIDHT, GlobalParams.WINDOW_HEIGTH).Length() - position.Length();
                if (Diff <= 1)
                    boundaryCollEventLunched = new Vector2(0, 0);
                childrens.Update();
            }
        }
        public void UpdateChildrenIndex()
        {
            if (childrens.container.Count >= 2)
                childrens.container = childrens.container.OrderBy(i => i.zIndex).ToList();
        }
        //load content
        public virtual void LoadContent()
        {
            childrens.LoadContent();
        }
        //draw
        public virtual void Draw()
        {
            border?.Draw();
            background?.Draw();
        }
        public virtual void DrawChildren()
        {
            GlobalParams.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, background.rasterizerState);
            //GlobalParams.spriteBatch.GraphicsDevice.ScissorRectangle = background.scissorRectangle;
            childrens.Draw();
            GlobalParams.spriteBatch.End();

            for (int i = 0; i < childrens.container.Count; i++)
                childrens.container[i].DrawChildren();
        }
        //init
        private void InitializeBackground()
        {
            background = new RoundedRectangle(Vector2.Zero, Vector2.One * 50);
            background.Initialize();
            background.LoadContent();

            background.texture = new Texture2D(GlobalParams.Device, 1, 1);
            background.texture.SetData(new Color[] { bgColor });
            //background.SetNewTextureColor(background.texture);
        }
        private void InitializeBorder()
        {
            border = new RoundedRectangle(Vector2.Zero, Vector2.One * 50);
            border.Initialize();
            border.LoadContent();

            border.texture = new Texture2D(GlobalParams.Device, 1, 1);
            border.texture.SetData(new Color[] { Color.Blue });
            //border.SetNewTexture(border.texture);
            border.overflow = false;
            borderSize.changed += (sender, e) => UpdateBorder();

        }
        public virtual void Initialize()
        {
            childrens.Initialize();
        }
        protected void BgInit(GlobalUI parent)
        {
            this.parent = parent;
            isHover = false;
            boundaryCollEventLunched = new Vector2(0, 0);
            boundaryColl = new BoundaryColl();
            hover = new HoverEvent();
            InitializeBackground();
            InitializeBorder();
            bgSize = new Vector2(50f, 50f);
            position = new Vector2(50f, 50f);
            bgColor = Color.White;
            childrens = new Container(this);
            zIndex = 1;
        }
        private void UpdateChildPos()
        {
            if (childrens == null || childrens.container == null) return;
            int n = childrens.container.Count;
            if (n == 0)
                return;
            for (int i = 0; i < n - 1; i++)
                childrens.container[i].position += Vector2.Zero;            
            n = 0;
        }
        //destroy
        public virtual void Destroy()
        {
            background.Destroy();
            border.Destroy();
            boundaryColl = null;
            hover = null;
            background.texture = null;
            parent = null;
            for (int i = 0; i < childrens.container.Count; i++)
            {
                childrens.container[i].Destroy();
            }
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


