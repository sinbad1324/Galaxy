
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics;
using Galaxy.Events;
using Galaxy.Gui.GuiInterface;
using Galaxy.modules;
using Galaxy.workspace.Objects;
using LearnMatrix;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace Galaxy.workspace
{

    public class CollisionGroupe
    {
        private List<GlobalObject> objs;
        private Dictionary<string, List<GlobalObject>> globalLists;
        public string name;
        public CollisionGroupe(List<GlobalObject> objs, Dictionary<string,List<GlobalObject>> globalLists ,string name )
        {
            this.objs = objs;
            this.globalLists = globalLists;
        }

        public void DestroyGroupe()
        {

            globalLists.Remove(name);

        }
    }

    public interface IGlobalParentObj
    {
        public Vector2 bgSize { get; set; }
        public Vector2 position { get; set; }
        public ObjectContainer childrens { get; set; }
    }

    public class GlobalObject : IGlobalParentObj
    {
        private Vector2 positionP;
        private int _zIndex;
        private int _transparency;
        private bool touched;
        private int touchedCounter;
        private float _rotate;
        private bool _overflow;
        public bool overflow
        {
            get { return _overflow; }
            set { new RasterizerState { ScissorTestEnable = value , FillMode=GlobalParams.globalFillMode }; _overflow = value; }
        }
        public float rotation
        {
            get { return _rotate; }
            set { _rotate = MathHelper.ToRadians(value); }
        }
        public Vector2 bgSize { get; set; }
        public Vector2 SpriteSize { get; set; }
        public Vector2 position
        {
            get { return positionP; }
            set { positionP = (parent.position + value); }
        }
        public Humanoid humanoid;
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

                this.SpriteColor = new Color(SpriteColor.R, SpriteColor.G, SpriteColor.B, _transparency);
            }
        }
        public int zIndex
        {
            get { return _zIndex; }
            set
            {
                _zIndex = value;
                parent.childrens.container = parent.childrens.container.OrderBy(i => i.zIndex).ToList();
            }
        }
        protected RasterizerState rasterizerState;
        public ObjectContainer childrens { get; set; }
        public int id;
        public string name;
        public Color SpriteColor;
        public bool canCollide;
        public Rectangle Sprite;

        public int border;
        public Color borderColor;
        public bool isLoaded;
        public IGlobalParentObj parent;
        public Texture2D texture;

        //events
        public Events.BoundaryColl boundaryColl;
        public Vector2 boundaryCollEventLunched;
        public Cloision colision;
        public Dictionary<string, List<GlobalObject>> ColisionLists;

        //setters 
        public void Move(float x, float y, float marge = 2)
        {
            void LaunchAction(Vector2 pos)
            {
                if (pos != boundaryCollEventLunched)
                {
                    boundaryColl?.EventAction();
                    boundaryCollEventLunched = pos;
                }
            }
            float Xpos = position.X + x;
            float Ypos = position.Y + y;

            float right = Xpos + Sprite.Size.X;
            float bottom = Ypos;

            if (right + marge >= GlobalParams.WINDOW_WIDHT)
            {
                Xpos = GlobalParams.WINDOW_WIDHT - Sprite.Size.X;
                LaunchAction(new Vector2(Xpos, position.Y));
            }
            else if (Xpos - marge <= 0)
            {
                Xpos = marge;
                LaunchAction(new Vector2(Xpos, position.Y));
            }
            if (bottom + marge >= GlobalParams.WINDOW_HEIGTH)
            {
                Ypos = GlobalParams.WINDOW_HEIGTH;
                LaunchAction(new Vector2(position.X, Ypos));
            }
            else if ((Ypos - Sprite.Size.Y) - marge <= 0)
            {
                Ypos = Sprite.Size.Y;
                LaunchAction(new Vector2(position.X, Ypos));
            }
            position = new Vector2(Xpos, Ypos);
        }

        //Update
        private void updateCollision()
        {
            if (canCollide && colision!=null)
            {
                foreach (KeyValuePair<string , List<GlobalObject>> nameLists in ColisionLists)
                {
                    foreach (var obj2 in nameLists.Value)
                    {
                        if (texture == null ||colision == null)
                            return;

                        if (Sprite.Intersects(obj2.Sprite) && obj2.id != id && obj2.canCollide == true)
                        {
                            touchedCounter++;
                            if (touched == false)
                            {
                                colision.TouchedAction(obj2, nameLists.Key);
                                touched = true;
                            }
                            colision?.TouchingAction(obj2, nameLists.Key);
                        }
                    }
                    if (touchedCounter <= 0)
                        touched = false;
                    touchedCounter = 0;
                }
            }
        }

        public virtual void Update()
        {
            updateCollision();
            float Diff = new Vector2(GlobalParams.WINDOW_WIDHT, GlobalParams.WINDOW_HEIGTH).Length() - position.Length();

            if (Diff <= 1)
                boundaryCollEventLunched = new Vector2(0, 0);
            this.Sprite.Size = new Point((int)this.SpriteSize.X, (int)this.SpriteSize.Y);
            this.Sprite.Location = new Point((int)this.position.X, (int)this.position.Y);
        }
        public virtual void Draw() { }
        public virtual void LoadContent() { }

        //draw
        public void DrawChildren()
        {
            if (childrens == null ||childrens.container.Count <= 0) return;
            GlobalParams.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, this.rasterizerState);
            GlobalParams.spriteBatch.GraphicsDevice.ScissorRectangle = Sprite;
            childrens.Draw();
            GlobalParams.spriteBatch.End();

            for (int i = 0; i < childrens.container.Count; i++)
                childrens.container[i].DrawChildren();
        }
        // setter

        public CollisionGroupe CreateColisionGroupe(List<GlobalObject> objs , string collisionName)
        {
            ColisionLists[collisionName] =objs;
            return new CollisionGroupe(objs, ColisionLists , collisionName);
        }

        protected void SpriteInit(IGlobalParentObj parent)
        {
            childrens = new ObjectContainer(this);
            rasterizerState = new RasterizerState { ScissorTestEnable = true };
            ColisionLists = new Dictionary<string, List<GlobalObject>>();
            Random rnd = new Random();
            id = GlobalParams.Workspace.childrens.container.Count + 1;
            colision = new Cloision();
            touched = false;
            touchedCounter = 0;
            _zIndex = 1;
            this.parent = parent;
            boundaryCollEventLunched = new Vector2(0, 0);
            boundaryColl = new BoundaryColl();
            SpriteSize = new Vector2(50f, 50f);
            position = new Vector2(
                (float)rnd.Next((int)SpriteSize.X, GlobalParams.WINDOW_WIDHT),
                (float)rnd.Next((int)SpriteSize.Y, GlobalParams.WINDOW_HEIGTH)
            );
            SpriteColor = Color.White;
            transparency = 0;
            border = 0;
            borderColor = Color.Black;
            Sprite = new Rectangle(0, 0, (int)SpriteSize.X, (int)SpriteSize.Y);
            canCollide = true;
        }

        public void Bdestroy()
        {
            try
            {
                GlobalParams.Workspace.childrens.container.Remove(this);
                boundaryColl = null;
                colision = null;
                ColisionLists?.Clear();
                texture = null;
            }
            catch { }
            // parent = null;

        }

    }

}
