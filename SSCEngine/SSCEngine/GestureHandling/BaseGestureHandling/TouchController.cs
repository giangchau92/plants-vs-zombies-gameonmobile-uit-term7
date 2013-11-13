using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using SSCEngine.Utils;
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
        private TracedDictionary<int, ITouch> touchesTracer = new TracedDictionary<int,ITouch>();

        public TouchController()
        {
        }

        public ICollection<ITouch> Touches
        {
            get
            {
                return touchesTracer.Values;
            }
        }

        public void Update(TouchCollection touchesCollection, GameTime gameTime)
        {
            this.touchesTracer.BeginTrace();
            foreach (TouchLocation touchLocation in touchesCollection)
            {
                if (this.touchesTracer.ContainsKey(touchLocation.Id))
                {
                    Touch touch = this.touchesTracer.GetAndMarkTracedObject(touchLocation.Id) as Touch;
                    touch.UpdateLocation(touchLocation);
                }
                else
                {
                    this.touchesTracer.Add(touchLocation.Id, new Touch(touchLocation));
                }
            }

            this.touchesTracer.EndTrace();
        }
    }
}
