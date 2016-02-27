using System.Collections.Generic;

namespace Eocron
{
    public sealed class CaptureTable<TValue>
    {
        private readonly Dictionary<string, List<OCapture<TValue>>> _captures = new Dictionary<string, List<OCapture<TValue>>>();

        public IEnumerable<OCapture<TValue>> this[string name]
        {
            get { return _captures[name]; }
        }

        internal void Add(string name, OCapture<TValue> capture)
        {
            List<OCapture<TValue>> list;
            if (!_captures.TryGetValue(name, out list))
            {
                list = new List<OCapture<TValue>>();
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
