using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSCEngine.GestureHandling.Implements.Events;

namespace SSCEngine.GestureHandling.Implements.Detectors
{
    public class FreeTapDetector : IGestureDetector
    {
        private List<IGestureEvent> listTap = new List<IGestureEvent>();
        private float minTap = 20 * 20;

        public ICollection<IGestureEvent> DetectGesture(ICollection<ITouch> touches)
        {
            List<IGestureEvent> taps = new List<IGestureEvent>(touches.Count);

            foreach (var touch in touches)
            {
                if (touch.Positions.Delta.LengthSquared() < minTap &&
                    touch.SystemTouch.State == Microsoft.Xna.Framework.Input.Touch.TouchLocationState.Released)
                    taps.Add(new FreeTap(touch, true));
                else
                    taps.Add(new FreeTap(touch, false));
            }
            return taps;
        }
    }
}
