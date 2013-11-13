using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSCEngine.GestureHandling.Implements.Detectors
{
    public abstract class BaseGestureDetector<GE> : IGestureDetector<GE> where GE : IGestureEvent
    {
        public BaseGestureDetector()
        {
            this.GestureEventHash = typeof(GE).GetHashCode();
        }

        public int GestureEventHash
        { get; private set; }

        public IEnumerable<GE> Gestures
        {
            get
            {
                return gestures;
            }
        }
        protected ICollection<GE> gestures = new LinkedList<GE>();

        public abstract void DetectGesture(ICollection<ITouch> touches, Microsoft.Xna.Framework.GameTime gameTime);
    }
}