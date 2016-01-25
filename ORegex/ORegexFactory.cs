using System;
using System.Text;

namespace ORegex
{
    public sealed class ORegexFactory
    {
        public static ObjectRegex<TValue> Create<TValue>(string[] patterns, ORegexOptions options,
            params Predicate<TValue>[] predicates)
        {
            var combined = CreatePattern(patterns);
            return new ObjectRegex<TValue>(combined, options, predicates);
        }

        public static ObjectRegex<TValue> Create<TValue>(string[] patterns, params Predicate<TValue>[] predicates)
        {
            return Create(patterns, ORegexOptions.None, predicates);
        }

        private static string CreatePattern(params string[] strs)
        {
            StringBuilder b = new StringBuilder();
            foreach (var str in strs)
            {
                if (b.Length > 0)
                {
                    b.Append('|');
                }
                b.Append('(');
                b.Append(str);
                b.Append(')');
            }
            return b.ToString();
        }
    }
}
