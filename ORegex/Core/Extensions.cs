using System;
using System.Collections.Generic;

namespace Eocron.Core
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
