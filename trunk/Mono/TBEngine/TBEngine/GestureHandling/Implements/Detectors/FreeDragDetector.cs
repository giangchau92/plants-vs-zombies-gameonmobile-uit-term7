using SCSEngine.GestureHandling.Implements.Events;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCSEngine.GestureHandling.Implements.Detectors
{
    class FreeDragDetector : BaseGestureDetector<FreeDrag>
    {
        private const float minSquaredDelta = 20 * 20;

        public override void DetectGesture(ICollection<ITouch> touches, Microsoft.Xna.Framework.GameTime gameTime)
        {
            this.gestures.BeginTrace();
            foreach (var touch in touches)
            {
                if (this.gestures.ContainsKey(touch))
                {
                    this.gestures.GetAndMarkTracedObject(touch);
                }
                else
                {
                    if (touch.Positions.TotalDelta.LengthSquared() >= minSquaredDelta)
                    {
                        this.gestures.Add(touch, new FreeDrag(touch));
                    }
                }
            }
            this.gestures.EndTrace();
        }
    }
}
