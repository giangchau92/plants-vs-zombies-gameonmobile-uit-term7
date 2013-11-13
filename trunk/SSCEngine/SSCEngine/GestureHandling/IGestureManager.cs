using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSCEngine.GestureHandling
{
    public interface IGestureManager : IUpdateable, IGameComponent
    {
        void AddDetector<GestureEvent>(IGestureDetector<GestureEvent> gDetector) where GestureEvent : IGestureEvent;
        void RemoveDetector<GestureEvent>(IGestureDetector<GestureEvent> gDetector) where GestureEvent : IGestureEvent;

        void AddDispatcher(IGestureDispatcher gDispatcher);
        void RemoveDispatcher(IGestureDispatcher gDispatcher);
    }
}