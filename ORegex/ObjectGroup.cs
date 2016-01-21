using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace ORegex
{
    [DebuggerDisplay("success={IsSuccess}, index={Index}, length={Length};")]
    public class ObjectGroup<TValue> : IEnumerable<TValue>
    {
        private readonly TValue[] _collection;

        public readonly bool IsSuccess;

        public readonly int Index;

        public readonly int Length;

        public IEnumerable<TValue> Value
        {
            get
            {
                if (!IsSuccess)
                {
                    throw new InvalidOperationException("Match is not successful.");
                }
                for (int i = Index; i < Index+Length; i++)
                {
                    yield return _collection[i];
                }
            }
        }

        internal ObjectGroup(Group group, TValue[] collection, int codeLength)
        {
            _collection = collection;
            Index = group.Index/codeLength;
            Length = group.Length/codeLength;
            IsSuccess = group.Success;
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
