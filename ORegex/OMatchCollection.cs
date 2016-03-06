using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eocron
{
    public sealed class OMatchCollection<TValue> : IEnumerable<OMatch<TValue>>
    {
        private static readonly List<OMatch<TValue>> Empty = new List<OMatch<TValue>>(); 
        private List<OMatch<TValue>> _matches;

        public int Count
        {
            get { return (_matches?? Empty).Count; }
        }

        internal void Add(OMatch<TValue> match)
        {
            if (_matches == null)
            {
                _matches = new List<OMatch<TValue>>();
            }
            _matches.Add(match);
        }


        public IEnumerator<OMatch<TValue>> GetEnumerator()
        {
            return (_matches ?? Empty).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
