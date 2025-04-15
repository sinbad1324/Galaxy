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
using System.Collections.Generic;
using LearnMatrix;
using Galaxy.VFX.ParticalEmiter;
using Galaxy.modules.Maths;
namespace Galaxy
{
    public class GalaxyMotor : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        public ScreenGui screenGui;
        public Workspace workspace;
        public Vector2 size;
        public string playerPass = "";
        private bool clicked;
        public int reStarted;
        private bool _pausedWorkspace;
        private ParticalEmiter _particalEmiter;
        public bool IsPlaying
        {
            get { return _IsPlaying; }
        }
        protected bool _started;
        public bool started { get { return _started; } }
        protected bool _IsPlaying;
        public int raid;
        double tm = 0;
        // public ParticalEmiter prt;
        public GalaxyMotor()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";    
            IsMouseVisible = true;
            _graphics.IsFullScreen = false;
            _graphics.PreferredBackBufferWidth = GlobalParams.WINDOW_WIDHT;
            _graphics.PreferredBackBufferHeight = GlobalParams.WINDOW_HEIGTH;
            _graphics.ApplyChanges();
            _started = false;
            reStarted = 5;
            raid = 0;
            _IsPlaying = true;
            _pausedWorkspace = false;
            // prt = new(8,"tr","ewf",workspace);
        }
        private void StopWorkspace()
        {
            _IsPlaying = false;
        }
        //Functions
        private void Start(GlobalUI parent)
        {
            TextLable showRaids = screenGui.childrens.addTextLable("showRaids", "raid " + this.raid);
            showRaids.bgSize = new Vector2(100, 30);
            showRaids.position = new Vector2(GlobalParams.WINDOW_WIDHT / 2 - 50, 0);
            showRaids.bgColor = Color.Transparent;
            _started = true;
            _IsPlaying = true;
        }

        private void Restart()
        {
            if (!IsPlaying)
            {
                if (reStarted < 0) { StopWorkspace(); return; }
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
                        _IsPlaying = true;
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
        }

        //Methodes IGlobal
        private void UpdateTimers(GameTime gameTime)
        {
            for (int i = 0; i < GlobalParams.delays.Count; i++)
                if (GlobalParams.delays[i] != null)
                    GlobalParams.delays[i].Update(gameTime);
        }

        private void PlayerDeath()
        {
            workspace.player.health = 1;
            workspace.player.death += (object obj, EventArgs e) =>
            {
                _IsPlaying = false;
                reStarted -= 1;
                Restart();
                if (reStarted < 0) { StopWorkspace(); return; }
                ImageLable img = screenGui.childrens.addImageLable("Tete de mort", "TDM2");
                img.bgSize = new Vector2(300, 300);
                img.position = new Vector2(GlobalParams.WINDOW_WIDHT / 2 - 150, -100);
                int totalTime = 100;
                new Thread(() =>
                {
                    for (int i = 0; i < totalTime; i++)
                    {
                        Thread.Sleep(10);
                       
                        img.position = Formules.GetDirectionSpeed(img.position, new Vector2(GlobalParams.WINDOW_WIDHT / 2 - 150, GlobalParams.WINDOW_HEIGTH / 2 - 150), (float)totalTime, (float)i);
                    }
                    Thread.Sleep(30);
                    img.Destroy();
                }).Start();
            };
        }
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            GlobalParams.Content = Content;
            GlobalParams.spriteBatch = _spriteBatch;
            GlobalParams.Device = GraphicsDevice;
            GlobalParams.GameWindow = Window;
            screenGui = new ScreenGui();
            GlobalParams.Screen = screenGui;
            screenGui.Initialize();
            screenGui.LoadContent();
            workspace = new Workspace();
            GlobalParams.Workspace = workspace;
            workspace.Initialize();
            workspace.LoadContent();
            // prt.Initialize();

            _particalEmiter = new ParticalEmiter(0,"","",workspace);
            _particalEmiter.Initialize();
            _particalEmiter.LoadContent();
            _particalEmiter.Emit(1);
            //StarterUI.CreateHealthContainer(screenGui, 5);
            //CreateLoginForm();
            //PlayerDeath();
            //float scale = 1.2f;
            //TextButton startbtn = StarterUI.StartBtn(screenGui);
            //startbtn.Button1Click.UnPressed += (object obj, EventArgs e) =>
            // {
            //     scale = 1f;
            //     startbtn.bgSize = new Vector2(200f * scale, 50f * scale);
            //     startbtn.position = new Vector2((GlobalParams.WINDOW_WIDHT / 2) - 100, GlobalParams.WINDOW_HEIGTH / 2 + 30 * scale);
            //     _started = true;
            //     reStarted = 5;
            //     Start(screenGui);
            //     _pausedWorkspace = true;
            //     startbtn.Destroy();
            // };


            base.Initialize();
        }

        private void CreateLoginForm()
        {
            Dictionary<string, TextLable> formData = StarterUI.Form(screenGui);
            TextButton submit = formData["submit"] as TextButton;
            if (submit != null)
                submit.Button1Click.Pressed += (object seder, EventArgs e) =>
                {
                    playerPass = formData["password"].text;
                    workspace.player.name = formData["name"].text;
                };
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            GlobalParams.spriteBatch = _spriteBatch;
            // prt.LoadContent();
        //TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            UpdateTimers(gameTime);
            GlobalParams.UpdateTime = gameTime;
            if (Keyboard.GetState().IsKeyDown(Keys.Y) && !clicked)
            {
                if (GlobalParams.globalFillMode == FillMode.Solid)
                    GlobalParams.globalFillMode = FillMode.WireFrame;
                else
                    GlobalParams.globalFillMode = FillMode.Solid;
                clicked = true;
            }
            else if (Keyboard.GetState().IsKeyUp(Keys.Y))
                clicked = false;
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            _particalEmiter.Update();
            //Workspace
            //if (_IsPlaying && _started)
                workspace.Update();
            //GUI
            screenGui.Update();
            //GameVerify();
            if (reStarted <= 0)
            {
                if (_pausedWorkspace)
                {
                    _pausedWorkspace = false;
                    StopWorkspace();
                    }
            }
            //screenGui.childrens.FindChildren<TextLable>("Points").text = "Points: " + workspace.player.points;
            // prt.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            GlobalParams.DrawTime = gameTime;
            _particalEmiter.Draw();
            // plan 2  
            //if (_IsPlaying && _started)
            // workspace.Draw();
            //plan 1
            // screenGui.Draw();
            // prt.Draw();
            
            base.Draw(gameTime);
        }
    }
}
