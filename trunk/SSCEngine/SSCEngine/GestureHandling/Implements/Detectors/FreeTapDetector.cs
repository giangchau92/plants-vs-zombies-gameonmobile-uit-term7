using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCSEngine.GestureHandling.Implements.Events;
using Microsoft.Xna.Framework.Input.Touch;

namespace SCSEngine.GestureHandling.Implements.Detectors
{
    public class FreeTapDetector : BaseGestureDetector<FreeTap>
    {
        public override void DetectGesture(ICollection<ITouch> touches, Microsoft.Xna.Framework.GameTime gameTime)
        {
            this.gestures.BeginTrace();
            foreach (ITouch touch in touches)
            {
                if (this.gestures.ContainsKey(touch))
                {
                    this.gestures.GetAndMarkTracedObject(touch);
                }
                else
                {
                    this.gestures.Add(touch, new FreeTap(touch));
                }
            }
            this.gestures.EndTrace();
        }
    }
}