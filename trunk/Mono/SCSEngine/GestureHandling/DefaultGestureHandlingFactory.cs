using Microsoft.Xna.Framework;
using SCSEngine.GestureHandling.Implements.Detectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCSEngine.GestureHandling
{
    public class DefaultGestureHandlingFactory : SCSEngine.GestureHandling.IGestureHandlingFactory
    {
        private DefaultGestureHandlingFactory()
        {
        }

        public static DefaultGestureHandlingFactory Instance { get; private set; }

        static DefaultGestureHandlingFactory()
        {
            Instance = new DefaultGestureHandlingFactory();
        }

        public IGestureDispatcher CreateDispatcher()
        {
            return new BaseGestureHandling.BaseGestureDispatcher();
        }

        public IGestureManager CreateManager(Game game)
        {
            return new BaseGestureHandling.BaseGestureManager(game, new BaseGestureHandling.TouchController());
        }

        public void InitDetectors(IGestureManager gMan)
        {
            gMan.AddDetector(new FreeDragDetector());
            gMan.AddDetector(new TapDetector());
            gMan.AddDetector(new FreeTapDetector());
        }
    }
}
