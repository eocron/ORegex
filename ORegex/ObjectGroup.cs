//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Text.RegularExpressions;

//namespace ORegex
//{
//    /// <summary>
//    /// ORegex group definition
//    /// </summary>
//    /// <typeparam name="TValue"></typeparam>
//    [DebuggerDisplay("success={IsSuccess}, index={Index}, length={Length};")]
//    public class ObjectGroup<TValue> : ObjectCapture<TValue>
//    {
//        public readonly ObjectCapture<TValue>[] Captures;
//        /// <summary>
//        /// Is group catch successful
//        /// </summary>
//        public readonly bool IsSuccess;


//        public override IEnumerable<TValue> Value
//        {
//            get
//            {
//                if (!IsSuccess)
//                {
//                    throw new InvalidOperationException("Match is not successful.");
//                }
//                for (int i = Index; i < Index + Length; i++)
//                {
//                    yield return _collection[i];
//                }
//            }
//        }

//        internal ObjectGroup(TValue[] collection, int index, int length) : base(collection, index, length)
//        {
//            IsSuccess = group.Success;
//            Captures = group.Captures.Cast<Capture>().Select(x => new ObjectCapture<TValue>(x, collection, codeLength)).ToArray();
//        }
//    }
//}
