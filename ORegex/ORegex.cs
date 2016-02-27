using Eocron.Core.Parse;
using System;
using System.Collections.Generic;
using System.Linq;
using Eocron;
using Eocron.Core;
using Eocron.Core.Ast;
using Eocron.Core.FinitieStateAutomaton;

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
        private readonly EocronCompiler<TValue> _compiler = new EocronCompiler<TValue>();
        private readonly CFSA<TValue> _cfsa;

        public ORegex(string pattern, EocronOptions options, params Func<TValue, bool>[] predicates) : this(pattern, options, CreatePredicateTable(predicates))
        {
        }

        public ORegex(string pattern, EocronOptions options, PredicateTable<TValue> table)
        {
            _cfsa = _compiler.Build(pattern, table);
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
            var captureTable = new CaptureTable<TValue>();
            for (int i = startIndex; i < values.Length; i++)
            {
                var capture = _cfsa.Run(values, i, captureTable);
                if (!capture.Equals(Range.Invalid))
                {
                    var match = new OMatch<TValue>(values, captureTable, capture);
                    captureTable = new CaptureTable<TValue>();
                    i += capture.Length - 1;
                    yield return match;
                }
            }
        }

        public OMatch<TValue> Match(TValue[] values, int startIndex = 0)
        {
            return Matches(values, startIndex).FirstOrDefault();
        }

        public bool IsMatch(TValue[] values, int startIndex = 0)
        {
            return !_cfsa.Run(values, startIndex, null).Equals(Range.Invalid);
        }
    }
}
