
using System.Collections.Generic;
using System.Threading;
using Debuger.InstancePlugin.UI;
using Galaxy.modules;
using Galaxy.workspace;
using LearnMatrix;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Galaxy.VFX.ParticalEmiter
{
    class ParticalEmiter : GlobalObject, IGlobal 
    {
        private string textureName;
        private List<Particule> ParticuleContainer;
        private int columns;
        private string path;
        //PUBLICS
        public float emitCount = 10;
        public Texture2D texture = Gradient.Linear(new List<ColorPoint>() { new ColorPoint { time = 0, color = Color.White }, new ColorPoint { time = 1, color = Color.White } }, 90);
        public List<KeyPoints> size = new List<KeyPoints>() { new KeyPoints { time = 0, value = 1 }, new KeyPoints { time = 1, value = 1 } };
        public List<KeyPoints> squash = new List<KeyPoints>() { new KeyPoints { time = 0, value = 1 }, new KeyPoints { time = 1, value = 1 } };
        public List<KeyPoints> transparncy = new List<KeyPoints>() { new KeyPoints { time = 0, value = 0 }, new KeyPoints { time = 1, value = 0 } };
        public int ZIndex = 1;
        public MinMax lifeTime = new MinMax { min = 4, max = 8 };
        public MinMax speed = new MinMax { min = 5, max = 10 };
        public Vector3 acceleration = new Vector3(0, 0, 0);
        public double time = 1;
        public MinMax SpawnRotation = new MinMax { min = 0, max = 0 };
        public MinMax rotSpeed = new MinMax { min = 0, max = 0 };
        public ParticalEmiter(int columns, string path ,string name, IGlobalParentObj parent)
        {
            this.textureName = path;
            this.name = name;
            this.columns = columns;
            this.path = path;
            this.parent = parent;
        }

        public void Emit(float emitCountParams =10)
        {
            
        }

        public void Initialize() {}
        public void LoadContent()
        {
            for (int i = 0; i < ParticuleContainer.Count; i++)
                ParticuleContainer[i].LoadContent();
        }
        public void Update() {
            for (int i = 0; i < ParticuleContainer.Count; i++)
                ParticuleContainer[i].Update();
            
         }
        public void Draw() 
        { 
            for (int i = 0; i < ParticuleContainer.Count; i++)
                ParticuleContainer[i].Draw();
            
        }
    }
}