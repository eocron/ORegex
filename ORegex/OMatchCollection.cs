using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Eocron
{
    internal sealed class OMatchCollection<TValue> : IOMatchCollection<TValue>
    {
        private readonly List<OMatch<TValue>> _list;

        internal OMatchCollection(IEnumerable<OMatch<TValue>> enumerable)
        {
            _list = enumerable.ToList();
        }

        public IEnumerator<IOMatch<TValue>> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count => _list.Count;
    }
}
