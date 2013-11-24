using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCSEngine.GestureHandling.BaseGestureHandling
{
    class Touch : ITouch
    {
        public Touch(TouchLocation location)
        {
            this.systemTouch = location;
            this.TouchID = location.Id;
            this.positions.Begin = this.systemTouch.Position;
            this.positions.Current = this.systemTouch.Position;
        }

        internal TouchLocation systemTouch;

        public TouchLocation SystemTouch
        {
            get { return systemTouch; }
        }

        internal TouchPositions positions;

        public TouchPositions Positions
        {
            get { return positions; }
        }

        public int TouchID { get; private set; }

        internal void UpdateLocation(TouchLocation touchLocation)
        {
            if (this.TouchID == touchLocation.Id)
            {
                this.systemTouch = touchLocation;
                this.positions.UpdateLocation(touchLocation.Position);
                return;
            }

            throw new InvalidOperationException(string.Format("TouchLocation id({0}) must be matched with TouchID({1})", touchLocation.Id, TouchID));
        }
    }
}
