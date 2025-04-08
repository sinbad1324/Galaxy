

using Galaxy.modules;
using LearnMatrix;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Galaxy.VFX
{   class Particule
    {
        public double elaplsedTime=0 ;
        public double lifeTime=4;
        private Texture2D texture;
        private Color color;
        private Rectangle rectangle;
        public Particule(Texture2D texture){
            this.texture = texture;
        }
        public void Initialize(){
            rectangle = new Rectangle(0,0,0,0);
        }
        public void LoadContent(){}
        public void Update(Vector2 size ){
            elaplsedTime+= GlobalParams.UpdateTime.ElapsedGameTime.Milliseconds;
            if (elaplsedTime > lifeTime)
                return;
            //  GlobalParams.UpdateTime.ElapsedGameTime.Milliseconds + speed + acceleration
        }
        public void Draw(){
            GlobalParams.spriteBatch.Draw(texture ,rectangle,color);
        }
    }
}