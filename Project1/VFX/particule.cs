

using Galaxy.modules;
using LearnMatrix;

namespace Galaxy.VFX
{   class Particule : IGlobal
    {
        public double elaplsedTime=0 ;
        public double lifeTime=4;
        public Particule(){
            
        }
        public void Initialize(){}
        public void LoadContent(){}
        public void Update(){
            elaplsedTime+= GlobalParams.UpdateTime.ElapsedGameTime.Milliseconds;
            if (elaplsedTime > lifeTime)
                return;
            // GlobalParams.UpdateTime.ElapsedGameTime.Milliseconds + speed + acceleration
        }
        public void Draw(){}
    }
}