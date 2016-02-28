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
        private readonly CFSA<TValue> _cfsa;
        private readonly ORegexOptions _options;
        public readonly string Pattern;

        public ORegex(string pattern, params Func<TValue,bool>[] predicates) : this(pattern, ORegexOptions.None, predicates){}

        public ORegex(string pattern, PredicateTable<TValue> table) : this(pattern, ORegexOptions.None, table) { }


        public ORegex(string pattern, ORegexOptions options, params Func<TValue, bool>[] predicates) : this(pattern, options, CreatePredicateTable(predicates)) { }

        public ORegex(string pattern, ORegexOptions options, PredicateTable<TValue> table)
        {
            _cfsa = _compiler.Build(pattern, table);
            _options = options;
            Pattern = pattern;
        }

        private static PredicateTable<TValue> CreatePredicateTable(Func<TValue, bool>[] predicates)
        {
            var table = new PredicateTable<TValue>();
            for (int i = 0; i < predicates.Length; i++)
            {
                table.AddPredicate(i.ToString(), predicates[i]);
            }
            return table;
        }

        public IEnumerable<OMatch<TValue>> Matches(TValue[] values, int startIndex = 0)
        {
            var captureTable = new OCaptureTable<TValue>();
            for (int i = startIndex; i < values.Length; i++)
            {
                var capture = _cfsa.Run(values, i, captureTable, true);
                if (!capture.Equals(Range.Invalid))
                {
                    var match = new OMatch<TValue>(values, captureTable, capture);
                    captureTable = new OCaptureTable<TValue>();
                    i += capture.Length - 1;

                    bool beginMatched = match.Index == startIndex;
                    bool endMatched = (match.Index + match.Length) == values.Length;

                    if (!_cfsa.ExactBegin && !_cfsa.ExactEnd ||
                        !(beginMatched ^ _cfsa.ExactBegin) && !(endMatched ^ _cfsa.ExactEnd))
                    {
                        yield return match;
                    }
                }
                if (_cfsa.ExactBegin)
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
            return !_cfsa.Run(values, startIndex, null, false).Equals(Range.Invalid);
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
