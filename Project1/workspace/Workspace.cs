using Galaxy.Events;
using Galaxy.Gui;
using Galaxy.Gui.Frames;
using Galaxy.Gui.GuiInterface;
using Galaxy.Gui.Images;
using Galaxy.Gui.Texts;
using Galaxy.modules;
using Galaxy.VFX;
using Galaxy.workspace.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Threading;


namespace Galaxy.workspace
{
    public class Workspace : global::Galaxy.modules.IGlobal, IGlobalParentObj
    {
        public Galaxy game;
        public Random rd = new Random();
        public ObjectContainer childrens { get; set; }
        public Vector2 bgSize { get; set; }
        public Vector2 position { get; set; }
        public int screenWidth { get; }
        public int screenHeight { get; }
        public GameWindow window;
        public Player player;
        public List<Enemis> EnemisGroup;
        public int maxEnemies;
        public int max_raide;
        public int minEnemies;
        private bool dbcBoost;

        public List<booster> boosterContainer;

        private void CreateEnemis()
        {
            Enemis en = new Enemis(this, "Enemis" + EnemisGroup.Count);
            en.Initialize();
            en.LoadContent(game.Content, game.GraphicsDevice);
            EnemisGroup.Add(en);

        }

      
        public Workspace(int _screenWidth, int _screenHeight, Galaxy game)
        {
            childrens = new  (this , this);
            boosterContainer = new List<booster>();
            this.screenWidth = _screenWidth;
            this.screenHeight = _screenHeight;
            this.game = game;
            this.window = game.Window;
            bgSize = new Vector2(screenWidth, screenHeight);
            position = new Vector2(0f, 0f);
            player = new Player(this, "player1");
            EnemisGroup = new List<Enemis>();
            maxEnemies = 8;
            minEnemies = 2;
            max_raide = 10;
            dbcBoost = true;
        }


        private void CreateBoosts()
        {
            if (dbcBoost)
            {
                dbcBoost = false;
                new Thread(() =>
                {
                    Thread.Sleep(rd.Next(10, 20) * 1000);
                    dbcBoost = true;
                }).Start();
                boosterContainer.Add(new booster(this, "speed" + boosterContainer.Count , new Vector2(rd.Next(0, screenWidth), -40)));
            }
        }


        //Accessors

        public List<GlobalObject> getCharacterEnemis()
        {
            List<GlobalObject> list = new List<GlobalObject>();

            for (int i = 0; i < EnemisGroup.Count; i++)
            {
                list.Add(EnemisGroup[i].charchter);
            }



            return list;
        }
        //Initialze
        
        public void Initialize()
        {
            childrens.Initialize();
            player.Initialize();


            Part backguroundImage = childrens.addPart("Backguround", "background");
            backguroundImage.SpriteSize = new Vector2(screenWidth, screenHeight);
            backguroundImage.position = new Vector2(0, 0);
        }
        public void LoadContent(ContentManager content, GraphicsDevice device)
        {
            childrens.LoadContent(content, device);
            player.LoadContent(content, device);
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
            TextLable showRaids = game.screenGui.childrens.FindChildren<TextLable>("showRaids");
            if (showRaids != null)
            {
                showRaids.text = "raid " + game.raid;
            }
            int totalTime = 100;
            TextLable vg = game.screenGui.childrens.addTextLable( "Vagues", "Vaguge numero " + game.raid);
            vg.bgSize = new Vector2(300, 50);
            vg.position = new Vector2(screenWidth / 2 - 150, -150);
            vg.color = Color.White;
            vg.bgColor = Color.Transparent;
            vg.horizontalAligne = HorizontalTextAligne.harizontalCenter;
            vg.verticalAligne = VerticalTextAligne.verticalCenter;
            new Thread(() =>
            {
                Thread.Sleep(1000);
                for (int i = 0; i < totalTime; i++)
                {
                    Thread.Sleep(10);
                    vg.position = Utils.GetDirectionSpeed(vg.position, new Vector2(screenWidth / 2 - 150, screenHeight / 2 - 150), (float)totalTime, (float)i);
                }
                vg.Destroy();
            }).Start();
        }

        private void UpdateRaide()
        {
            if (EnemisGroup.Count <= 0 && game.raid <= max_raide)
            {
                game.raid++;
                RaidNotif();
                int enemyCount = Math.Clamp(minEnemies + (game.raid - 1) * 2, minEnemies, maxEnemies);
                for (int i = 0; i < enemyCount; i++)
                {
                    CreateEnemis();
                }
                //UpdatePlayer();
                //UpdateEnemis();
            }


        }


        //update
        public void Update()
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
        public void Draw(SpriteBatch target)
        {
            target.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            childrens.Draw(target);
            target.End();

            for (int i = 0; i < childrens.container.Count; i++)
                childrens.container[i].DrawChildren(target);
        }
    }
}
