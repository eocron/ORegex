using ORegex.Core.Parse;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Schema;
using ORegex.Core;
using ORegex.Core.FinitieStateAutomaton;

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
    public sealed class ObjectRegex<TValue>
    {
        private readonly ORegexCompiler<TValue> _compiler = new ORegexCompiler<TValue>();
        private readonly CFSA<TValue> _cfsa;

        public ObjectRegex(string pattern, ORegexOptions options, params Func<TValue, bool>[] predicates) : this(pattern, options, CreatePredicateTable(predicates))
        {
        }

        public ObjectRegex(string pattern, ORegexOptions options, PredicateTable<TValue> table)
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

        public IEnumerable<ObjectMatch<TValue>> Matches(TValue[] values)
        {
            var stream = new ObjectStream<TValue>(values);

            while (!stream.IsEos())
            {
                var range = _cfsa.Run(stream);
                if (range.Length != 0)
                {
                    yield return new ObjectMatch<TValue>(values, range.Index, range.Length);
                }
                else
                {
                    stream.Step();
                }
            }
        }
    }
}
