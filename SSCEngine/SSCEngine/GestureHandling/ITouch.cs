using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSCEngine.GestureHandling
{
    public interface ITouch
    {
        int TouchID { get; }

        TouchLocation SystemTouch { get; }
        //GestureType Type { get; }

        TouchPositions Positions { get; }
    }
}