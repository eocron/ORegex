using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework.Compatibility;

namespace Tests.Core
{
    public static class Extensions
    {
        /// <summary>
        /// Evaluates IEnumerable. Use for test purposes only.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        public static void Evaluate<T>(this IEnumerable<T> enumerable)
        {
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            enumerable.ToArray();
        }

        public static Stopwatch Measure(Action action)
        {
            var sw = new Stopwatch();
            try
            {
                sw.Restart();
                action();
            }
            finally 
            {
                sw.Stop();
            }
            return sw;
        }
    }
}
