using Galaxy.workspace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Galaxy.Events;

//Tween tw = new Tween(part);
//tw.Create(new TweenInfos(10 , EasingStyle.Linear , DirectionStyle.Bounce) , part2.position);
//tw.Start();

//tw.tweenCompleted.completed += (object obj, EventArgs args) => {
//    Console.WriteLine("Completed");
//};

namespace Galaxy.modules
{
    //public class TweenInfos // fait avec chatGPT mais bien  compris
    //{
    //    public int totalTime;
    //    public EasingStyle easingStyle;
    //    public DirectionStyle directionStyle;
    //    public TweenInfos(int Time, EasingStyle style, DirectionStyle dir) { 
    //        this.totalTime = Time;
    //        this.easingStyle = style;
    //        this.directionStyle = dir;
    //    }

    //    public Vector2 CalculatePosition(Vector2 pointA, Vector2 pointB, float currentTime)
    //    {
    //        float normalizedTime = currentTime / totalTime;

    //        normalizedTime = Math.Clamp(normalizedTime, 0f, 1f);

    //        normalizedTime = AdjustForDirection(normalizedTime, directionStyle);

    //        float easedTime = ApplyEasing(normalizedTime, easingStyle);

    //        float TotalDisance = (pointA - pointB).Length();
    //        float speed = TotalDisance / totalTime;
    //        Vector2 lookvector = Vector2.Normalize((pointA - pointB));



    //        //float x = Utils.Lerp(pointA.X, pointB.X, easedTime);
    //        //float y = Utils.Lerp(pointA.Y, pointB.Y, easedTime);

    //        return pointA + (lookvector*speed);
    //    }
    //    private float AdjustForDirection(float time, DirectionStyle directionStyle)
    //    {
    //        switch (directionStyle)
    //        {
    //            case DirectionStyle.Backward:
    //                return 1 - time; // Reverse the direction
    //            case DirectionStyle.Alternating:
    //                return time < 0.5f ? time * 2 : (1 - time) * 2; // Move forward in the first half, reverse in the second half
    //            case DirectionStyle.Bounce:
    //                return Math.Abs(MathF.Sin(time * MathF.PI)); // Bounce effect
    //            default: // Forward
    //                return time;
    //        }
    //    }

    //    private float ApplyEasing(float time, EasingStyle easingStyle)
    //    {
    //        switch (easingStyle)
    //        {
    //            case EasingStyle.Linear:
    //                return time; // No easing, linear motion
    //            case EasingStyle.Quadratic:
    //                return time * time; // Quadratic easing (ease-in)
    //            case EasingStyle.Cubic:
    //                return time * time * time; // Cubic easing (ease-in)
    //            case EasingStyle.Quartic:
    //                return time * time * time * time; // Quartic easing (ease-in)
    //            case EasingStyle.Quintic:
    //                return time * time * time * time * time; // Quintic easing (ease-in)
    //            case EasingStyle.Sinusoidal:
    //                return (float)Math.Sin(time * Math.PI / 2); // Smooth sine wave easing
    //            case EasingStyle.Exponential:
    //                return (float)Math.Pow(2, 10 * (time - 1)); // Exponential easing
    //            case EasingStyle.Elastic:
    //                return (float)Math.Sin(13 * Math.PI / 2 * time) * MathF.Pow(2, 10 * (time - 1)); // Elastic easing
    //            case EasingStyle.Bounce:
    //                return BounceEaseOut(time); // Bounce effect
    //            default:
    //                return time;
    //        }
    //    }

    //    private float BounceEaseOut(float time)
    //    {
    //        if (time < 1 / 2.75f)
    //        {
    //            return 7.5625f * time * time;
    //        }
    //        else if (time < 2 / 2.75f)
    //        {
    //            time -= 1.5f / 2.75f;
    //            return 7.5625f * time * time + 0.75f;
    //        }
    //        else if (time < 2.5 / 2.75)
    //        {
    //            time -= 2.25f / 2.75f;
    //            return 7.5625f * time * time + 0.9375f;
    //        }
    //        else
    //        {
    //            time -= 2.625f / 2.75f;
    //            return 7.5625f * time * time + 0.984375f;
    //        }
    //    }
    //}
    //public class Tween
    //{
    //    private bool stop;
    //    private bool started;
    //    private double StatredTime;
    //    private GlobalObject _object;
    //    private DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    //    public TweenCompletedEvent tweenCompleted;
    //    public Tween(GlobalObject obj) {
    //        _object = obj;
    //        stop = false;
    //        started = false;
    //        tweenCompleted = new TweenCompletedEvent();
    //    }

    //    public void Create(TweenInfos twInfo , Vector2 PointB) {
    //        new Thread(() => {
    //            while (true)
    //            {
    //                if (stop)
    //                    break;

    //                if (started)
    //                {
    //                    float currentTime = (float)((DateTime.UtcNow - epoch).TotalSeconds - StatredTime);
    //                    Console.WriteLine(currentTime);
    //                    Vector2 newPos = twInfo.CalculatePosition(_object.position, PointB, currentTime);
    //                    _object.position = newPos;
    //                    if (currentTime >= twInfo.totalTime )
    //                    {
    //                        stop = true;
    //                        _object.position = PointB;
    //                    }



    //                }
    //                Thread.Sleep(10);

    //            }
    //                tweenCompleted.Action();

    //        }).Start();

    //    }

    //    public void Start() {
    //        StatredTime =  (DateTime.UtcNow - epoch).TotalSeconds;
    //        started = true;
    //    }
    //    public void Pause() {
    //        started = false;
    //    }

    //    public void Stop() {
    //        stop = true;
    //    }

    //}


    //public  enum EasingStyle
    //  {
    //      Linear,      // Linear motion with no acceleration or deceleration
    //      Quadratic,   // Acceleration or deceleration following a quadratic curve
    //      Cubic,       // More pronounced acceleration or deceleration with a cubic curve
    //      Quartic,     // Even stronger acceleration or deceleration
    //      Quintic,     // Extremely strong acceleration or deceleration
    //      Sinusoidal,  // Motion based on a sine wave
    //      Exponential, // Exponential acceleration or deceleration
    //      Circular,    // Smooth transition based on circular motion
    //      Elastic,     // Elastic motion with overshooting before settling
    //      Bounce       // Bouncing effect with multiple changes in direction
    //  }

    //  public enum DirectionStyle
    //  {
    //      Forward,     // Motion in the initial direction
    //      Backward,    // Motion in the reverse direction
    //      Alternating, // Motion alternates between forward and backward
    //      Bounce       // Motion changes direction several times like a bounce
    //  }
}
