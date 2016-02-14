using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ORegex.Core
{
    public sealed class ObjectStream<TValue>
    {
        private readonly TValue[] _sequence;

        private int _currentPosition;

        public int CurrentIndex
        {
            get { return _currentPosition; }
            set { _currentPosition = value; }
        }

        public ObjectStream(IEnumerable<TValue> sequence)
        {
            _sequence = sequence.ToArray();
        }

        public void Step()
        {
            _currentPosition++;
        }

        public TValue CurrentElement
        {
            get { return _sequence[CurrentIndex]; }
        }

        public bool IsEos()
        {
            return _currentPosition >= _sequence.Length;
        }


    }
}
