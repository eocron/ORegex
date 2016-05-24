using System.Collections;
using System.Collections.Generic;
using Eocron.Core;
using Eocron.Core.Ast;

namespace Eocron
{
    public class OCapture<TValue> : IEnumerable<TValue>
    {
        private readonly TValue[] _collection;

        public readonly int Index;

        public readonly int Length;

        public IEnumerable<TValue> Values
        {
            get
            {
                var count = Index + Length;
                for (int i = Index; i < count; i++)
                {
                    yield return _collection[i];
                }
            }
        }

        internal OCapture(SequenceHandler<TValue> handler, Range range)
        {
            range = handler.Translate(range);
            Index = range.Index;
            Length = range.Length;
            _collection = handler.Collection;
        }

        public IEnumerator<TValue> GetEnumerator()
        {
            return Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
