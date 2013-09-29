using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSCEngine.GestureHandling
{
    public interface IGestureDispatcher<GestureHandler> where GestureHandler : IGestureHandler
    {
        void AddTarget(IGestureTarget<GestureHandler> gTarget);
        void RemoveTarget(IGestureTarget<GestureHandler> gTarget);

        void Dispatch(GestureHandler gHandler);
    }
}