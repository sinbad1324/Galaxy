using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Security.Cryptography;
using LearnMatrix;
using System.Threading;
using Galaxy.modules.Maths;

namespace Galaxy.modules
{

    public struct KeyPoints
    {
        private double _time;
        public double time { get { return _time; } set { _time = Math.Clamp(value, 0, 1); } }
        public double value;
        public double env;
        public string GetStr()
        {
            return $"time = {time} , value = {value} , env = {env}";
        }
        public KeyPoints(double valeur, double time, double env = 0)
        {
            this.value = valeur;
            this.time = time;
            this.env = env;
        }
    }
    public struct MinMax
    {
        private double _min;
        private double _max;
        public double min { set { if (value > _max) throw new ArgumentOutOfRangeException(nameof(min), "Cette (min) valeur est plus grand que le max"); _min = value; } get { return _min; } }
        public double max { set { if (value < _min) throw new ArgumentOutOfRangeException(nameof(max), "Cette (max) valeur est plus petit que le min"); _max = value; } get { return _max; } }

        public MinMax(double min, double max)
        {
            this._min = min;
            this._max = max;
            if (_min > _max) throw new ArgumentOutOfRangeException(nameof(min), "Cette (min) valeur est plus grand que le max");

        }
    }
    public class KeyPointSequence
    {
        public List<KeyPoints> keys;
        public KeyPointSequence()
        {
            keys = new List<KeyPoints>();
        }

        public void Add(KeyPoints k)
        {
            if (keys.Count > 21) return;
            keys.Add(k);
            Sort();
        }

        public void Sort()
        {
            keys = keys.OrderBy(k => k.time).ToList();
        }

        public void Sequnce(double totalTime, Action<KeyPoints> callback)
        {
            new Thread(() =>
            {
                double currentTime = 0;
                int i = 0;
                while (currentTime <= totalTime && i < this.keys.Count - 1)
                {
                    if (GlobalParams.UpdateTime != null)
                    {
                        currentTime += GlobalParams.UpdateTime.ElapsedGameTime.Milliseconds;
                        double normalis = currentTime / totalTime;
                        KeyPoints key1 = this.keys[i];
                        KeyPoints key2 = this.keys[i + 1];
                        if (normalis >= key2.time)
                            i++;
                        callback(new KeyPoints(Formules.InterpolationLinaire(key1, key2, normalis), normalis, 0));
                    }
                }
            }).Start();
        }


        private int i = 0;
        public double InUpdate(double normalis)
        {
            if (i < this.keys.Count - 1)
            {
                KeyPoints key1 = this.keys[i];
                KeyPoints key2 = this.keys[i + 1];
                if (normalis >= key2.time)
                    i++;
                return Formules.InterpolationLinaire(key1, key2, normalis);
            }
            else
            {
                i = 0;
            }
            return 0;
        }
    }
    public class Utils
    {
        public static void Print(string separator, params object[] args)
        {
            string affichage = "";
            for (int i = 0; i < args.Length - 1; i++)
                if (i > 0)
                    affichage += "_" + affichage;
            Console.WriteLine(affichage);
        }
        static public Dictionary<string, float> GetSegements(Vector2 Position, Vector2 size)
        {
            return new Dictionary<string, float>{
                { "left", Position.X - size.X / 2 },
                { "right", Position.X + size.X / 2 },
                { "top", Position.Y - size.Y / 2 },
                { "bottom", Position.Y + size.Y / 2 }
            };
        }
    }
}
