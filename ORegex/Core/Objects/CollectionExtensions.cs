using System;
using System.Collections;
using System.Collections.Generic;

namespace Eocron.Core.Objects
{
    public static class CollectionExtensions
    {
        public static IEnumerable<IEnumerable<T>> Batches<T>(this IEnumerable<T> collection, int batchSize)
        {
            var enumerator = collection.GetEnumerator();
            while (enumerator.MoveNext())
                yield return GetBatch(enumerator.Current, enumerator, batchSize);
        }
        public static bool IsNullOrEmpty<T>(this ICollection<T> collection)
        {
            if (collection != null)
                return collection.Count == 0;
            return true;
        }

        public static bool IsNullOrEmpty(this IEnumerable enumerable)
        {
            if (enumerable == null)
                return true;
            return !enumerable.GetEnumerator().MoveNext();
        }

        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var obj in enumerable)
            {
                action(obj);
            }
        }

        private static IEnumerable<T> GetBatch<T>(T firstElement, IEnumerator<T> enumerator, int batchSize)
        {
            yield return firstElement;
            for (var index = 1; index < batchSize && enumerator.MoveNext(); ++index)
                yield return enumerator.Current;
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

        public static IEnumerable<T> Single<T>(T element)
        {
            yield return element;
        }
    }
}