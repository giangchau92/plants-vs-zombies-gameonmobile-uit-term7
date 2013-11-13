using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSCEngine.GestureHandling.BaseGestureHandling
{
    internal class TouchController : ITouchController
    {
        private const int MaxGestures = 4;
        private static readonly Vector2 InvalidPosition = new Vector2(int.MinValue, int.MinValue);
        private IDictionary<int, ITouch> touchesTracer;

        public TouchController()
        {
            this.touchesTracer = new Dictionary<int, ITouch>(2*MaxGestures);
        }

        public ICollection<ITouch> Touches
        {
            get
            {
                return touchesTracer.Values;
            }
        }

        private List<int> releasedTouchIDs = new List<int>(MaxGestures);
        public void Update(TouchCollection touchesCollection, GameTime gameTime)
        {
            releasedTouchIDs.Clear();
            releasedTouchIDs.AddRange(touchesTracer.Keys);

            foreach (TouchLocation touchLocation in touchesCollection)
            {
                if (this.touchesTracer.ContainsKey(touchLocation.Id))
                {
                    Touch touch = this.touchesTracer[touchLocation.Id] as Touch;
                    touch.UpdateLocation(touchLocation);

                    releasedTouchIDs.Remove(touchLocation.Id);
                }
                else
                {
                    this.touchesTracer.Add(touchLocation.Id, new Touch(touchLocation));
                }
            }

            foreach (int releasedTouch in this.releasedTouchIDs)
            {
                this.touchesTracer.Remove(releasedTouch);
            }
        }
    }
}
