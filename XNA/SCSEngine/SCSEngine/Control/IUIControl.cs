using Microsoft.Xna.Framework;
using SCSEngine.GestureHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCSEngine.Control
{
    public interface IUIControl : IDrawable
    {
        ICanvas Canvas { get; }
        bool IsUICompleted { get; }

        void RegisterGestures(IGestureDispatcher dispatcher);
        void LeaveGestures(IGestureDispatcher dispatcher);
    }
}
