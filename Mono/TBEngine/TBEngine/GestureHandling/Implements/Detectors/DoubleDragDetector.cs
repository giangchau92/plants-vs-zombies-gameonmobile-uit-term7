using SCSEngine.Mathematics;
using SCSEngine.GestureHandling.Implements.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCSEngine.GestureHandling.Implements.Detectors
{
    public class DoubleDragDetector : BaseGestureDetector<DoubleDrag>
    {
        private float minAngle = 10;
        //public ICollection<IGestureEvent> DetectGesture(ICollection<ITouch> touches)
        //{
        //    List<IGestureEvent> drags = new List<IGestureEvent>(touches.Count);

        //    List<ITouch> usedTouch = new List<ITouch>(touches.Count); 
        //    for (int i = 0; i < touches.Count; i++ )
        //    {
        //        ITouch touch1 = touches.ElementAt(i);
        //        for (int j = i + 1; j < touches.Count; j++)
        //        {
        //            ITouch touch2 = touches.ElementAt(j);

        //            // Khong su dung lai cac touch da bat
        //            if (usedTouch.Contains(touch1) || usedTouch.Contains(touch2))
        //                continue;
        //            float angle = (float)(GMath.Angle(touch1.Positions.Delta, touch2.Positions.Delta) * 180 / Math.PI); // Degree

        //            if (angle < minAngle)
        //            {
        //                drags.Add(new DoubleDrag(touch1, touch2));
        //                usedTouch.Add(touch1);
        //                usedTouch.Add(touch2);
        //            }
        //        }
        //    }

        //    return drags;
        //}

        public override void DetectGesture(ICollection<ITouch> touches, Microsoft.Xna.Framework.GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
