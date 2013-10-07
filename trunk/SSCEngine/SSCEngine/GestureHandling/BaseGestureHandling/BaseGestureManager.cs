using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSCEngine.GestureHandling.BaseGestureHandling
{
    class BaseGestureManager : GameComponent, IGestureManager
    {
        private List<IGestureDetector> detectors = new List<IGestureDetector>();
        private List<IGestureDispatcher> dispatchers = new List<IGestureDispatcher>();

        private ITouchController touchController;

        public BaseGestureManager(Game game, ITouchController touchController) :
            base(game)
        {
            this.touchController = touchController;
        }

        public void AddDetector(IGestureDetector gDetector)
        {
            this.detectors.Add(gDetector);
        }

        public void RemoveDetector(IGestureDetector gDetector)
        {
            this.detectors.Remove(gDetector);
        }

        public void AddDispatcher(IGestureDispatcher gDispatcher)
        {
            this.dispatchers.Add(gDispatcher);
        }

        public void RemoveDispatcher(IGestureDispatcher gDispatcher)
        {
            this.dispatchers.Remove(gDispatcher);
        }

        public override void Update(GameTime gameTime)
        {
            this.touchController.Update(TouchPanel.GetState(), gameTime);
            ICollection<ITouch> touches = this.touchController.Touches;

            List<IGestureEvent> gestures = new List<IGestureEvent>();
            foreach (var detector in this.detectors)
            {
                gestures.AddRange(detector.DetectGesture(touches));
            }

            var copyDispatchers = new List<IGestureDispatcher>(this.dispatchers);
            foreach (var dispatcher in copyDispatchers)
            {
                foreach (var gesture in gestures)
                {
                    dispatcher.Dispatch(gesture);
                }
            }

            base.Update(gameTime);
        }
    }
}
