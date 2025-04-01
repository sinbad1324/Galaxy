using Galaxy.Events;
using Galaxy.Gui.Frames;
using Galaxy.modules;
using Galaxy.VFX;
using LearnMatrix;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Threading;


namespace Galaxy.workspace.Objects
{

    public class Player : Humanoid
    {
        public event EventHandler death; 
        public int damage;
        public int points;
        private List<Part> shootContainer;
        public int shootDelay;
        public Player(string name)
        {
            base.init(name);
            shootContainer = new List<Part>();
            shootDelay = 400;
            maxHealth = 1000;
            health = maxHealth;
            speed = 10;
            MaxSpeed = 10;
            damage = 35;
        }
        private void LunchDeath()
        {
            if (death != null)
            {
                death(this, EventArgs.Empty);
            }
        }
        private void Shoot()
        {
            if (!dbc)
            {
                dbc = true;
                new Thread(() => { Thread.Sleep(shootDelay); dbc = false; }).Start();

                Part newshoot = GlobalParams.Workspace.childrens.addPart(
                     "shot_" + name + "_" + shootContainer.Count,
                     "fireShooter1"
                 );
                newshoot.CreateColisionGroupe(GlobalParams.Workspace.getCharacterEnemis() , "Enemis");
                newshoot.position = charchter.position + new Vector2(random.Next(-30, 150), -50);
                newshoot.bgSize = new Vector2(2, 2);
                int dm = damage;
                Vector2 chPos = charchter.position;
                newshoot.rotation = 135;
                shootContainer.Add(newshoot);
                new Thread(() =>
                {
                    for (int i = 0; i < 100; i++)
                    {
                        Thread.Sleep(10);
                        newshoot.Move(0, -10);
                    }
                }).Start();

                newshoot.colision.touched += (object obj, TouchArgs e) =>
                {
                    if (e.ActionName != "Enemis")
                        return;
                    if ((e.obj.humanoid.charchter.position - chPos).Length() <= 70f)
                    {
                        dm += 20;
                    }
                    newshoot.Destroy();
                    shootContainer.Remove(newshoot);
                    if (e.obj.humanoid != null)
                        e.obj.humanoid.Damage(dm);
                    points++;

                    Flipbook animation = GlobalParams.Workspace.childrens.addFlipbook("Explosion", 8);
                    animation.position = newshoot.position - new Vector2(20, 100);
                    animation.interval = 0;
                    animation.Play();
                    animation.Finish.finished += (object obj, EventArgs args) =>
                    { animation.Destroy(); };
                };

                newshoot.boundaryColl.Touched += (object obj, EventArgs e) =>
                {
                    newshoot.Destroy();
                    shootContainer.Remove(newshoot);
                };
            }
        }
        private void KeyPressing()
        {
            KeyboardState keystate = Keyboard.GetState();

            bool w = keystate.IsKeyDown(Keys.W) || keystate.IsKeyDown(Keys.Up);
            bool s = keystate.IsKeyDown(Keys.S) || keystate.IsKeyDown(Keys.Down);
            bool d = keystate.IsKeyDown(Keys.D) || keystate.IsKeyDown(Keys.Right);
            bool a = keystate.IsKeyDown(Keys.A) || keystate.IsKeyDown(Keys.Left);

            if ((w && d) || (s && d) || (w && a) || (s && a))
            {
                speed = MaxSpeed / 2;
            }
            else
            {
                speed = MaxSpeed;
            }

            if (w)
                charchter.Move(0, -speed);
            if (s)
                charchter.Move(0, speed);

            if (a)
                charchter.Move(-speed, 0);
            if (d)
                charchter.Move(speed, 0);

            if (keystate.IsKeyDown(Keys.Space))
                Shoot();
        }
        public override void Initialize()
        {
            base.Initialize();
            charchter.humanoid = this;
        }
        public override void LoadContent() { }    
        public override void Damage(float value)
        {
            base.Damage(value);
            if (health <= 0)
                LunchDeath();
        }
        public override void Update()
        {
            base.Update();
            KeyPressing();
        }

        public override void Draw() { base.Draw(); }

    }
}
