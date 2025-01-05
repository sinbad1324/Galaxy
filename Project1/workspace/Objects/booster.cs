using Galaxy.modules;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

using Microsoft.Xna.Framework;
using Galaxy.Events;

using System.Collections.Generic;
using System.Threading;




namespace Galaxy.workspace.Objects
{
    public enum BoosterType 
    {
        speed,
        health
    }
    public class booster :IGlobal
    {
        private Random rd = new Random();
        protected Workspace workspace;
        public string name;

        public BoosterType type;
        public Part body;
        protected string texture;
        private float speed;
        private float rotate;

        private BoosterType NewType()
        {
            type = (BoosterType)rd.Next(0,2);

            return type;
        } 

       private  string GetTextureFromType()
        {
            return type.ToString()+"Boost";
        }

        private void Colision() {
            List<GlobalObject> l = new List<GlobalObject>();
            l.Add(workspace.player.charchter);
            body.CreateColisionGroupe(l);

            body.colision.touched += (object obj, TouchArgs e) =>
            {

                body.Destroy();
                workspace.boosterContainer.Remove(this);
                if (type == BoosterType.speed)
                {
                    int currentDelay = workspace.player.shootDelay;
                    float currentSpeed = workspace.player.speed;
                    workspace.player.charchter.SpriteColor = Color.Red;
                    workspace.player.shootDelay = 100;
                    workspace.player.speed = 30;
                    new Thread(() =>
                    {
                        Thread.Sleep(rd.Next(5, 9) * 1000);
                        workspace.player.shootDelay = currentDelay;
                        workspace.player.speed = currentSpeed;
                        workspace.player.charchter.SpriteColor = Color.White;
                    }).Start();

                }else if (type == BoosterType.health) {
                    workspace.player.health = workspace.player.maxHealth;
                }

            };
        }

        public booster(Workspace workspace, string name , Vector2 startPos )
        {
            rotate = 0;
            NewType();
            texture = GetTextureFromType();
            this.workspace = workspace;
            this.name = name;
            body = this.workspace.childrens.addPart( this.name, texture);
            body.position = startPos; 
            body.SpriteSize = new Vector2(50f, 50f);
            Colision();

        }

        public  void Update()
        {
            body.position += new Vector2(0,1);
            rotate++;
            body.rotation = rotate;
        }
        public  void LoadContent(ContentManager content, GraphicsDevice device) { }
        public  void Draw(SpriteBatch target) { }

    }
}
