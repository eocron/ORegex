using System.Collections.Generic;
using Eocron.Core.Ast;

namespace Eocron.Core
{
    public struct SequenceHandler<TValue>
    {
        private readonly IList<TValue> _collection;

        public bool Reverse;

        public SequenceHandler(IList<TValue> collection)
        {
            _collection = collection;
            Reverse = false;
        }

        public IList<TValue> Collection => _collection;

        public int Count => _collection.Count;

        public TValue this[int i]
        {
            get
            {
                i = Invert(i);
                return _collection[i];
            }
        }

        public int Invert(int i)
        {
            return Reverse ? _collection.Count - i - 1 : i;
        }

        /// <summary>
        /// Translate imaginary range to real input sequence range.
        /// </summary>
        /// <param name="range"></param>
        /// <returns></returns>
        public Range Translate(Range range)
        {
            if (Reverse)
            {
                var index = Invert(range.Index) - range.Length + 1;
                return new Range(index, range.Length);
            }
            return range;
        }
    }
}
