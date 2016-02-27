using System;
using System.Collections;
using System.Reflection;

namespace Eocron.Core.Objects
{
    public static class ComparisonExtensions
    {
        public static bool DeepEquals<T>(T obj1, T obj2, ComparisonOptions options = ComparisonOptions.None)
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
                var enumerable1 = (IEnumerable) obj1;
                var enumerable2 = (IEnumerable) obj2;
                if (!(!(obj1 is Array) || !(obj2 is Array) ? orderedCollectionComparison : orderedArrayComparison))
                    return CollectionExtensions.AreEqual(enumerable1, enumerable2,
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
    }
}