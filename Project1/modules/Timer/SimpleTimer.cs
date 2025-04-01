using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LearnMatrix;
using Microsoft.Xna.Framework;

namespace Debuger.modules.Timer
{
    public class Delay
    {
        private bool actived;
        private double delay;
        private double totalMs = 0;
        private Action callBack;

       public Delay(double delay, Action func = null, bool active = true)
        {
            this.delay = delay;
            callBack = func;
            actived = active;
            GlobalParams.delays.Add(this);
        }

        public void Update(GameTime gameTime)
        {
            if (actived)
            {
                totalMs += gameTime.ElapsedGameTime.TotalMilliseconds;
                if (totalMs >= delay)
                {
                    if (callBack != null)
                        callBack();
                    Destroy();
                }
            }
        }

        void Destroy()
        {
            actived = false;
            callBack = null;
            totalMs = 0;
            delay = 0;
            GlobalParams.delays.Remove(this);
            GC.SuppressFinalize(this);
        }
    }
}
