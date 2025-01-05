using System;
using Galaxy.workspace;
using Microsoft.Xna.Framework;

namespace Galaxy.Events
{


    public class FlipbookFinished
    {
        public event EventHandler finished;

        private void LunchEvent()
        {
            if (finished != null)
            {
                finished(this, EventArgs.Empty);
            }
        }

        public void Action() { 
            LunchEvent();
        }
    }
    public class BoundaryColl
    {
        public event EventHandler Touched;

        protected virtual void LunchEvent()
        {
            if (Touched != null)
            {
                Touched(this , EventArgs.Empty);
            }
        }

        public void EventAction()
        {
            LunchEvent();
        }
    }

    public class HoverEventArgs:EventArgs
    {
        Vector2 mousePosition {  get;  }

       public HoverEventArgs(Vector2 mp)
        {
            this.mousePosition = mp;
        }
    }
    public class HoverEvent
    {
        public event EventHandler<HoverEventArgs> Hover;
        public event EventHandler HoverLeave;

        protected virtual void LunchEvent(Vector2 mp)
        {
            HoverEventArgs newArgs = new HoverEventArgs(mp);
            if (Hover != null)
            {
                Hover(this, newArgs);
            }
        }
        protected virtual void LunchHoverLeaveEvent()
        {
            if (HoverLeave != null)
                HoverLeave(this, EventArgs.Empty);
        }
        public void EventAction(Vector2 mp)
        {
            LunchEvent(mp);
        }
        public void EventHoverLeaveAction()
        {
            LunchHoverLeaveEvent();
        }
    }

    public class mouseButton1Event
    {
        public event EventHandler Pressed;
        public event EventHandler UnPressed;


        protected virtual void LunchPressedEvent()
        {
            if (Pressed != null)
            {
                Pressed(this , EventArgs.Empty);
            }
        }

        protected virtual void LunchUnPressedEvent()
        {
            if (UnPressed != null)
                UnPressed(this, EventArgs.Empty);
            
        }

        public void PressedAction()
        {
            LunchPressedEvent();
        }
        public void UnPressedAction()
        {
            LunchUnPressedEvent();
        }
    }

    public class LostFocusedArgs : EventArgs
    {
        public string text { get; }
        public LostFocusedArgs(string text) {
            this.text = text;
        }
    }
    public class LostFocused
    {
        public event EventHandler<LostFocusedArgs> lostFocused;

        protected virtual void LunchPressedEvent(string text)
        {

            LostFocusedArgs arg = new LostFocusedArgs(text);
            if (lostFocused != null)
            {
                lostFocused(this, arg);
            }
        }


        public void Action(string text)
        {
            LunchPressedEvent(text);
        }
    }


    public class TouchArgs : EventArgs 
    {
        public GlobalObject obj { get;  }
        public TouchArgs(GlobalObject obj)
        {
            this.obj = obj;
        }
    }
    public class  Cloision 
    {
        public event EventHandler<TouchArgs> touched;
        public event EventHandler<TouchArgs> touching;

        public void  TouchedLuncher(GlobalObject obj)
        {
            TouchArgs arg = new TouchArgs(obj);
            if (touched != null)
            {
                touched(this, arg);
            }
        }

        public void TouchedAction(GlobalObject obj)
        {
            TouchedLuncher(obj);
        }


        public void TouchingLuncher(GlobalObject obj)
        {
            TouchArgs arg = new TouchArgs(obj);
            if (touching != null)
            {
                touching(this, arg);
            }
        }

        public void TouchingAction(GlobalObject obj)
        {
            TouchingLuncher(obj);
        }


    }


   // public class 


    public class TweenCompletedEvent
    {
        public event EventHandler completed;

        protected virtual void LunchEvent()
        {
            
            if (completed != null)
            {
                completed(this, EventArgs.Empty);
            }
        }


        public void Action()
        {
            LunchEvent();
        }
    }

}
