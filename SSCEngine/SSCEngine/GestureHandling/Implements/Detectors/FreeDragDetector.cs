using SSCEngine.GestureHandling.Implements.Events;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSCEngine.GestureHandling.Implements.Detectors
{
    class FreeDragDetector : BaseGestureDetector<FreeDrag>
    {
        public override void DetectGesture(ICollection<ITouch> touches, Microsoft.Xna.Framework.GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
