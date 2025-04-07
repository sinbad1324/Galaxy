using Galaxy.Gui.Frames;
using Galaxy.modules;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using Galaxy.Gui.Texts;
using LearnMatrix;
using Debuger.InstancePlugin.UI;


namespace Galaxy.workspace.Objects
{
    public abstract class Humanoid : IGlobal
    {
        public string name;
        public Random random = new Random();
        public float maxHealth;
        protected int _lvl;
        protected double _exp;
        protected bool dbc;
        public int lvl { get { return _lvl; } }
        public float health;
        public double exp { get { return _exp; } }

        public int healthPlace;
        public float speed;
        public float MaxSpeed;
        protected Frame background;
        protected Frame forground;
        protected TextLable nameText;
        public Part charchter;
        protected string texture;
        protected void init(string name)
        {
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
            charchter = GlobalParams.Workspace.childrens.addPart(this.name, texture);
            charchter.rotation = -90;
            charchter.position = new Vector2(200, 500);
            charchter.SpriteSize = new Vector2(100f, 100f);

            background = GlobalParams.Screen.childrens.addFrame(
                "backHealthBar" + name,
               Vector2.One * 200,
                new Vector2(200, 10),
                Color.White
            );
            forground = background.childrens.addFrame(
              "forHealthBar" + name,
              new Vector2(0, 0),
              new Vector2(0, 0),
              Color.Green
          );
            background.position = (charchter.Sprite.Location.ToVector2() + charchter.SpriteSize / 2) + (new Vector2(0, 20) * healthPlace);
            forground.position = new Vector2(0, 0);
            background.bgSize = new Vector2(100, 10);
            forground.bgSize = new Vector2(100, 10);

            nameText = GlobalParams.Screen.childrens.addTextLable("NameText"+ name, name);
            nameText.bgColor = Color.Transparent;
            nameText.background.SetNewTextureColor(Color.Transparent);
            nameText.border.SetNewTextureColor(Color.Transparent);
            nameText.color = Color.White;
            nameText.scale = 1.2f;


        }

        public virtual void LoadContent()
        {
            charchter.LoadContent();
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
                forground.position = Vector2.Zero;
            }
            if (nameText != null)
                nameText.position = (charchter.Sprite.Location.ToVector2() - new Vector2(charchter.SpriteSize.X / 2, 155));
        }
        public virtual void Update()
        {
            updatePos();
            updateColor();
        }
        public virtual void Draw()
        { }
        public virtual void Destroy()
        {
            if (background != null)
                background.Destroy();
            texture = null;

            if (forground != null)
                forground.Destroy();
            if (nameText != null) nameText.Destroy();
            background = null;
            forground = null;
            this.charchter.Destroy();
        }
    }

}
