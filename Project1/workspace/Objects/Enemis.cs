using Galaxy.Events;
using Galaxy.VFX;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Galaxy.workspace.Objects
{
    public class Enemis : Humanoid
    {

        private List<Part> shootContainer;

        private bool deathed;

        public Enemis(Workspace workspace, string name)
        {
            base.init(workspace , name);
            speed = random.Next(1,5);
            base.texture = "Enemis";
            healthPlace = -50;
            shootContainer = new List<Part>();
            deathed = false;
        }
        
        public override void Initialize()
        {
            base.Initialize();
            healthPlace = -1;
            bool toucheDbc = false;
            charchter.bgSize = new Vector2(1,1);
            charchter.rotation = 180;
            charchter.position = new Vector2((float)random.Next(0,workspace.screenWidth) , (float)random.Next((int)charchter.bgSize.Y+70,random.Next(150,450)));

            charchter.boundaryColl.Touched += (object obj, EventArgs ev) => {
                speed *= -1;
                Thread.Sleep(500);
            };
            charchter.humanoid = this;
            List < GlobalObject >l= new List<GlobalObject>();
            l.Add(workspace.player.charchter);
            charchter.CreateColisionGroupe(l);
            charchter.colision.touched += (object obj, TouchArgs e) => {
                if (e.obj.humanoid != null && workspace.EnemisGroup.Contains(e.obj.humanoid) == null)
                {
                    e.obj.humanoid.Damage(10);
                    Flipbook animation = this.workspace.childrens.addFlipbook( "ghost", 8);
                    animation.position = charchter.position;
                    animation.interval = 0;
                    animation.Play();
                    animation.Finish.finished += (object obj, EventArgs args) =>
                    {animation.Destroy();};
                }
                else if (workspace.EnemisGroup.Contains(e.obj.humanoid) && !toucheDbc)
                {
                    int sp = 1;
                    speed *= -sp;
                    e.obj.humanoid.speed *= -sp;
                    toucheDbc = true;
                    new Thread(() =>
                    {
                        Thread.Sleep(1000);
                        speed /= sp;
                        e.obj.humanoid.speed /= sp;
                        toucheDbc = false;
                    }).Start();
                }

            };

            List<GlobalObject> Friends = new List<GlobalObject>();
            for (int i = 0; i < workspace.EnemisGroup.Count; i++)
            {
                Friends.Add(workspace.EnemisGroup[i].charchter);
            }
            charchter.CreateColisionGroupe(Friends);

        }

        private void Shoot()
        {


            if (!dbc)
            {

                dbc = true;
                new Thread(() => { Thread.Sleep(random.Next(5, 10)*100); dbc = false; }).Start();

                Part newshoot = workspace.childrens.addPart(
                     "shot_" + name + "_" + shootContainer.Count,
                     "fireShooter1"
                 );
                newshoot.position = charchter.position + new Vector2(random.Next(-20, 20), random.Next(30,50));
                newshoot.rotation = -45;
                shootContainer.Add(newshoot);
                new Thread(() =>
                {
                    for (int i = 0; i < 100; i++)
                    {
                        Thread.Sleep(10);
                        newshoot.Move(0, 8);
                    }
                }).Start();
   
                List<GlobalObject> l = new List<GlobalObject>();
                l.Add(workspace.player.charchter);
                newshoot.CreateColisionGroupe(l);

                newshoot.colision.touched += (object obj, TouchArgs e) =>
                {
                    newshoot.Destroy();
                    shootContainer.Remove(newshoot);
                    e.obj.humanoid.Damage(10);

                    Flipbook animation = this.workspace.childrens.addFlipbook( "Explosion", 8);
                    animation.position = newshoot.position - new Vector2(60, 60);
                    animation.interval = 0;
                    animation.Play();
                    animation.Finish.finished += (object obj, EventArgs args) =>
                    { animation.Destroy(); };
                };

                newshoot.boundaryColl.Touched += (object obj, EventArgs e) => {
                    newshoot.Destroy();
                    shootContainer.Remove(newshoot);
                };
            }
        }

        public override void Damage(float value)
        {
            base.Damage(value);
            if (health <= 0)
            {
                this.Destroy();
                workspace.EnemisGroup.Remove(this);
            }
        }

        private void Death()
        {
            Console.WriteLine("death");
            if (random.Next(0,5) == 2)
            {
   
               workspace.boosterContainer.Add(new booster(workspace, "speed" + workspace.boosterContainer.Count , charchter.position));

            }
        }

        private void changePos()
        {
            // charchter.Move(speed ,0);
            if ((charchter.position.X + charchter.bgSize.X) >= workspace.screenWidth || charchter.Sprite.Location.X <= 0)
            {
                speed *= -1;
            }
          

            charchter.position += new Vector2(speed,0);
        }

        public override void Destroy()
        {
            Death();

            base.Destroy();
            workspace.EnemisGroup.Remove(this);
        }

        public override void Update()
        {
            base.Update();
            changePos();
            Shoot();

            if (!deathed && health <= 0)
            {
                 deathed = true;
                Death();
            }
        }
    }
}
