using Galaxy.Gui.GuiInterface;
using Galaxy.modules;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;


namespace Galaxy.Gui.Texts
{
    public class TextLable : GlobalUI, global::Galaxy.modules.IGlobal
    {
        /// <summary>
        ///   cette class c'est un textLable avec un backgroud
        ///   
        /// </summary>

        // Text

        //Private
        private SpriteFont Font;
        private string fontName;
        private Vector2 textPosition;
        private HorizontalTextAligne _horizontalAligne;
        private VerticalTextAligne _verticalAligne;

        //Public
        public Vector2 getTextSize { get { return Font == null ? Vector2.Zero : Font.MeasureString(text); } }
        public string text;
        public Color color;
        public int textTransparency;
        public float scale;
        public Vector2 textSize;
        public Vector2 getTextPosition { get { return this.textPosition; } }

        public HorizontalTextAligne horizontalAligne
        {
            get { return _horizontalAligne; }
            set
            {
                _horizontalAligne = value;
                TextToBackground();
            }
        }
        public VerticalTextAligne verticalAligne
        {
            get { return _verticalAligne; }
            set
            {
                _verticalAligne = value;
                TextToBackground();
            }
        }

        //Constructor
        public TextLable(ScreenGui screen, GlobalUI parent, string name, string text, string fontName)
        {
            this.screenGui = screen;
            this.text = text;
            this.name = name;
            this.fontName = fontName;
            scale = 1f;
            this.BgInit(parent);
            initialze();
        }

        public virtual void DestroyVariables()
        {
            base.Bdestroy();
            Font = null;
            fontName = "";
            textPosition = Vector2.Zero;
        }

        // Functions

        // Init
        //Private
        private void initialze()
        {
            this.color = Color.Black;
            this.isLoaded = true;


        }
        private void initTransparency()
        {
            if (textTransparency < 0)
                textTransparency = 0;
            if (textTransparency < 255)
                textTransparency = 255;
            color = new Color(color.R, color.G, color.B, textTransparency);
        }
        private void TextToBackground()
        {
            if (Font.MeasureString(this.text).X > 0 && Font.MeasureString(this.text).Y > 0)
            {
                textSize = new Vector2(Font.MeasureString(this.text).X * scale, Font.MeasureString(this.text).Y * scale);
                textPosition = this.position;
                //XXXXXXXXXXXXXXXXXXXXXXXX
                if (this.horizontalAligne == HorizontalTextAligne.left)
                    textPosition = new Vector2(this.position.X , textPosition.Y);
                else if (this.horizontalAligne == HorizontalTextAligne.right)
                    textPosition = new Vector2( ((textPosition.X + bgSize.X) - textSize.X), textPosition.Y);
                else if (this.horizontalAligne == HorizontalTextAligne.harizontalCenter)
                    textPosition = new Vector2((textPosition.X + bgSize.X / 2) - textSize.X / 2, textPosition.Y);
                // YYYYYYYYYYYYYYYYYYYYYYYYYYYYYY   
                if (this.verticalAligne == VerticalTextAligne.top)
                    textPosition = new Vector2(this.textPosition.X, position.Y );
                else if (this.verticalAligne == VerticalTextAligne.bottom)
                    textPosition = new Vector2(textPosition.X, (textPosition.Y + bgSize.Y) - textSize.Y);
                else if (this.verticalAligne == VerticalTextAligne.verticalCenter)
                    textPosition = new Vector2(textPosition.X, (textPosition.Y + bgSize.Y / 2) - textSize.Y / 2);
            }
        }

        //public
        //Load content
        public override void LoadContent(ContentManager content, GraphicsDevice device)
        {
            base.LoadContent(content, device);

            if (isLoaded)
            {
                this.texture = new Texture2D(device, 1, 1);
                texture.SetData<Color>(new Color[] { bgColor });
                this.Font = content.Load<SpriteFont>(this.fontName);
                isLoaded = false;
                horizontalAligne = HorizontalTextAligne.left;
                verticalAligne = VerticalTextAligne.top;
            }
        }
        //update
        public override void Update()
        {
            initTransparency();
            base.Update();
            //    <-|-[*]-|->
            TextToBackground();

        }
        //render
        public override void DrawChildren(SpriteBatch target)
        {

            target.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, this.rasterizerState);
            target.GraphicsDevice.ScissorRectangle = bg;
            if (Font != null)
                target.DrawString(this.Font, this.text, this.textPosition, this.color, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
            childrens.Draw(target);
            target.End();

            for (int i = 0; i < childrens.container.Count; i++)
                childrens.container[i].DrawChildren(target);
        }
        public override void Draw(SpriteBatch target)
        {
            if (Font != null)
            {
                target.Draw(texture, this.bg, bgColor);
            }
        }

        public virtual void Destroy()
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
    }
}
