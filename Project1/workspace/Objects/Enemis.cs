using Galaxy.Events;
using Galaxy.VFX;
using LearnMatrix;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Galaxy.workspace.Objects
{
    public class Enemis : Humanoid
    {
        private List<Part> shootContainer;
        private bool deathed;
        public Enemis(string name)
        {
            base.init(name);
            speed = random.Next(1, 5);
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
            charchter.bgSize = new Vector2(1, 1);
            charchter.rotation = 180;
            charchter.position = new Vector2((float)random.Next(0, GlobalParams.WINDOW_WIDHT), (float)random.Next((int)charchter.bgSize.Y + 70, random.Next(150, 450)));

            charchter.boundaryColl.Touched += (object obj, EventArgs ev) =>
            {
                speed *= -1;
                Thread.Sleep(500);
            };
            charchter.humanoid = this;
            List<GlobalObject> l = new List<GlobalObject>();
            l.Add(GlobalParams.Workspace.player.charchter);
            charchter.CreateColisionGroupe(l, "WithPlayer"); //CREATE COLLISION NAME  
            charchter.colision.touched += (object obj, TouchArgs e) =>
            {
                if (e.ActionName == "WithPlayer")
                {
                    if (e.obj.humanoid != null)
                    {
                        if (e.obj != GlobalParams.Workspace.player.charchter)
                            return;
                        if (GlobalParams.Workspace.childrens.FindChildren<Flipbook>("playerGhost") != null)
                            return;
                        e.obj.humanoid.Damage(10);
                        Flipbook animation = GlobalParams.Workspace.childrens.addFlipbook("ghost", 8, "playerGhost");
                        animation.position = charchter.position;
                        animation.interval = 0;
                        animation.Play();
                        animation.Finish.finished += (object obj, EventArgs args) =>
                        { animation.Destroy(); };
                    }
                    else if (GlobalParams.Workspace.EnemisGroup.Contains(e.obj.humanoid) && !toucheDbc)
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

                }
                List<GlobalObject> Friends = new List<GlobalObject>();
                for (int i = 0; i < GlobalParams.Workspace.EnemisGroup.Count; i++)
                    Friends.Add(GlobalParams.Workspace.EnemisGroup[i].charchter);

                charchter.CreateColisionGroupe(Friends, "WithFriends");
            };
        }
        private void Shoot()
        {
            if (!dbc)
            {
                dbc = true;
                new Thread(() => { Thread.Sleep(random.Next(5, 10) * 100); dbc = false; }).Start();
                Part newshoot = GlobalParams.Workspace.childrens.addPart(
                     "shot_" + name + "_" + shootContainer.Count,
                     "fireShooter1"
                 );
                newshoot.position = charchter.position + new Vector2(random.Next(-20, 20), random.Next(30, 50));
                newshoot.rotation = -45;
                shootContainer.Add(newshoot);
                new Thread(() =>
                {
                    for (int i = 0; i < GlobalParams.WINDOW_HEIGTH; i++)
                    {
                        if (newshoot == null)
                            break;
                        Thread.Sleep(10);
                        newshoot.Move(0, 8);
                    }
                    if (newshoot != null)
                    {
                        newshoot.Destroy();
                        shootContainer.Remove(newshoot);
                        newshoot = null;
                    }
                }).Start();

                List<GlobalObject> l = new List<GlobalObject>();
                l.Add(GlobalParams.Workspace.player.charchter);
                newshoot.CreateColisionGroupe(l, "FirePlayer");
                newshoot.colision.touched += (object obj, TouchArgs e) =>
                {
                    if (e.ActionName == "FirePlayer")
                    {
                        newshoot.Destroy();
                        shootContainer.Remove(newshoot);
                        e.obj.humanoid.Damage(10);

                        Flipbook animation = GlobalParams.Workspace.childrens.addFlipbook("Explosion", 8);
                        animation.position = newshoot.position - new Vector2(60, 60);
                        animation.interval = 0;
                        animation.Play();
                        animation.Finish.finished += (object obj, EventArgs args) =>
                        { animation.Destroy(); };
                    }
                };

                newshoot.boundaryColl.Touched += (object obj, EventArgs e) =>
                {
                    newshoot.Destroy();
                    shootContainer.Remove(newshoot);
                    newshoot = null;
                };
            }
        }
        public override void Damage(float value)
        {
            base.Damage(value);
            if (health <= 0)
            {
                this.Destroy();
                GlobalParams.Workspace.EnemisGroup.Remove(this);
            }
        }
        private void Death()
        {
            if (deathed)
                return;
            if (random.Next(0, 5) == 2)
                GlobalParams.Workspace.boosterContainer.Add(new booster("booster" + GlobalParams.Workspace.boosterContainer.Count, charchter.position));
        }
        private void changePos()
        {
            // charchter.Move(speed ,0);
            if ((charchter.position.X + charchter.bgSize.X) >= GlobalParams.WINDOW_WIDHT || charchter.Sprite.Location.X <= 0)
                speed *= -1;
            charchter.position += new Vector2(speed, 0);
        }
        public override void Destroy()
        {
            Death();
            base.Destroy();

            GlobalParams.Workspace.EnemisGroup.Remove(this);
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
