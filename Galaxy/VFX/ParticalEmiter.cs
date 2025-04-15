
using System;
using System.Collections.Generic;
using System.Threading;
using Debuger.InstancePlugin.UI;
using Galaxy.modules;
using Galaxy.modules.Maths;
using Galaxy.workspace;
using LearnMatrix;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Galaxy.VFX.ParticalEmiter
{

    public interface ParticuleEmiterMere
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

    public class EmitGroup : IGlobal
    {
        public Random Random = new Random();
        private Particule[] ParticuleContainer;
        private double initLifeTime;
        //PUBLICS
        public double elaplsedTime = 0;
        public const int MAX_EMIT = 5;

        public string textureName;
        public int columns;
        public string path;
        public float emitCount = 10;
        public Texture2D texture = Gradient.Linear(new List<ColorPoint>() { new ColorPoint { time = 0, color = Color.White }, new ColorPoint { time = 1, color = Color.White } }, 90);
        public KeyPointSequence size = new KeyPointSequence();
        public List<KeyPoints> squash = new List<KeyPoints>() { new KeyPoints { time = 0, value = 1 }, new KeyPoints { time = 1, value = 1 } };
        public List<KeyPoints> transparncy = new List<KeyPoints>() { new KeyPoints { time = 0, value = 0 }, new KeyPoints { time = 1, value = 0 } };
        public int ZIndex = 1;
        public MinMax lifeTime = new MinMax(4, 8);
        public MinMax speed = new MinMax(4, 8);
        public Vector3 acceleration = new Vector3(0, 0, 0);
        public double time = 1;
        public MinMax SpawnRotation = new MinMax(0, 0);
        public MinMax rotSpeed = new MinMax(0, 0);

        public Vector2 startPos;
        public Vector2 endPos;
        private Vector2 parentPos;
        public EmitGroup(Vector2 parentPos)
        {
            this.parentPos = parentPos;
            size.Add(new KeyPoints(1, 0, 0));
            size.Add(new KeyPoints(300, .5, 0));
            // size.Add(new KeyPoints(70, .3, 0));
            // size.Add(new KeyPoints(80, 0.8, 0));
            size.Add(new KeyPoints(100, 1, 0));
            initLifeTime = Random.Next((int)lifeTime.min, (int)lifeTime.max + 1) * 1000;

        }

        public void Emit(float emitCountParams = 1)
        {
            int x = 0;
            for (int i = 0; i < MAX_EMIT && x <= emitCountParams; i++)
            {
                if (ParticuleContainer[i].Emited == false)
                {
                    ParticuleContainer[i].Emited = true;
                    x++;
                }
            }
        }
        public void Initialize()
        {
            RandomPos();
            ParticuleContainer = new Particule[MAX_EMIT];

            for (int i = 0; i < MAX_EMIT; i++)
            {
                ParticuleContainer[i] = new Particule(texture);
                ParticuleContainer[i].Initialize();
            }
        }
        public void LoadContent()
        {
            for (int i = 0; i < MAX_EMIT; i++)
            {
                if (ParticuleContainer[i] != null)
                    ParticuleContainer[i].LoadContent();
            }
        }
        public void Update()
        {
            if (GlobalParams.UpdateTime == null) return;
            elaplsedTime += GlobalParams.UpdateTime.ElapsedGameTime.Milliseconds;
            if (elaplsedTime > initLifeTime)
                return;
            double normaliserLeTemp = elaplsedTime / initLifeTime;
            Vector2 scale = Vector2.One * (float)size.InUpdate(normaliserLeTemp);
            // System.Console.WriteLine(parentPos+"__"+endPos);
            Vector2 newPos = Formules.GetDirectionSpeed(startPos , endPos , (float)initLifeTime ,(float)elaplsedTime);
            for (int i = 0; i < MAX_EMIT; i++)
            {
                if (ParticuleContainer[i] != null)
                    ParticuleContainer[i].Update(scale,newPos);
            }
        }
        public void Draw()
        {
            for (int i = 0; i < ParticuleContainer.Length; i++)
            {
                if (ParticuleContainer[i] != null)
                    ParticuleContainer[i].Draw();
            }
        }
        public void RandomPos()
        {  
            startPos =(Vector2.One*((float)((Random.NextDouble()+.1) * 10)))+parentPos;
            endPos =(Vector2.One*((float)((Random.NextDouble()+1) * 100)))+parentPos;
        }
    }
    public class ParticalEmiter : GlobalObject, IGlobal
    {
        public const int MAX_GROUP = 30;
        private EmitGroup[] EmitGroupContainer;
        //PUBLICS

        public string textureName;
        public int columns;
        public string path;
        public float emitCount = 10;
        //public Texture2D texture = Gradient.Linear(new List<ColorPoint>() { new ColorPoint { time = 0, color = Color.White }, new ColorPoint { time = 1, color = Color.White } }, 90);
        public List<KeyPoints> size = new List<KeyPoints>() { new KeyPoints { time = 0, value = 1 }, new KeyPoints { time = 1, value = 1 } };
        public List<KeyPoints> squash = new List<KeyPoints>() { new KeyPoints { time = 0, value = 1 }, new KeyPoints { time = 1, value = 1 } };
        public List<KeyPoints> transparncy = new List<KeyPoints>() { new KeyPoints { time = 0, value = 0 }, new KeyPoints { time = 1, value = 0 } };
        public int ZIndex = 1;
        public MinMax lifeTime = new MinMax(4, 5);
        public MinMax speed = new MinMax(5, 10);
        public Vector3 acceleration = new Vector3(0, 0, 0);
        public double time = 1;
        public MinMax SpawnRotation = new MinMax(0, 0);
        public MinMax rotSpeed = new MinMax(0, 0);

        public ParticalEmiter(int columns, string path, string name, IGlobalParentObj parent)
        {
            this.textureName = path;
            this.name = name;
            this.columns = columns;
            this.path = path;
            this.parent = parent;
            EmitGroupContainer = new EmitGroup[MAX_GROUP];
            position = Vector2.One*200;
        }

        public void Emit(int emitCountParams = 1,int RowEmit=1)
        {
            for (int i = 0; i <= emitCountParams; i++)
            {
                EmitGroupContainer[GlobalParams.random.Next(0,MAX_GROUP)].Emit(1);
            }
        }

        public void Initialize()
        {
            for (int i = 0; i < MAX_GROUP; i++)
            {
                EmitGroupContainer[i] = new EmitGroup(position);
                EmitGroupContainer[i].Initialize();
            }
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
            { EmitGroupContainer[i].Draw(); }

            // base.Draw();
        }
    }
}