using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using SCSEngine.GestureHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantsVsZombies.Orientations
{
    public class OrientedTouchController : ITouchController
    {
        private ITouchController baseTC;
        private GameOrientation orientation;

        public OrientedTouchController(ITouchController baseTouchController, GameOrientation ori)
        {
            this.baseTC = baseTouchController;
            this.orientation = ori;
        }

        public ICollection<ITouch> Touches
        {
            get { return baseTC.Touches; }
        }

        public void Update(TouchCollection toucheCollection, GameTime gameTime)
        {
            TouchLocation[] touchLocs = new TouchLocation[toucheCollection.Count];
            int i = 0;
            foreach (var touch in toucheCollection)
            {
                touchLocs[i++] = new TouchLocation(touch.Id, touch.State, orientation.Transform(touch.Position));
            }

            baseTC.Update(new TouchCollection(touchLocs), gameTime);
        }
    }
}