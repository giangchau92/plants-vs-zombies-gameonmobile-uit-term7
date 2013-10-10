using SSCEngine.GestureHandling.Implements.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSCEngine.GestureHandling.Implements.Detectors
{
    public class TapDetector : IGestureDetector
    {
        float minTap = 100*100;
        private List<IGestureEvent> listTap = new List<IGestureEvent>();

        public ICollection<IGestureEvent> DetectGesture(ICollection<ITouch> touches)
        {
            List<IGestureEvent> taps = new List<IGestureEvent>(touches.Count);

            foreach (var item in touches)
            {
                if (item.SystemTouch.State == Microsoft.Xna.Framework.Input.Touch.TouchLocationState.Released
                    && item.Positions.TotalDelta.LengthSquared() < minTap)
                    taps.Add(new Tap(item));
            }

            return taps;
        }
    }
}
