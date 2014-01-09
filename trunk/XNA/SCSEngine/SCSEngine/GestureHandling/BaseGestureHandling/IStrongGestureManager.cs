using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCSEngine.GestureHandling
{
    interface IStrongGestureManager
    {
        void Update(ICollection<ITouch> touches, GameTime gameTime);
    }

    class StrongGestureManager<GestureEvent> : IStrongGestureManager where GestureEvent : IGestureEvent
    {
        private IGestureDetector<GestureEvent> detector;
        private IEnumerable<IGestureDispatcher> dispatchers;

        public StrongGestureManager(IGestureDetector<GestureEvent> dect, IEnumerable<IGestureDispatcher> dispatchers)
        {
            this.detector = dect;
            this.dispatchers = dispatchers;
        }

        public void Update(ICollection<ITouch> touches, GameTime gameTime)
        {
            this.detector.DetectGesture(touches, gameTime);

            foreach (GestureEvent ge in this.detector.Gestures)
            {
                foreach (IGestureDispatcher dispatcher in this.dispatchers)
                {
                    if (dispatcher.Enabled)
                    {
                        // Asycn problembs
                        dispatcher.Dispatch<GestureEvent>(ge);
                    }
                }
            }
        }
    }
}