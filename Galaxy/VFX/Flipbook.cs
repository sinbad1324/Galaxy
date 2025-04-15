using Galaxy.Events;
using Galaxy.Gui.Frames;
using Galaxy.Gui.GuiInterface;
using Galaxy.modules;
using Galaxy.workspace;
using LearnMatrix;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;



namespace Galaxy.VFX
{


    public class Flipbook : GlobalObject
    {

        private int columns;
        private string path;
        private Vector2 imageSize;
        private int numFrames;
        private int activeFrame;
        private int Counter;
        private int rowPos;
        private int colPos;
        private bool isPaused;
        public int interval;
        public FlipbookFinished Finish;
        public bool isFinishedDestroy=false;
        private void NextFrame()
        {
            activeFrame++;
            if (activeFrame >= numFrames)
                Stop();

            colPos++;
            if (colPos >= columns)
            {
                colPos = 0;
                rowPos++;
            }
        }
        public Flipbook( int columns, string path, string name, IGlobalParentObj parent)
        {
            rotation = 0;
            this.parent = parent;
            this.name = name;
            Finish = new FlipbookFinished();
            isPaused = false;
            numFrames = columns * columns;
            this.columns = columns;
            this.path = path;
            activeFrame = 0;
            interval = 2;
            Counter = 0;
            position = Vector2.Zero;
            canCollide = false;
            SpriteSize = new Vector2(imageSize.X/columns , imageSize.Y / columns);
        }
        public override void LoadContent()
        {
            texture = GlobalParams.Content.Load<Texture2D>(path );
            this.imageSize = new Vector2(texture.Width, texture.Height);
        }
        public override void Update()
        {
            if (isPaused || this.columns < 1) return;
            Counter++;
            if (Counter > interval)
            {
                NextFrame();
                Counter = 0;
            }
        }
        public void Reset()
        {
            Counter = 0;
            colPos = 0;
            rowPos = 0;
            activeFrame = 0;

        }
        public void Stop()
        {
            Pause();
            Reset();
            Finish.Action();
            if (isFinishedDestroy)
                Destroy();
        }
        public void Play()
        {
             isPaused = false;
        }
        public void Pause()
        {
            isPaused = true;
        }
        public Rectangle Rectangle()
        {
            int x = Convert.ToInt32(imageSize.X / columns);
            int y = Convert.ToInt32(imageSize.Y / columns);
            return new Rectangle(
                colPos * x,
                rowPos * y,
                x,
                y
                );
        }
        public override void Draw()
        {
            if (isPaused) return; 
            GlobalParams.spriteBatch.Draw(texture, position, Rectangle(), Color.White, rotation, Vector2.Zero, Vector2.One, SpriteEffects.None, 1);
        }

        public void Destroy()
        {
            Counter = 0;
            colPos = 0;
            rowPos = 0;
            activeFrame = 0;
            activeFrame = 0;
            base.Bdestroy();
            this.name = "";
            GlobalParams.Workspace.childrens.container.Remove(this);
            GC.SuppressFinalize(this);
        }
    }
}
