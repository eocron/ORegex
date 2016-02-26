using System.Collections.Generic;

namespace ORegex
{
    public sealed class CaptureTable<TValue>
    {
        private readonly Dictionary<string, List<ObjectCapture<TValue>>> _captures = new Dictionary<string, List<ObjectCapture<TValue>>>();

        public IEnumerable<ObjectCapture<TValue>> this[string name]
        {
            get { return _captures[name]; }
        }

        internal void Add(string name, ObjectCapture<TValue> capture)
        {
            List<ObjectCapture<TValue>> list;
            if (!_captures.TryGetValue(name, out list))
            {
                list = new List<ObjectCapture<TValue>>();
                _captures.Add(name,list);
            }
            list.Add(capture);
        }

        internal void Remove(string name)
        {
            _captures.Remove(name);
        }

        internal void Add(CaptureTable<TValue> table)
        {
            foreach (var c in table._captures)
            {
                foreach (var v in c.Value)
                {
                    Add(c.Key, v);
                }
            }
        }
    }
}
