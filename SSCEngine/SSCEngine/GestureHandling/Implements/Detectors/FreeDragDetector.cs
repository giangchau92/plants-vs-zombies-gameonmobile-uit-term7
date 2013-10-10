using SSCEngine.GestureHandling.Implements.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSCEngine.GestureHandling.Implements.Detectors
{
    class FreeDragDetector : IGestureDetector
    {
        public ICollection<IGestureEvent> DetectGesture(ICollection<ITouch> touches)
        {
            List<IGestureEvent> drags = new List<IGestureEvent>(touches.Count);
            float minDrap = 20 * 20;

            foreach (var touch in touches)
            {
                if (touch.Positions.Delta.LengthSquared() > minDrap)
                    drags.Add(new FreeDrag(touch));
            }

            return drags;
        }
    }
}
