using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Eocron
{
    public sealed class OMatchCollection<TValue> : IReadOnlyCollection<OMatch<TValue>>
    {
        private readonly List<OMatch<TValue>> _list;

        internal OMatchCollection(IEnumerable<OMatch<TValue>> enumerable)
        {
            _list = enumerable.ToList();
        }

        public IEnumerator<OMatch<TValue>> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count
        {
            get { return _list.Count; }
        }
    }
}
