using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSCEngine.GestureHandling
{
    public interface IGestureTarget<GestureHandler> where GestureHandler : IGestureHandler
    {
        uint Priority { get; }
        bool IsExclusive { get; }

        void ReceivedGesture(GestureHandler gHandler);

        bool IsGestureCompleted { get; }

        public class Comparer : IComparer<IGestureTarget<GestureHandler>>
        {
            private Comparer()
            {
            }

            public static Comparer Instance { get; private set; }

            static Comparer()
            {
                Instance = new Comparer();
            }

            #region IComparer<IGestureTarget<GestureHandler>> Members

            public int Compare(IGestureTarget<GestureHandler> x, IGestureTarget<GestureHandler> y)
            {
                if (x.Priority > y.Priority)
                    return 1;
                else if (x.Priority == y.Priority)
                    return 0;

                return -1;
            }

            #endregion
        }
    }
}
