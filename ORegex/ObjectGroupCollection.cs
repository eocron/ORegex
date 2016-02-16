using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ORegex.Core.FinitieStateAutomaton;

namespace ORegex
{
    [DebuggerDisplay("Count = {Count}")]
    public sealed class ObjectGroupCollection<TValue> : IEnumerable<ObjectGroup<TValue>>
    {
        private readonly Dictionary<string, ObjectGroup<TValue>> _nameToGroup;

        private readonly ObjectGroup<TValue>[] _captures;

        internal ObjectGroupCollection(CFSAContext<TValue> context)
        {
            _nameToGroup = new Dictionary<string, ObjectGroup<TValue>>();
            _captures = new ObjectGroup<TValue>[context._captures.Length];
        }

        public int Count
        {
            get
            {
                return _nameToGroup.Count;
            }
        }

        public ObjectGroup<TValue> this[string name]
        {
            get
            {
                return _nameToGroup[name];
            }
        }

        public ObjectGroup<TValue> this[int index]
        {
            get { return _captures[index]; }
        }


        internal void Add(string name, ObjectGroup<TValue> group)
        {
            _nameToGroup.Add(name, group);
        }

        internal void Add(int id, ObjectGroup<TValue> group)
        {
            _captures[id].Captures.Add(group);
        }

        public IEnumerator<ObjectGroup<TValue>> GetEnumerator()
        {
            return _nameToGroup.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
