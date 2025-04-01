using Galaxy.Events;
using Galaxy.Gui;
using Galaxy.Gui.Frames;
using Galaxy.Gui.GuiInterface;
using Galaxy.Gui.Images;
using Galaxy.Gui.Texts;
using Galaxy.modules;
using Galaxy.VFX;
using Galaxy.workspace.Objects;
using LearnMatrix;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Threading;


namespace Galaxy.workspace
{
    public  class Workspace : global::Galaxy.modules.IGlobal, IGlobalParentObj
    {
        public List<Enemis> EnemisGroup;
        public int maxEnemies;
        public int max_raide;

        public int minEnemies;
        //Private
        public Random rd = new Random();
        public ObjectContainer childrens { get; set; }
        public Vector2 bgSize { get; set; }
        public Vector2 position { get; set; }

        public Player player;
        public List<booster> boosterContainer;
        private bool dbcBoost;

        public Workspace()
        {
            childrens = new (this);
            bgSize = new Vector2(GlobalParams.WINDOW_WIDHT, GlobalParams.WINDOW_HEIGTH);
            position = new Vector2(0f, 0f);
            player = new Player( "player1");
            dbcBoost = true;
            boosterContainer = new List<booster>();
            EnemisGroup = new List<Enemis>();
            maxEnemies = 8;
            minEnemies = 2;
            max_raide = 10;
        }
        public List<GlobalObject> getCharacterEnemis()
        {
            List<GlobalObject> list = new List<GlobalObject>();

            for (int i = 0; i < EnemisGroup.Count; i++)
            {
                list.Add(EnemisGroup[i].charchter);
            }
            return list;
        }

        private void UpdatePlayer()
        {
            player.shootDelay -= (int)(player.shootDelay / 5);
        }

        private void UpdateEnemis()
        {
            for (int i = 0; i < EnemisGroup.Count; i++)
            {
                EnemisGroup[i].maxHealth += 10f;
                EnemisGroup[i].health = EnemisGroup[i].maxHealth;
            }
        }

        private void RaidNotif()
        {
            //TextLable showRaids = GlobalParams.Screen.childrens.FindChildren<TextLable>("showRaids");
            //if (showRaids != null)
            //{
            //    showRaids.text = "raid " + GlobalParams.raid;
            //}
            //int totalTime = 100;
            //TextLable vg = GlobalParams.Screen.childrens.addTextLable("Vagues", "Vaguge numero " + GlobalParams.raid);
            //vg.bgSize = new Vector2(300, 50);
            //vg.position = new Vector2(GlobalParams.WINDOW_WIDHT / 2 - 150, -150);
            //vg.color = Color.White;
            //vg.bgColor = Color.Transparent;
            //vg.horizontalAligne = HorizontalTextAligne.harizontalCenter;
            //vg.verticalAligne = VerticalTextAligne.verticalCenter;
            //new Thread(() =>
            //{
            //    Thread.Sleep(1000);
            //    for (int i = 0; i < totalTime; i++)
            //    {
            //        Thread.Sleep(10);
            //        vg.position = Utils.GetDirectionSpeed(vg.position, new Vector2(GlobalParams.WINDOW_WIDHT / 2 - 150, GlobalParams.WINDOW_HEIGTH / 2 - 150), (float)totalTime, (float)i);
            //    }
            //    vg.Destroy();
            //}).Start();
        }

        private void UpdateRaide()
        {
            if (EnemisGroup.Count <= 0 && GlobalParams.raid <= max_raide)
            {
                GlobalParams.raid++;
                RaidNotif();
                int enemyCount = Math.Clamp(minEnemies + (GlobalParams.raid - 1) * 2, minEnemies, maxEnemies);
                for (int i = 0; i < enemyCount; i++)
                {
                    CreateEnemis();
                }
                UpdatePlayer();
                UpdateEnemis();
            }
        }

        private void CreateEnemis()
        {
            Enemis en = new Enemis("Enemis" + EnemisGroup.Count);
            en.Initialize();
            en.LoadContent();
            EnemisGroup.Add(en);

        }
        protected void CreateBoosts()
        {
            if (dbcBoost)
            {
                dbcBoost = false;
                new Thread(() =>
                {
                    Thread.Sleep(rd.Next(10, 20) * 3000);
                    boosterContainer.Add(new booster( "speed" + boosterContainer.Count, new Vector2(rd.Next(0, GlobalParams.WINDOW_WIDHT), -40)));
                    dbcBoost = true;
                }).Start();
            }
        }

        public virtual void Initialize()
        {
            childrens.Initialize();
            player.Initialize();
            Part backguroundImage = childrens.addPart("Backguround", "background");
            backguroundImage.SpriteSize = new Vector2(GlobalParams.WINDOW_WIDHT, GlobalParams.WINDOW_HEIGTH);
            backguroundImage.position = new Vector2(0, 0);

        }
        public virtual void LoadContent()
        {
            childrens.LoadContent();
            player.LoadContent();
        }
        //update
        public virtual void Update()
        {
            childrens.Update();
            player.Update();
            CreateBoosts();
            UpdateRaide();
            for (int i = 0; i < EnemisGroup.Count; i++)
                EnemisGroup[i].Update();
            for (int i = 0; i < boosterContainer.Count; i++)
                boosterContainer[i].Update();
        }
        //render
        public virtual void Draw()
        {
            GlobalParams.spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null,null);
            childrens.Draw();
            GlobalParams.spriteBatch.End();
            for (int i = 0; i < childrens.container.Count; i++)
            {
                if (childrens.container[i] != null)
                {
                childrens.container[i].DrawChildren();

                }
            }
        }
    
    }
}
