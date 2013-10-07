using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSCEngine.GestureHandling
{
    public class DefaultGestureHandlingFactory : SSCEngine.GestureHandling.IDefaultGestureHandlingFactory
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
    }
}
