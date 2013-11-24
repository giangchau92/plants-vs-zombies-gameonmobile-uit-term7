using Microsoft.Xna.Framework;
using SCSEngine.GestureHandling;
using System;
using System.Collections.Generic;

namespace SCSEngine.Control
{
    public interface IUIContainer : IGestureDispatcher, IDrawable
    {
        IEnumerable<IUIControl> Components { get; }

        void Add(IUIControl control);
        void Remove(IUIControl control);
    }
}
