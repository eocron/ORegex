using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ORegex
{
    /// <summary>
    /// Objected regex.  Very useful for simple tasks with a little amount of predicates.
    /// For use just type {number} instead of character like you usualliy write regex.
    /// Number can be from 0 to MaxPredicatesCount-1.
    /// Example of pattern: {0}(.{0})* where {0} -> isPrime(x) on sequence: 1 2 3 4 5 6 7 8 9 10 11 12 13
    /// Syntax to write patterns is RegularExpressions.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    [DebuggerDisplay("{_pattern.ToString()}")]
    public sealed class ObjectRegex<TValue>
    {
        private readonly Predicate<TValue>[] _predicates;
        private readonly Regex _pattern;
        private readonly RegexObjectMapper _mapper;

        public bool IgnoreRedundantPredicates = false;

        public ObjectRegex(string str, params Predicate<TValue>[] predicates)
        {
            if (predicates == null || predicates.Length == 0)
            {
                throw new ArgumentException("Predicates is empty.", "predicates");
            }
            if (predicates.Distinct().Count() != predicates.Length)
            {
                throw new ArgumentOutOfRangeException("predicates", "Predicates repeating");
            }
            _predicates = predicates;
            _mapper = new RegexObjectMapper(_predicates.Length);

            str = PrecompilePattern(str);
            _pattern = new Regex(str, RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.ExplicitCapture);

        }

        private string PrecompilePattern(string str)
        {
            var matches =
                Regex.Matches(str, @"\{(\d+)\}")
                    .Cast<Match>()
                    .Select(x => int.Parse(x.Groups[1].Value))
                    .Distinct()
                    .ToArray();

            bool[] predicatesUsage = null;
            if (!IgnoreRedundantPredicates)
            {
                predicatesUsage = new bool[_predicates.Length];
            }
            foreach (var match in matches)
            {
                if (!_mapper.IsExist(match))
                {
                    throw new ArgumentOutOfRangeException("str",
                        "Index described in pattern should point to predicate - " + match);
                }
                if (!IgnoreRedundantPredicates)
                {
                    predicatesUsage[match] = true;
                }
                str = str.Replace("{" + match + "}", _mapper.GetStateString(match));
            }
            if (!IgnoreRedundantPredicates)
            {
                StringBuilder b = new StringBuilder();
                for (int i = 0; i < predicatesUsage.Length; i++)
                {
                    if (!predicatesUsage[i])
                    {
                        if (b.Length > 0)
                        {
                            b.Append(",");
                        }
                        b.Append(i);
                    }
                }
                if (b.Length > 0)
                {
                    throw new ArgumentException("predicates", "Redundant predicates: " + b.ToString());
                }
            }
            return str;
        }

        public IEnumerable<ObjectMatch<TValue>> Matches(TValue[] values)
        {
            var str = GetStateString(values);
            var matches = _pattern.Matches(str);
            foreach (Match match in matches)
            {
                yield return CreateMatch(match, values);
            }
        }

        public ObjectMatch<TValue> Match(TValue[] values)
        {
            var str = GetStateString(values);
            var match = _pattern.Match(str);
            return CreateMatch(match, values);
        }

        public bool IsMatch(TValue[] values)
        {
            var str = GetStateString(values);
            return _pattern.IsMatch(str);
        }

        public IEnumerable<ObjectMatch<TValue>> Matches(TValue[] values, int startat)
        {
            var str = GetStateString(values);
            var matches = _pattern.Matches(str, GetStartAt(startat));
            foreach (Match match in matches)
            {
                yield return CreateMatch(match, values);
            }
        }

        public ObjectMatch<TValue> Match(TValue[] values, int startat)
        {
            var str = GetStateString(values);
            var match = _pattern.Match(str, GetStartAt(startat));
            return CreateMatch(match, values);
        }

        public bool IsMatch(TValue[] values, int startat)
        {
            var str = GetStateString(values);
            return _pattern.IsMatch(str, GetStartAt(startat));
        }

        private ObjectMatch<TValue> CreateMatch(Match match, TValue[] values)
        {
            return new ObjectMatch<TValue>(_pattern, match, values, _mapper.CodeLength);
        }

        private string GetStateString(TValue[] values)
        {
            int[] states = new int[values.Length];
            for (int i = 0; i < states.Length; i++)
            {
                states[i] = -1;
                for (int j = 0; j < _predicates.Length; j++)
                {
                    if (_predicates[j](values[i]))
                    {
                        states[i] = j;
                        break;
                    }
                }
            }
            return _mapper.GetStateString(states);
        }

        private int GetStartAt(int index)
        {
            return _mapper.ConvertIndex(index);
        }
    }
}
