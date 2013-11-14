using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSCEngine.Utils
{
    public class TracedDictionary<K, V> : Dictionary<K, V>
    {
        private bool isTracing = false;
        private List<K> tracedStorage;

        public TracedDictionary()
        {
            this.tracedStorage = new List<K>();
        }

        public void BeginTrace()
        {
            this.isTracing = true;
            this.tracedStorage.Clear();
            this.tracedStorage.AddRange(this.Keys);
        }

        public void EndTrace()
        {
            this.isTracing = false;

            foreach (K key in this.tracedStorage)
            {
                this.Remove(key);
            }
        }

        public V GetAndMarkTracedObject(K key)
        {
            if (this.isTracing && this.tracedStorage.Contains(key))
            {
                this.tracedStorage.Remove(key);
            }

            return this[key];
        }
    }
}
