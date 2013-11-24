using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SCSEngine.GestureHandling.BaseGestureHandling
{
    class BaseGestureManager : GameComponent, IGestureManager
    {
        private IDictionary<int, IStrongGestureManager> subManager = new Dictionary<int, IStrongGestureManager>();   // key is gesture event hash
        private List<IGestureDispatcher> dispatchers = new List<IGestureDispatcher>();

        private ITouchController touchController;

        public BaseGestureManager(Game game, ITouchController touchController) :
            base(game)
        {
            this.touchController = touchController;
        }

        public void AddDetector<GestureEvent>(IGestureDetector<GestureEvent> gDetector) where GestureEvent : IGestureEvent
        {
            this.subManager.Add(gDetector.GestureEventHash , new StrongGestureManager<GestureEvent>(gDetector, this.dispatchers));
        }

        public void RemoveDetector<GestureEvent>(IGestureDetector<GestureEvent> gDetector) where GestureEvent : IGestureEvent
        {
            this.subManager.Remove(gDetector.GestureEventHash);
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

            List<IGestureEvent> gestures = new List<IGestureEvent>(); // info ex: freedrag class
            // Gom nhóm và tạo các gesture
            foreach (var man in this.subManager)
            {
                man.Value.Update(touches, gameTime);
            }

            base.Update(gameTime);
        }
    }
}
