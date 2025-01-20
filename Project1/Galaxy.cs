using Galaxy.Events;
using Galaxy.Gui;
using Galaxy.Gui.Frames;
using Galaxy.Gui.GuiInterface;
using Galaxy.Gui.Images;
using Galaxy.Gui.Texts;
using Galaxy.modules;
using Galaxy.workspace;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.ComponentModel;
using System.Threading;
using System.Xml.Linq;


namespace Galaxy
{
    public class Galaxy : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private bool _started;
        public ScreenGui screenGui;
        public Workspace workspace;
        public Vector2 size;

        public string PlayerName;
        public int reStarted;
        public int raid;
        public bool started { get { return _started; } }

        private bool _IsPlaying;

        private bool _pausedWorkspace;
        public bool IsPlaying
        {
            get { return _IsPlaying; }
        }

        public Galaxy()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.IsFullScreen = false;
            _graphics.PreferredBackBufferWidth = 1500;
            _graphics.PreferredBackBufferHeight = 1000;
            _graphics.ApplyChanges();
            screenGui = new ScreenGui(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight, Window);
            workspace = new Workspace(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight, this);
            _started = false;
            reStarted = 0;
            raid = 0;
            _IsPlaying = true;
            _pausedWorkspace = false;

        }


        private void StopWorkspace()
        {
            //for (int i = 0; i < workspace.EnemisGroup.Count; i++)
            //    workspace.EnemisGroup[i].Destroy();
            //workspace.EnemisGroup.Clear();
            //workspace.boosterContainer.Clear();
            _IsPlaying = false;
        }


        private void Start()
        {
            TextLable showRaids = screenGui.childrens.addTextLable("showRaids", "raid " + this.raid);
            showRaids.bgSize = new Vector2(100, 30);
            showRaids.position = new Vector2(screenGui.screenWidth / 2 - 50, 0);
            showRaids.bgColor = Color.Transparent;
            _started = true;
            _IsPlaying = true;

        }

        private void startBtn()
        {

            TextButton StartBtn = screenGui.childrens.addTextButton("StartBtn", "Start");
            StartBtn.color = Color.Black;
            StartBtn.bgColor = Color.White;
            StartBtn.bgSize = new Vector2(200f, 50f);
            StartBtn.position = new Vector2((screenGui.screenWidth / 2) - 100, screenGui.screenHeight / 2 + 30);
            StartBtn.verticalAligne = VerticalTextAligne.verticalCenter;
            StartBtn.horizontalAligne = HorizontalTextAligne.harizontalCenter;

            float scale = 1.2f;
            StartBtn.hover.Hover += (object obj, HoverEventArgs e) =>
            {
                scale = 1.2f;
                StartBtn.bgSize = new Vector2(200f * scale, 50f * scale);
                StartBtn.position = new Vector2((screenGui.screenWidth / 2) - (100f * scale), screenGui.screenHeight / 2 + 30 * scale);
            };
            StartBtn.hover.HoverLeave += (object obj, EventArgs e) =>
            {
                scale = 1f;
                StartBtn.bgSize = new Vector2(200f * scale, 50f * scale);
                StartBtn.position = new Vector2((screenGui.screenWidth / 2) - 100, screenGui.screenHeight / 2 + 30 * scale);
            };

            StartBtn.Button1Click.UnPressed += (object obj, EventArgs e) =>
            {
                scale = 1f;
                StartBtn.bgSize = new Vector2(200f * scale, 50f * scale);
                StartBtn.position = new Vector2((screenGui.screenWidth / 2) - 100, screenGui.screenHeight / 2 + 30 * scale);
                //if (NameInput.text != "")
                //{
                _started = true;
                //  NameInput.Destroy();
                reStarted = 5;
                Start();
                _pausedWorkspace = true;
                StartBtn.Destroy();

                //}

            };

        }

        private void GameVerify()
        {

            if (!IsPlaying)
            {
                if (reStarted <= 0)
                {
                    StopWorkspace();
                    return;
                }
                TextButton btn = screenGui.childrens.addTextButton("RestartButton", "Re start");
                btn.bgSize = new Vector2(300, 50);
                btn.position = new Vector2(workspace.screenWidth / 2 - 150, workspace.screenHeight / 2 - 25);
                btn.color = Color.Black;
                btn.horizontalAligne = HorizontalTextAligne.harizontalCenter;
                btn.verticalAligne = VerticalTextAligne.verticalCenter;
                btn.Button1Click.Pressed += (object obj, EventArgs e) =>
                {
                    _IsPlaying = true;
                    btn.Destroy();
                };

            }
            workspace.player.death += (object obj, EventArgs e) =>
            {
                reStarted=-1;               
                if (reStarted <= 0)
                {
                    StopWorkspace();
                    return;
                }
                ImageLable img = screenGui.childrens.addImageLable("Tete de mort", "TDM2");
                img.bgSize = new Vector2(300, 300);
                img.position = new Vector2(workspace.screenWidth / 2 - 150, -100);

                int totalTime = 100;
                new Thread(() =>
                {
                    for (int i = 0; i < totalTime; i++)
                    {
                        Thread.Sleep(10);
                        img.position = Utils.GetDirectionSpeed(img.position, new Vector2(workspace.screenWidth / 2 - 150, workspace.screenHeight / 2 - 150), (float)totalTime, (float)i);

                    }
                }).Start();

                new Thread(() =>
                {
                    Thread.Sleep(10 * totalTime);
                    TextButton btn = screenGui.childrens.addTextButton("RestartButton", "Re start");
                    btn.bgSize = new Vector2(300, 50);
                    btn.position = new Vector2(workspace.screenWidth / 2 - 150, workspace.screenHeight / 2 - 25);
                    btn.color = Color.Black;
                    btn.horizontalAligne = HorizontalTextAligne.harizontalCenter;
                    btn.verticalAligne = VerticalTextAligne.verticalCenter;

                    Frame HealthContainer = screenGui.childrens.FindChildren<Frame>("HealthContainer");
                    if (HealthContainer != null)
                    {
                        ImageLable lasthealth = HealthContainer.childrens.FindChildren<ImageLable>("health" + (reStarted));
                        if (lasthealth != null) lasthealth.Destroy();
                    }
                    btn.Button1Click.Pressed += (object obj, EventArgs e) =>
                    {
                        workspace.player.health = workspace.player.maxHealth;
                        workspace.maxEnemies = 5;
                        btn.Destroy();
                        btn = null;
                        ImageLable img = screenGui.childrens.FindChildren<ImageLable>("Tete de mort");
                        if (img != null)
                            img.Destroy();

                        for (int i = 0; i < workspace.EnemisGroup.Count; i++)
                        {
                            workspace.EnemisGroup[i].health = workspace.EnemisGroup[i].maxHealth;
                        }
                        _IsPlaying = true;
                    };
                }).Start();
                _IsPlaying = false;
            };

        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();

            screenGui.Initialize();
            workspace.Initialize();

            GameVerify();



            startBtn();
        }

        protected override void LoadContent()
        {

            _spriteBatch = new SpriteBatch(GraphicsDevice);
            //1 workspace
            workspace.LoadContent(Content, GraphicsDevice);
            // 2 workspace
            screenGui.LoadContent(Content, GraphicsDevice);
            // TODO: use this.Content to load your game content here

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
            }
            // 1 workspace
            if (reStarted <= 0)
            {
                if (_pausedWorkspace)
                {
                    _pausedWorkspace = false;
                    StopWorkspace();
                    startBtn();
                }
            }

            if (_IsPlaying && _started)
            {
                workspace.Update();
            }
            // GUI
            screenGui.Update();
            screenGui.childrens.FindChildren<TextLable>("Points").text = "Points: " + workspace.player.points;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            // plan 2
            if (_IsPlaying && _started)
                workspace.Draw(_spriteBatch);
            
            // plan 1 
            screenGui.Draw(_spriteBatch);

            // scrFR.Draw(_spriteBatch); 
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
