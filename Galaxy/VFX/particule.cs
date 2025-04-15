

using Galaxy.modules;
using LearnMatrix;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Galaxy.VFX
{   class Particule
    {
        public bool Emited = false;
        public double elaplsedTime=0 ;
        public double lifeTime=4;
        private Texture2D texture;
        private Color color=Color.Wheat;
        private Rectangle rectangle;
        public Particule(Texture2D texture){
            this.texture = texture;
        }
        public void Initialize(){
            rectangle = new Rectangle(0,0,0,0);
        }
        public void LoadContent(){}
        public void Update(Vector2 size,Vector2 position){
            if (Emited == false)return;
            //elaplsedTime+= GlobalParams.UpdateTime.ElapsedGameTime.Milliseconds;
            //if (elaplsedTime > lifeTime)
            //    return;
            System.Console.WriteLine(position);
            rectangle.Location = position.ToPoint();
            rectangle.Size = size.ToPoint();
            //  GlobalParams.UpdateTime.ElapsedGameTime.Milliseconds + speed + acceleration
        }
        public void Draw(){
            GlobalParams.spriteBatch.Begin();
            GlobalParams.spriteBatch.Draw(texture ,rectangle,color);
            GlobalParams.spriteBatch.End();
        }
    }
}