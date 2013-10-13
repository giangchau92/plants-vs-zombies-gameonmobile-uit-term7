using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSCEngine.GestureHandling.BaseGestureHandling
{
    class BaseGestureDispatcher : Dictionary<Type, List<IGestureTarget>>, IGestureDispatcher
    {
        private Dictionary<Type, IDispatchAdapter> adapters = new Dictionary<Type,IDispatchAdapter>();

        public BaseGestureDispatcher()
        {
            this.Enabled = true;
        }

        public void AddTarget<GE>(IGestureTarget<GE> gTarget) where GE : IGestureEvent
        {
            if (!this.ContainsKey(typeof(GE)))
            {
                this.Add(typeof(GE), new List<IGestureTarget>());
                this.adapters.Add(typeof(GE), new DispatchAdapter<GE>());
            }

            this[typeof(GE)].Add(gTarget);
        }

        public void RemoveTarget<GE>(IGestureTarget<GE> gTarget) where GE : IGestureEvent
        {
            if (this.ContainsKey(typeof(GE)))
            {
                this[typeof(GE)].Remove(gTarget);

                if (this[typeof(GE)].Count == 0)
                {
                    this.Remove(typeof(GE));
                    this.adapters.Remove(typeof(GE));
                }
            }
        }

        public void Dispatch(IGestureEvent gEvent)
        {
            if (this.ContainsKey(gEvent.GetType()) && this.adapters.ContainsKey(gEvent.GetType()))
            {
                // dispatch gesture
                var dispatchAdapter = this.adapters[gEvent.GetType()];
                IEnumerable<IGestureTarget> targets = new List<IGestureTarget>(this[gEvent.GetType()]);
                dispatchAdapter.Dispatch(gEvent, targets);

                // remove completed targets
                var realTargets = this[gEvent.GetType()];
                realTargets.Clear();
                foreach (var target in targets)
                {
                    if (!target.IsGestureCompleted)
                    {
                        realTargets.Add(target);
                    }
                }
            }
        }

        private class DispatchAdapter<GE> : IDispatchAdapter where GE : IGestureEvent
        {
            private Dictionary<int, IGestureTarget<GE>> handledTargets = new Dictionary<int, IGestureTarget<GE>>();
            private Dictionary<IGestureTarget<GE>, GestureHandleType> lastHandleTypes = new Dictionary<IGestureTarget<GE>, GestureHandleType>();

            public void Dispatch(IGestureEvent e, IEnumerable<IGestureTarget> targets)
            {
                var ge = (GE) e;
                if (ge == null)
                    return;

                if (this.handledTargets.ContainsKey(e.Touch.SystemTouch.Id))
                {
                    IGestureTarget<GE> target = this.handledTargets[e.Touch.SystemTouch.Id];
                    GestureHandleType handleType = target.CheckHandleGesture(ge, GestureHandleType.Exclusive);
                    if (handleType != GestureHandleType.None)
                    {
                        target.ReceivedGesture(ge);
                    }

                    if (handleType != GestureHandleType.Exclusive)
                    {
                        this.handledTargets.Remove(e.Touch.SystemTouch.Id);
                    }
                }
                else
                {
                    foreach (var target in targets)
                    {
                        var specTarget = target as IGestureTarget<GE>;
                        switch (specTarget.CheckHandleGesture(ge))
                        {
                            case GestureHandleType.Handled:
                                specTarget.ReceivedGesture(ge);
                                break;
                            case GestureHandleType.Exclusive:
                                this.handledTargets.Add(e.Touch.SystemTouch.Id, specTarget);
                                specTarget.ReceivedGesture(ge);
                                return;
                            case GestureHandleType.None:
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }

        private interface IDispatchAdapter
        {
            void Dispatch(IGestureEvent e, IEnumerable<IGestureTarget> targets);
        }


        public bool Enabled { get; set; }
    }
}
