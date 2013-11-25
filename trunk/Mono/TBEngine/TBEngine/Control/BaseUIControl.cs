using Microsoft.Xna.Framework;
using SCSEngine.Utils.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCSEngine.Control
{
    class BaseCanvas : ICanvas
    {
        public CRectangleF Bound { get; private set; }
        public CRectangleF Content { get; private set; }

        public BaseCanvas()
        {
            this.Bound = CRectangleF.Empty;
            this.Content = CRectangleF.Empty;
        }
    }

    public abstract class BaseUIControl : DrawableGameComponent, IUIControl
    {
        public ICanvas Canvas { get; private set; }

        public BaseUIControl(Game game)
            : base(game)
        {
            this.Canvas = new BaseCanvas();
            this.IsGestureCompleted = false;
        }

        public abstract void RegisterGestures(GestureHandling.IGestureDispatcher dispatcher);

        public abstract void LeaveGestures(GestureHandling.IGestureDispatcher dispatcher);

        public bool IsGestureCompleted { get; protected set; }
    }
}