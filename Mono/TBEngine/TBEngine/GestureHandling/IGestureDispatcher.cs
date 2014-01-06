using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCSEngine.GestureHandling
{
    public interface IGestureDispatcher
    {
        void AddTarget<GestureEvent>(IGestureTarget<GestureEvent> gTarget) where GestureEvent : IGestureEvent;
        void RemoveTarget<GestureEvent>(IGestureTarget<GestureEvent> gTarget) where GestureEvent : IGestureEvent;

        void Dispatch<GestureEvent>(GestureEvent gEvent) where GestureEvent : IGestureEvent;

        bool Enabled { get; set; }

        IGestureTarget<GestureEvent> HandleTarget<GestureEvent>(GestureEvent e) where GestureEvent : IGestureEvent;
        void SetHandleTarget<GestureEvent>(GestureEvent e, IGestureTarget<GestureEvent> gTarget) where GestureEvent : IGestureEvent;
    }
}