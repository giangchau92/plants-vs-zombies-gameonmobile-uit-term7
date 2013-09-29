using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSCEngine.GestureHandling.BaseGestureHandling
{
    class BaseGestureDispatcher<GH> : List<IGestureTarget<GH>>, IGestureDispatcher<GH> where GH : IGestureHandler
    {
        public void AddTarget(IGestureTarget<GH> gTarget)
        {
            this.Add(gTarget);
        }

        public void RemoveTarget(IGestureTarget<GH> gTarget)
        {
            this.Remove(gTarget);
        }

        public void Dispatch(GH gHandler)
        {
            List<IGestureTarget<GH>> targets = new List<IGestureTarget<GH>>(this);
            targets.Sort(IGestureTarget<GH>.Comparer.Instance);

            foreach (IGestureTarget<GH> target in targets)
            {
                target.ReceivedGesture(gHandler);
            }

            for (int i = 0; i < this.Count; )
            {
                if (this.ElementAt(i).IsGestureCompleted)
                {
                    this.RemoveAt(i);
                }
                else
                {
                    ++i;
                }
            }
        }
    }
}
