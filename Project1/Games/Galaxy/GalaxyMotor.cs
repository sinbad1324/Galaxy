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
using System.Threading;
using Project1.Games.Galaxy.component;


namespace Galaxy
{
    public class GalaxyMotor : Game
    {
        public GraphicsDeviceManager _graphics;
        public SpriteBatch _spriteBatch;
        public ScreenGui screenGui;
        public Workspace workspace;
        public Vector2 size;
        public string PlayerName;
        public int reStarted;

        private bool _pausedWorkspace;
        public bool IsPlaying
        {
            get { return _IsPlaying; }
        }


        protected bool _started;
        public bool started { get { return _started; } }

        protected bool _IsPlaying;
        public int raid;


        public GalaxyMotor()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.IsFullScreen = false;
            _graphics.PreferredBackBufferWidth = 1500;
            _graphics.PreferredBackBufferHeight = 1000;
            _graphics.ApplyChanges();
            screenGui = new ScreenGui(this, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight, Window);
            workspace = new Workspace(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight, this);
            _started = false;
            reStarted = 0;
            raid = 0;
            _IsPlaying = true;
            _pausedWorkspace = false;

        }

        private void StopWorkspace()
        {
            _IsPlaying = false;
        }
        // Functions
        private void Start(GlobalUI parent)
        {
            TextLable showRaids = screenGui.childrens.addTextLable("showRaids", "raid " + this.raid);
            showRaids.bgSize = new Vector2(100, 30);

            showRaids.position = new Vector2(screenGui.screenWidth / 2 - 50, 0);
            showRaids.bgColor = Color.Transparent;
            _started = true;
            _IsPlaying = true;

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
                TextButton btn = StarterUI.RestartBtnComp(screenGui);
                int totalTime = 1;
                new Thread(() =>
                {
                    Thread.Sleep(10 * totalTime);
                    Frame HealthContainer = screenGui.childrens.FindChildren<Frame>("HealthContainer");
                    if (HealthContainer != null)
                    {
                        ImageLable lasthealth = HealthContainer.childrens.FindChildren<ImageLable>("health" + reStarted);
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
                    };
                }).Start();
            }
            workspace.player.death += (object obj, EventArgs e) =>
            {
                reStarted = -1;
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
                _IsPlaying = false;
            };
        }

        //Methodes IGlobal 


        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();
            screenGui.Initialize();
            workspace.Initialize();
            GameVerify();
            float scale = 1.2f;
            TextButton startbtn = StarterUI.StartBtn(screenGui);
            StarterUI.StartBtn(screenGui).Button1Click.UnPressed += (object obj, EventArgs e) =>
            {
                scale = 1f;
                startbtn.bgSize = new Vector2(200f * scale, 50f * scale);
                startbtn.position = new Vector2((screenGui.screenWidth / 2) - 100, screenGui.screenHeight / 2 + 30 * scale);
                //if (NameInput.text != "")
                //{
                _started = true;
                //  NameInput.Destroy();
                reStarted = 5;
                Start(screenGui);
                _pausedWorkspace = true;
                startbtn.Destroy();

                //}
            };
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
            //Workspace
            if (_IsPlaying && _started)
                workspace.Update();
            // GUI
            screenGui.Update();


            if (reStarted <= 0)
            {
                if (_pausedWorkspace)
                {
                    _pausedWorkspace = false;
                    StopWorkspace();
                    StarterUI.StartBtn(screenGui);
                }
            }
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
