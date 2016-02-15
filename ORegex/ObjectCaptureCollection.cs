using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ORegex
{
    public sealed class ObjectCaptureCollection<TValue> : IEnumerable<ObjectCapture<TValue>>
    {
        private readonly List<ObjectCapture<TValue>> _captures;

        internal ObjectCaptureCollection()
        {
            _captures = new List<ObjectCapture<TValue>>();
        }

        public int Count
        {
            get { return _captures.Count; }
        }

        public ObjectCapture<TValue> this[int i]
        {
            get { return _captures[i]; }
        }

        internal void Add(ObjectCapture<TValue> group)
        {
            _captures.Add(group);
        }

        public IEnumerator<ObjectCapture<TValue>> GetEnumerator()
        {
            return _captures.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
