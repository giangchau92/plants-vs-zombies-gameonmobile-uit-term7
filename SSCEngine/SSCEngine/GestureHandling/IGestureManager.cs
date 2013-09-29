using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSCEngine.GestureHandling
{
    public interface IGestureManager
    {
        void AddGestureHandler(IGestureHandler ghandler);
        void RemoveGestureHandler(IGestureHandler ghandler);

        void Update(ITouchController touchController);
    }
}