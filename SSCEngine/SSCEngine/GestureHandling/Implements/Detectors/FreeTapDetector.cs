using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSCEngine.GestureHandling.Implements.Events;
using Microsoft.Xna.Framework.Input.Touch;

namespace SSCEngine.GestureHandling.Implements.Detectors
{
    public class FreeTapDetector : BaseGestureDetector<FreeTap>
    {
        public override void DetectGesture(ICollection<ITouch> touches, Microsoft.Xna.Framework.GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
