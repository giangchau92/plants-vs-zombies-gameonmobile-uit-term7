using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCSEngine.GestureHandling
{
    public interface IGestureEvent
    {
        ITouch Touch { get; }
        ICollection<ITouch> GestureTouches { get; }
    }
}
