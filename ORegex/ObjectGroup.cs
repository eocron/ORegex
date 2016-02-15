using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ORegex
{
    /// <summary>
    /// ORegex group definition
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    [DebuggerDisplay("success={IsSuccess}, index={Index}, length={Length};")]
    public class ObjectGroup<TValue> : ObjectCapture<TValue>
    {
        public readonly ObjectCaptureCollection<TValue> Captures;
        /// <summary>
        /// Is group catch successful
        /// </summary>
        public readonly bool IsSuccess;


        public override IEnumerable<TValue> Value
        {
            get
            {
                if (!IsSuccess)
                {
                    throw new InvalidOperationException("Match is not successful.");
                }
                for (int i = Index; i < Index + Length; i++)
                {
                    yield return _collection[i];
                }
            }
        }

        internal ObjectGroup(TValue[] collection, int index, int length, IEnumerable<ObjectCapture<TValue>> captures) : base(collection, index, length)
        {
            Captures = new ObjectCaptureCollection<TValue>();
            Captures.Add(this);
            foreach (var c in captures)
            {
                Captures.Add(c);
            }

            IsSuccess = Captures.Count > 0;
        }
    }
}
