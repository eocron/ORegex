using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace ORegex
{
    /// <summary>
    /// Object capture
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    [DebuggerDisplay("index={Index}, length={Length};")]
    public class ObjectCapture<TValue>: IEnumerable<TValue>
    {
        protected readonly TValue[] _collection;

        /// <summary>
        /// Start index
        /// </summary>
        public int Index { get; private set; }

        /// <summary>
        /// Length of capture
        /// </summary>
        public int Length { get; private set; }

        /// <summary>
        /// Value between end and start point
        /// </summary>
        public virtual IEnumerable<TValue> Value
        {
            get
            {
                for (int i = Index; i < Index + Length; i++)
                {
                    yield return _collection[i];
                }
            }
        }

        internal ObjectCapture(TValue[] collection, int index, int length)
        {
            Index = index;
            Length = length;
            _collection = collection;
        }

        public IEnumerator<TValue> GetEnumerator()
        {
            return Value.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
