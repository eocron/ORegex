using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Eocron.Core
{
    [Flags]
    public enum ComparisonOptions
    {
        None = 0,
        CheckArraysOrder = 1,
        CheckCollectionsOrder = 2
    }

    internal static class Extensions
    {
        public static bool DeepEquals<T>(this T obj1, T obj2, ComparisonOptions options = ComparisonOptions.None)
        {
            return DeepEqualsUnsafe(obj1, obj2, options.HasFlag(ComparisonOptions.CheckCollectionsOrder),
                options.HasFlag(ComparisonOptions.CheckArraysOrder));
        }

        private static bool DeepEqualsUnsafe(object obj1, object obj2, bool orderedCollectionComparison,
            bool orderedArrayComparison)
        {
            if (obj1 == null && obj2 == null)
                return true;
            if (obj1 == null || obj2 == null)
                return false;
            var type = obj1.GetType();
            if (type.IsValueType || type == typeof(string) || typeof(Delegate).IsAssignableFrom(type))
                return obj1.Equals(obj2);
            if (obj1 is IEnumerable && obj2 is IEnumerable)
            {
                var enumerable1 = (IEnumerable)obj1;
                var enumerable2 = (IEnumerable)obj2;
                if (!(!(obj1 is Array) || !(obj2 is Array) ? orderedCollectionComparison : orderedArrayComparison))
                    return AreEqual(enumerable1, enumerable2,
                        (a, b) => DeepEqualsUnsafe(a, b, orderedCollectionComparison, orderedArrayComparison));
                var enumerator1 = enumerable1.GetEnumerator();
                var enumerator2 = enumerable2.GetEnumerator();
                bool flag1;
                bool flag2;
                do
                {
                    flag1 = enumerator1.MoveNext();
                    flag2 = enumerator2.MoveNext();
                    if (!flag1 || !flag2)
                        goto label_12;
                } while (DeepEqualsUnsafe(enumerator1.Current, enumerator2.Current, orderedCollectionComparison,
                    orderedArrayComparison));
                return false;
            label_12:
                if (flag1 || flag2)
                    return false;
            }
            else
            {
                var properties = type.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField);
                foreach (var propertyInfo in properties)
                {
                    if (propertyInfo.CanRead &&
                        !DeepEqualsUnsafe(propertyInfo.GetValue(obj1, null), propertyInfo.GetValue(obj2, null), orderedCollectionComparison, orderedArrayComparison))
                        return false;
                }
            }
            return true;
        }

        public static bool EqualCount(IEnumerable enumerable1, IEnumerable enumerable2, out int count)
        {
            count = 0;
            if (enumerable1 == null && enumerable2 == null)
                return true;
            if (enumerable1 == null || enumerable2 == null)
                return false;
            var enumerator1 = enumerable1.GetEnumerator();
            var enumerator2 = enumerable2.GetEnumerator();
            while (true)
            {
                var flag1 = enumerator1.MoveNext();
                var flag2 = enumerator2.MoveNext();
                if (!(flag1 ^ flag2))
                {
                    if (flag1)
                        ++count;
                    else
                        goto label_8;
                }
                else
                    break;
            }
            count = 0;
            return false;
        label_8:
            return true;
        }

        public static bool AreEqual(IEnumerable enumerable1, IEnumerable enumerable2, Func<object, object, bool> equals)
        {
            int count;
            if (!EqualCount(enumerable1, enumerable2, out count))
                return false;
            if (count == 0)
                return true;
            var hashSet = new HashSet<int>();
            foreach (var obj1 in enumerable1)
            {
                var flag = false;
                var num = 0;
                foreach (var obj2 in enumerable2)
                {
                    if (!hashSet.Contains(num) && equals(obj1, obj2))
                    {
                        flag = true;
                        hashSet.Add(num);
                        break;
                    }
                    ++num;
                }
                if (!flag)
                    return false;
            }
            return true;
        }

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

        public static SortedSet<TValue> ToSortedSet<TValue>(this IEnumerable<TValue> enumerable)
        {
            return new SortedSet<TValue>(enumerable);
        }

        public static Set<TValue> ToSet<TValue>(this IEnumerable<TValue> enumerable)
        {
            return new Set<TValue>(enumerable);
        }
    }
}
