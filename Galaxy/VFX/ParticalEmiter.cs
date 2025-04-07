
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

    interface ParticuleEmiterMere
    {

        string textureName { get; set; }
        int columns { get; set; }
        string path { get; set; }
        float emitCount { get; set; }
        Texture2D texture { get; set; }
        List<KeyPoints> size { get; set; }
        List<KeyPoints> squash { get; set; }
        List<KeyPoints> transparncy { get; set; }
        int ZIndex { get; set; }
        MinMax lifeTime { get; set; }
        MinMax speed { get; set; }
        Vector3 acceleration { get; set; }
        double time { get; set; }
        MinMax SpawnRotation { get; set; }
        MinMax rotSpeed { get; set; }


    }

    class EmitGroup() : IGlobal
    {
        private Particule[] ParticuleContainer;
        private double initLifeTime;
        //PUBLICS
        public double elaplsedTime = 0;
        public const int MAX_EMIT = 100;

        public string textureName;
        public int columns;
        public string path;
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

        public void Emit(float emitCountParams = 10)
        {

        }

        public void Initialize()
        {
            ParticuleContainer = new Particule[MAX_EMIT];
        }
        public void LoadContent()
        {
            for (int i = 0; i < ParticuleContainer.Length; i++)
                ParticuleContainer[i].LoadContent();
        }
        public void Update()
        {
            Vector2 scale = Vector2.One;
            elaplsedTime += GlobalParams.UpdateTime.ElapsedGameTime.Milliseconds;
            if (elaplsedTime > initLifeTime)
                return;
            double normaliserLeTemp =elaplsedTime/initLifeTime;

            for (int i = 0; i < ParticuleContainer.Length; i++)
                ParticuleContainer[i].Update(scale);

        }
        public void Draw()
        {
            for (int i = 0; i < ParticuleContainer.Length; i++)
                ParticuleContainer[i].Draw();

        }
    }
    class ParticalEmiter : GlobalObject, IGlobal
    {
        public const int MAX_GROUP = 5;
        private EmitGroup[] EmitGroupContainer;
        //PUBLICS

        public string textureName;
        public int columns;
        public string path;
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

        public ParticalEmiter(int columns, string path, string name, IGlobalParentObj parent)
        {
            this.textureName = path;
            this.name = name;
            this.columns = columns;
            this.path = path;
            this.parent = parent;
        }

        public void Emit(float emitCountParams = 10)
        {

        }

        public void Initialize()
        {
            EmitGroupContainer = new EmitGroup[MAX_GROUP];
        }
        public override void LoadContent()
        {
            for (int i = 0; i < EmitGroupContainer.Length; i++)
                EmitGroupContainer[i].LoadContent();
        }
        public override void Update()
        {

            for (int i = 0; i < EmitGroupContainer.Length; i++)
                EmitGroupContainer[i].Update();

        }
        public override void Draw()
        {
            for (int i = 0; i < EmitGroupContainer.Length; i++)
                EmitGroupContainer[i].Draw();

        }
    }
}