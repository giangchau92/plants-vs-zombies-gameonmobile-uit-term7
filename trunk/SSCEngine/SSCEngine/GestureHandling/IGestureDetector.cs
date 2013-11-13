using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSCEngine.GestureHandling
{
    public interface IGestureDetector<GestureEvent> where GestureEvent : IGestureEvent
    {
        /// <summary>
        /// Gesture event hash MUST CORRECTED with typeof(GestureEvent).GetHashCode()
        /// </summary>
        int GestureEventHash { get; }
        IEnumerable<GestureEvent> Gestures { get; }

        void DetectGesture(ICollection<ITouch> touches, GameTime gameTime);
    }
}