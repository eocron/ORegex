using System;
using System.Collections.Generic;
using System.Linq;
using Eocron.Core;
using Eocron.Core.Ast;
using Eocron.Core.FinitieStateAutomaton;
using Eocron.Core.Parse;

namespace Eocron
{
    /// <summary>
    /// Objected regex pattern instance.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public class ORegex<TValue> : IORegex<TValue>
    {
        /// <summary>
        /// Defines maximum fixed stack size. If match is too large - exception will be thrown.
        /// Default: 1024
        /// </summary>
        // ReSharper disable once StaticMemberInGenericType
        public static int MaxMatchSize = 1024;
        public ORegexOptions Options { get; }
        public string Pattern { get; }

        private static readonly ORegexCompiler<TValue> Compiler = new ORegexCompiler<TValue>();
        private readonly IFSA<TValue> _fa;

        public ORegex(string pattern, params TValue[] values) : this(pattern, ORegexOptions.None, null, values) { }

        public ORegex(string pattern, ORegexOptions options, IEqualityComparer<TValue> comparer, params TValue[] values) : this(pattern, options, CreateValuesPredicateTable(values, comparer)) { }

        public ORegex(string pattern, params Func<TValue,bool>[] predicates) : this(pattern, ORegexOptions.None, predicates){}

        public ORegex(string pattern, ORegexOptions options, params Func<TValue, bool>[] predicates) : this(pattern, options, CreatePredicateTable(predicates)) { }

        public ORegex(string pattern, PredicateTable<TValue> table) : this(pattern, ORegexOptions.None, table) { }

        public ORegex(string pattern, ORegexOptions options, PredicateTable<TValue> table)
        {
            _fa = Compiler.Build(pattern, table, options);
            Options = options;
            Pattern = pattern;
        }

        internal ORegex(IFSA<TValue> finiteAutomaton, ORegexOptions options)
        {
            _fa = finiteAutomaton.ThrowIfNull();
            Options = options;
            Pattern = "#Internal pattern are not available by default.";
        }

        public IOMatchCollection<TValue> Matches(IList<TValue> values, int startIndex = -1)
        {
            Validate(values,startIndex);
            var matches = GetAllMathes(values, startIndex);
            return new OMatchCollection<TValue>(matches);
        }

        public IOMatch<TValue> Match(IList<TValue> values, int startIndex = -1)
        {
            Validate(values, startIndex);
            return GetAllMathes(values, startIndex).FirstOrDefault();
        }


        public bool IsMatch(IList<TValue> values, int startIndex = -1)
        {
            Validate(values, startIndex);
            var handler = new SequenceHandler<TValue>(values)
            {
                Reverse = Options.HasFlag(ORegexOptions.ReverseSequence)
            };
            startIndex = GetStartIndex(handler, startIndex);

            for (int i = startIndex; i <= handler.Count; i++)
            {
                Range range;
                if (_fa.TryRun(handler, i, null, out range))
                {
                    bool beginMatched = range.Index == startIndex;
                    bool endMatched = range.RightIndex == handler.Count;

                    if (_fa.ExactBegin && _fa.ExactEnd)
                    {
                        return beginMatched && endMatched;
                    }
                    if (_fa.ExactBegin)
                    {
                        return beginMatched;
                    }
                    if (_fa.ExactEnd)
                    {
                        return endMatched;
                    }
                    return true;
                }
                if (_fa.ExactBegin)
                {
                    break;
                }
            }
            return false;
        }

        public List<TValue> Replace(IList<TValue> values, Func<IOMatch<TValue>, IEnumerable<TValue>> replaceProvider, int startIndex = -1)
        {
            replaceProvider.ThrowIfNull();
            Validate(values, startIndex);

            var matches = Matches(values, startIndex);

            if (matches.Count > 0)
            {
                List<TValue> result = new List<TValue>();
                int i = 0;
                foreach (var m in matches)
                {
                    var transform = replaceProvider(m);
                    for (int j = i; j < m.Index; j++)
                    {
                        result.Add(values[j]);
                    }
                    result.AddRange(transform);
                    i = m.Index + m.Length;
                }
                for (int j = i; j < values.Count; j++)
                {
                    result.Add(values[j]);
                }
                return result;
            }
            return null;
        }

        private static PredicateTable<TValue> CreateValuesPredicateTable(
            TValue[] values,
            IEqualityComparer<TValue> comparer = null)
        {
            if (values == null || values.Length == 0)
            {
                throw new ArgumentException("No values provided.");
            }
            if (comparer == null)
            {
                comparer = EqualityComparer<TValue>.Default;
            }
            var table = new PredicateTable<TValue>();
            for (int i = 0; i < values.Length; i++)
            {
                table.AddCompare(i.ToString(), values[i], comparer);
            }
            return table;
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

        private IEnumerable<OMatch<TValue>> GetAllMathes(IList<TValue> values, int startIndex = -1)
        {
            var handler = new SequenceHandler<TValue>(values)
            {
                Reverse = Options.HasFlag(ORegexOptions.ReverseSequence)
            };
            startIndex = GetStartIndex(handler, startIndex);

            var captureTable = new OCaptureTable<TValue>(_fa.CaptureNames);
            for (int i = startIndex; i <= handler.Count; i++)
            {
                Range range;
                if (_fa.TryRun(handler, i, captureTable, out range))
                {
                    bool beginMatched = range.Index == startIndex;
                    bool endMatched = range.RightIndex == handler.Count;

                    if (!_fa.ExactBegin && !_fa.ExactEnd ||
                        !(beginMatched ^ _fa.ExactBegin) && !(endMatched ^ _fa.ExactEnd))
                    {
                        var match = new OMatch<TValue>(handler, captureTable, range);
                        captureTable.Add(0, match);
                        yield return match;
                    }
                    captureTable = new OCaptureTable<TValue>(_fa.CaptureNames);
                    i += range.Length == 0 ? 0 : range.Length - 1;
                }
                if (_fa.ExactBegin)
                {
                    break;
                }
            }
        }

        private static void Validate(IList<TValue> values, int startIndex)
        {
            if (startIndex < -1)
            {
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            }

            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }
        }

        private static int GetStartIndex(SequenceHandler<TValue> handler, int startIndex)
        {
            if (startIndex < 0)
            {
                return 0;
            }
            return handler.Invert(startIndex);
        }

        public override string ToString()
        {
            return Pattern;
        }
    }
}
