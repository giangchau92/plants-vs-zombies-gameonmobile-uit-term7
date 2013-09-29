using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSCEngine.GestureHandling.Implements
{
    public class GestureDispatcherFactory
    {
        public IGestureDispatcher<GestureHandling> CreateDispatcher<GestureHandling>() where GestureHandling : IGestureHandler
        {
            return new BaseGestureHandling.BaseGestureDispatcher<GestureHandling>();
        }
    }
}
