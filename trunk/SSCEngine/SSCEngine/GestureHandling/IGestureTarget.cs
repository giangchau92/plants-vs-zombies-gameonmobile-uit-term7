using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSCEngine.GestureHandling
{
    public class GestureTargetComparer : IComparer<IGestureTarget>
    {
        private GestureTargetComparer()
        {
        }

        public static GestureTargetComparer Instance { get; private set; }

        static GestureTargetComparer()
        {
            Instance = new GestureTargetComparer();
        }

        #region IComparer<IGestureTarget> Members

        public int Compare(IGestureTarget x, IGestureTarget y)
        {
            if (x.Priority > y.Priority)
                return 1;
            else if (x.Priority == y.Priority)
                return 0;

            return -1;
        }

        #endregion
    }

    public interface IGestureTarget
    {
        uint Priority { get; }

        bool IsGestureCompleted { get; }
    }

    public enum GestureHandleType
    {
        None,
        Handled,
        Exclusive
    }

    public interface IGestureTarget<GestureEvent> : IGestureTarget where GestureEvent : IGestureEvent
    {
        void ReceivedGesture(GestureEvent gEvent);
        GestureHandleType CheckHandleGesture(GestureEvent gEvent, GestureHandleType lastHandleType);
    }
}
