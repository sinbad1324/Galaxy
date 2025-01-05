using Galaxy.Gui.Frames;
using Galaxy.modules;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;


namespace Galaxy.workspace.Objects
{
    public abstract class Humanoid : IGlobal
    {
        public string name;
        public Random random = new Random();
        public float maxHealth;
        protected int _lvl;
        protected double _exp;
        protected Workspace workspace;
        protected bool dbc;
        public int lvl { get { return _lvl; } }
        public float health;
        public double exp { get { return _exp; } }
        public int healthPlace;
        public float speed;
        public float MaxSpeed;
        protected Frame background;
        protected Frame forground;

        public Part charchter;
        protected string texture;
        protected void init(Workspace workspace, string name)
        {
            this.workspace = workspace;
            this.name = name;
            MaxSpeed = 10f;
            speed = MaxSpeed;
            dbc = false;
            this.health = 100f;
            this._lvl = 1;
            this._exp = 0;
            maxHealth = 100f;
            texture = "Avions1";
        }


        public virtual void Damage(float value)
        {
            health -= value;

        }

        public virtual void Initialize()
        {

            healthPlace = 20;
            charchter = this.workspace.childrens.addPart(this.name, texture);
            charchter.rotation = -90;
            charchter.position = new Vector2(200, 500);
            charchter.SpriteSize = new Vector2(100f, 100f);

            background = workspace.game.screenGui.childrens.addFrame(
                "backHealthBar" + name,
               (charchter.Sprite.Location.ToVector2() + charchter.SpriteSize / 2) + (new Vector2(0, 20) * healthPlace),
                new Vector2(100, 10),
                Color.White
            );

            forground = background.childrens.addFrame(
              "forHealthBar" + name,
              new Vector2(0, 0),
              new Vector2(0, 0),
              Color.Red
          );
            background.position = (charchter.Sprite.Location.ToVector2() + charchter.SpriteSize / 2) + (new Vector2(0, 20) * healthPlace);
            forground.position = new Vector2(0, 0);
            background.bgSize = new Vector2(100,10);
            forground.bgSize = new Vector2(100, 10);

            Console.WriteLine(forground.position);

        }

        public virtual void LoadContent(ContentManager content, GraphicsDevice device)
        {
            // charchter.LoadContent(content, device);
        }

        private void updateColor()
        {
            Color color = Color.Green;
            float heal = ((health / maxHealth) * 100);
            if (heal <= 70 && heal > 50)
            {
                color = Color.Yellow;
            }
            else if (heal <= 50 && heal > 30)
            {
                color = Color.Orange;

            }
            else if (heal < 30)
            {
                color = Color.Red;
            }
            forground.bgColor = color;
        }
        private void updatePos()
        {
            if (background != null && forground != null)
            {
                forground.bgSize = new Vector2((health / maxHealth) * 100, background.bgSize.Y);

                if (healthPlace < 0)
                    background.position = (charchter.Sprite.Location.ToVector2() - new Vector2(charchter.SpriteSize.X, 0)) + (new Vector2(0, healthPlace));

                else
                    background.position = (charchter.Sprite.Location.ToVector2()) + (new Vector2(0, healthPlace));

            }
        }
        public virtual void Update()
        {
            //charchter.Update();
                updatePos();
                updateColor();
            
        }
        public virtual void Draw(SpriteBatch target)
        {
            //charchter.Draw(target);    
        }

        public virtual void Destroy()
        {
           
            if (background != null)
                background.Destroy();

            if (forground != null)
                forground.Destroy();
                
            background = null;
            forground = null;
            this.charchter.Destroy();
        }
    }

}
