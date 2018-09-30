using System.Collections;
using System.Collections.Generic;
using Eocron.Core;
using Eocron.Core.Ast;

namespace Eocron
{
    internal class OCapture<TValue> : IOCapture<TValue>
    {
        private readonly IList<TValue> _collection;

        public int Index { get; }
        public int Length { get; }

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
