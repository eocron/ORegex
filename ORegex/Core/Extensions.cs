using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;

namespace ORegex.Core
{
    internal static class Extensions
    {
        public static string ThrowIfEmpty(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                throw new ArgumentNullException("String is empty.");
            }
            return str;
        }

        public static TValue ThrowIfNull<TValue>(this TValue value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("Value is null.");
            }
            return value;
        }

        public static HashSet<TValue> ToHashSet<TValue>(this IEnumerable<TValue> enumerable)
        {
            return new HashSet<TValue>(enumerable);
        }

        public static Set<TValue> ToSet<TValue>(this IEnumerable<TValue> enumerable)
        {
            return new Set<TValue>(enumerable);
        }
    }
}
