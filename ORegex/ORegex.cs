using System;
using System.Collections.Generic;
using System.Linq;
using Eocron.Core.Ast;
using Eocron.Core.FinitieStateAutomaton;
using Eocron.Core.Parse;

namespace Eocron
{
    /// <summary>
    /// Objected regex.  Very useful for simple tasks with a little amount of predicates.
    /// For use just type {number} instead of character like you usualliy write regex.
    /// Number can be from 0 to MaxPredicatesCount-1.
    /// Example of pattern: {0}(.{0})* where {0} -> isPrime(x) on sequence: 1 2 3 4 5 6 7 8 9 10 11 12 13
    /// Syntax to write patterns is RegularExpressions.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public class ORegex<TValue>
    {
        private readonly ORegexCompiler<TValue> _compiler = new ORegexCompiler<TValue>();

        private readonly IFSA<TValue> _fa;

        public readonly ORegexOptions Options;

        public readonly string Pattern;

        public ORegex(string pattern, params Func<TValue,bool>[] predicates) : this(pattern, ORegexOptions.None, predicates){}

        public ORegex(string pattern, ORegexOptions options, params Func<TValue, bool>[] predicates) : this(pattern, options, CreatePredicateTable(predicates)) { }

        public ORegex(string pattern, PredicateTable<TValue> table) : this(pattern, ORegexOptions.None, table) { }

        public ORegex(string pattern, ORegexOptions options, PredicateTable<TValue> table)
        {
            _fa = _compiler.Build(pattern, table);
            Options = options;
            Pattern = pattern;
        }

        private static PredicateTable<TValue> CreatePredicateTable(Func<TValue, bool>[] predicates)
        {
            if (predicates == null || predicates.Length == 0)
            {
                throw new ArgumentException("No predicates provided.");
            }
            var table = new PredicateTable<TValue>();
            for (int i = 0; i < predicates.Length; i++)
            {
                table.AddPredicate(i.ToString(), predicates[i]);
            }
            return table;
        }

        public IEnumerable<OMatch<TValue>> Matches(TValue[] values, int startIndex = 0)
        {
            Range range;
            var captureTable = new OCaptureTable<TValue>();
            for (int i = startIndex; i <= values.Length; i++)
            {
                if (_fa.TryRun(values, i, out range))
                {
                    var match = new OMatch<TValue>(values, captureTable, range);
                    captureTable.Add(_fa.Name, match);
                    captureTable = new OCaptureTable<TValue>();

                    bool beginMatched = match.Index == startIndex;
                    bool endMatched = (match.Index + match.Length) == values.Length;

                    if (!_fa.ExactBegin && !_fa.ExactEnd ||
                        !(beginMatched ^ _fa.ExactBegin) && !(endMatched ^ _fa.ExactEnd))
                    {
                        yield return match;
                    }

                    i += range.Length == 0 ? 0 : range.Length - 1;
                }
                if (_fa.ExactBegin)
                {
                    break;
                }
            }
        }

        public OMatch<TValue> Match(TValue[] values, int startIndex = 0)
        {
            return Matches(values, startIndex).FirstOrDefault();
        }

        public bool IsMatch(TValue[] values, int startIndex = 0)
        {
            Range range;
            for (int i = startIndex; i <= values.Length; i++)
            {
                if (_fa.TryRun(values, i, out range))
                {
                    return true;
                }
                if (_fa.ExactBegin)
                {
                    break;
                }
            }
            return false;
        }

        public TValue[] Replace(TValue[] values, int startIndex = 0)
        {
            var matches = Matches(values, startIndex);
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return Pattern;
        }
    }
}
