using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using SCSEngine.GestureHandling.Implements.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCSEngine.GestureHandling.Implements.Detectors
{
    public class TapDetector : BaseGestureDetector<Tap>
    {
        float minTap = 8 * 8;

        public override void DetectGesture(ICollection<ITouch> touches, GameTime gameTime)
        {
            this.gestures.BeginTrace();
            foreach (ITouch touch in touches)
            {
                if (touch.SystemTouch.State == TouchLocationState.Released)
                {
                    if (touch.Positions.TotalDelta.LengthSquared() < minTap)
                    {
                        this.gestures.Add(touch, new Tap(touch));
                    }
                }
            }
            this.gestures.EndTrace();
        }
    }
}