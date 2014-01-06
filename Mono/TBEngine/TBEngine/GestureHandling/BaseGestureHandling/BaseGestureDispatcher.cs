using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCSEngine.GestureHandling.BaseGestureHandling
{
    class BaseGestureDispatcher : IGestureDispatcher
    {
        private IDictionary<int, IStrongTargetStorage> adapters = new Dictionary<int, IStrongTargetStorage>();
        public BaseGestureDispatcher()
        {
            this.Enabled = true;
        }

        public void AddTarget<GE>(IGestureTarget<GE> gTarget) where GE : IGestureEvent
        {
            int geHash = typeof(GE).GetHashCode();
            if (!this.adapters.ContainsKey(geHash))
            {
                this.adapters.Add(geHash, new StrongTargetStorage<GE>());
            }

            StrongTargetStorage<GE> targetStorage = this.adapters[geHash] as StrongTargetStorage<GE>;
            targetStorage.Add(gTarget);
        }

        public void RemoveTarget<GE>(IGestureTarget<GE> gTarget) where GE : IGestureEvent
        {
            int geHash = typeof(GE).GetHashCode();
            if (this.adapters.ContainsKey(geHash))
            {
                StrongTargetStorage<GE> targetStorage = this.adapters[geHash] as StrongTargetStorage<GE>;
                targetStorage.Remove(gTarget);

                if (targetStorage.Count == 0)
                {
                    this.adapters.Remove(geHash);
                }
            }

        }

        public void Dispatch<GestureEvent>(GestureEvent gEvent) where GestureEvent : IGestureEvent
        {
            int geHash = typeof(GestureEvent).GetHashCode();
            if (this.adapters.ContainsKey(geHash))
            {
                StrongTargetStorage<GestureEvent> targetStorage = this.adapters[geHash] as StrongTargetStorage<GestureEvent>;

                if (targetStorage.HandledTargets.ContainsKey(gEvent.Touch.TouchID))
                {
                    IGestureTarget<GestureEvent> target = targetStorage.HandledTargets[gEvent.Touch.TouchID];
                    if (!target.ReceivedGesture(gEvent))
                    {
                        targetStorage.HandledTargets.Remove(gEvent.Touch.SystemTouch.Id);
                    }
                }
                else
                {
                    IEnumerable<IGestureTarget<GestureEvent>> copyTargets = new LinkedList<IGestureTarget<GestureEvent>>(targetStorage);

                    foreach (var target in copyTargets)
                    {
                        if (target.IsHandleGesture(gEvent))
                        {
                            if (target.ReceivedGesture(gEvent))
                            {
                                targetStorage.HandledTargets.Add(gEvent.Touch.TouchID, target);
                                break;
                            }
                        }
                    }
                }
            }
        }

        private class StrongTargetStorage<GE> : List<IGestureTarget<GE>>, IStrongTargetStorage where GE : IGestureEvent
        {
            private Dictionary<int, IGestureTarget<GE>> handledTargets = new Dictionary<int, IGestureTarget<GE>>();
            public Dictionary<int, IGestureTarget<GE>> HandledTargets
            {
                get { return handledTargets; }
            }
        }

        private interface IStrongTargetStorage
        {
        }


        public bool Enabled { get; set; }


        public IGestureTarget<GestureEvent> HandleTarget<GestureEvent>(GestureEvent e) where GestureEvent : IGestureEvent
        {
            int geHash = typeof(GestureEvent).GetHashCode();
            if (this.adapters.ContainsKey(geHash))
            {
                StrongTargetStorage<GestureEvent> targetStorage = this.adapters[geHash] as StrongTargetStorage<GestureEvent>;
                if (targetStorage.HandledTargets.ContainsKey(e.Touch.TouchID))
                {
                    return targetStorage.HandledTargets[e.Touch.TouchID];
                }
            }

            return null;
        }

        public void SetHandleTarget<GestureEvent>(GestureEvent e, IGestureTarget<GestureEvent> gTarget) where GestureEvent : IGestureEvent
        {
            int geHash = typeof(GestureEvent).GetHashCode();
            if (this.adapters.ContainsKey(geHash))
            {
                StrongTargetStorage<GestureEvent> targetStorage = this.adapters[geHash] as StrongTargetStorage<GestureEvent>;
                if (targetStorage.HandledTargets.ContainsKey(e.Touch.TouchID))
                {
                    targetStorage.HandledTargets[e.Touch.TouchID] = gTarget;
                }
                else
                {
                    targetStorage.HandledTargets.Add(e.Touch.TouchID, gTarget);
                }
            }
        }
    }
}
