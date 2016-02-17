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
        public ObjectCaptureCollection<TValue> Captures { get; set; }

        /// <summary>
        /// Is group catch successful
        /// </summary>
        public bool IsSuccess
        {
            get { return Length >= 0; }
        }


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

        internal ObjectGroup(TValue[] collection, int index, int length) : base(collection, index, length)
        {
        }
    }
}
