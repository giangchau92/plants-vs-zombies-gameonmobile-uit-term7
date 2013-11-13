using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;

namespace SSCEngine.GestureHandling.Implements.Events
{
    public class FreeTap : BaseGestureHandling.BaseGestureEvent
    {
        public FreeTap(ITouch tap)
            : base(1)
        {
            this.gestureTouches.Add(tap);
        }

        public Vector2 Begin
        {
            get { return this.Touch.Positions.Begin;}
        }

        public Vector2 Current
        {
            get { return this.Touch.Positions.Current; }
        }

        public bool IsFinish
        {
            get
            {
                return (this.Touch.SystemTouch.State == TouchLocationState.Released);
            }
        }
    }
}
