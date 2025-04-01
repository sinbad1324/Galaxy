using Galaxy.modules;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

using Microsoft.Xna.Framework;
using Galaxy.Events;

using System.Collections.Generic;
using System.Threading;
using LearnMatrix;




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
        public string name;

        public BoosterType type;
        public Part body;
        protected string texture;
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
            l.Add(GlobalParams.Workspace.player.charchter);
            body.CreateColisionGroupe(l , "WithPlayers");

            body.colision.touched += (object obj, TouchArgs e) =>
            {
                if (e.ActionName != "WithPlayers")
                    return;
                
                body.Destroy();
                GlobalParams.Workspace.boosterContainer.Remove(this);
                if (type == BoosterType.speed)
                {
                    int currentDelay = GlobalParams.Workspace.player.shootDelay;
                    float currentSpeed = GlobalParams.Workspace.player.speed;
                    GlobalParams.Workspace.player.charchter.SpriteColor = Color.Red;
                    GlobalParams.Workspace.player.shootDelay = 100;
                    GlobalParams.Workspace.player.speed = 30;
                    new Thread(() =>
                    {
                        Thread.Sleep(rd.Next(5, 9) * 1000);
                        GlobalParams.Workspace.player.shootDelay = currentDelay;
                        GlobalParams.Workspace.player.speed = currentSpeed;
                        GlobalParams.Workspace.player.charchter.SpriteColor = Color.White;
                    }).Start();

                }else if (type == BoosterType.health) {
                    GlobalParams.Workspace.player.health = GlobalParams.Workspace.player.maxHealth;
                }

            };
        }

        public booster( string name , Vector2 startPos )
        {
            rotate = 0;
            NewType();
            texture = GetTextureFromType();
            this.name = name;
            body = GlobalParams.Workspace.childrens.addPart( this.name, texture);
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
        public  void LoadContent() { }
        public  void Draw() { }

    }
}
