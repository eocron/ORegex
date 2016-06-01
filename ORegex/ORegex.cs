using System;
using System.Collections.Generic;
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
    public class ORegex<TValue>
    {
        /// <summary>
        /// Defines maximum fixed stack size. If match is too large - exception will be thrown.
        /// Default: 1024
        /// </summary>
        public static int MaxMatchSize = 1024;

        private static readonly ORegexCompiler<TValue> Compiler = new ORegexCompiler<TValue>();

        private readonly IFSA<TValue> _fa;

        public readonly ORegexOptions Options;

        public readonly string Pattern;

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

        /// <summary>
        /// Tries to match pattern starting from startIndex position.
        /// </summary>
        /// <param name="values"></param>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        public OMatchCollection<TValue> Matches(TValue[] values, int startIndex = 0)
        {
            CheckInput(values,startIndex);
            var handler = new SequenceHandler<TValue>(values);
            handler.Reverse = Options.HasFlag(ORegexOptions.ReverseSequence);

            OMatchCollection<TValue> result = new OMatchCollection<TValue>();
            var captureTable = new OCaptureTable<TValue>(_fa.CaptureNames);
            for (int i = startIndex; i <= handler.Count; i++)
            {
                Range range;
                if (_fa.TryRun(handler, i, captureTable, out range))
                {
                    bool beginMatched = range.Index == startIndex;
                    bool endMatched = (range.Index + range.Length) == handler.Count;

                    if (!_fa.ExactBegin && !_fa.ExactEnd ||
                        !(beginMatched ^ _fa.ExactBegin) && !(endMatched ^ _fa.ExactEnd))
                    {
                        var match = new OMatch<TValue>(handler, captureTable, range);
                        captureTable.Add(0, match);
                        result.Add(match);
                    }
                    captureTable = new OCaptureTable<TValue>(_fa.CaptureNames);
                    i += range.Length == 0 ? 0 : range.Length - 1;
                }
                if (_fa.ExactBegin)
                {
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Tries to match pattern starting from startIndex position.
        /// </summary>
        /// <param name="values"></param>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        public OMatch<TValue> Match(TValue[] values, int startIndex = 0)
        {
            CheckInput(values, startIndex);
            var handler = new SequenceHandler<TValue>(values);
            handler.Reverse = Options.HasFlag(ORegexOptions.ReverseSequence);

            var captureTable = new OCaptureTable<TValue>(_fa.CaptureNames);
            for (int i = startIndex; i <= handler.Count; i++)
            {
                Range range;
                if (_fa.TryRun(handler, i, captureTable, out range))
                {
                    bool beginMatched = range.Index == startIndex;
                    bool endMatched = (range.Index + range.Length) == handler.Count;

                    if (!_fa.ExactBegin && !_fa.ExactEnd ||
                        !(beginMatched ^ _fa.ExactBegin) && !(endMatched ^ _fa.ExactEnd))
                    {
                        var match = new OMatch<TValue>(handler, captureTable, range);
                        captureTable.Add(0, match);
                        return match;
                    }

                    i += range.Length == 0 ? 0 : range.Length - 1;
                }
                if (_fa.ExactBegin)
                {
                    break;
                }
            }
            return null;
        }

        /// <summary>
        /// Tries to match pattern starting from startIndex position.
        /// </summary>
        /// <param name="values"></param>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        public bool IsMatch(TValue[] values, int startIndex = 0)
        {
            CheckInput(values, startIndex);
            var handler = new SequenceHandler<TValue>(values);
            handler.Reverse = Options.HasFlag(ORegexOptions.ReverseSequence);

            for (int i = startIndex; i <= handler.Count; i++)
            {
                Range range;
                if (_fa.TryRun(handler, i, null, out range))
                {
                    bool beginMatched = range.Index == startIndex;
                    bool endMatched = (range.Index + range.Length) == handler.Count;

                    if (!_fa.ExactBegin && !_fa.ExactEnd ||
                        !(beginMatched ^ _fa.ExactBegin) && !(endMatched ^ _fa.ExactEnd))
                    {
                        return true;
                    }
                }
                if (_fa.ExactBegin)
                {
                    break;
                }
            }
            return false;
        }

        /// <summary>
        /// Replaces mathes in sequence by given replacProvider.
        /// </summary>
        /// <param name="values"></param>
        /// <param name="replaceProvider">Provides subSequence replacement.</param>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        public TValue[] Replace(TValue[] values, Func<OMatch<TValue>, TValue[]> replaceProvider, int startIndex = 0)
        {
            replaceProvider.ThrowIfNull();
            CheckInput(values, startIndex);
            var handler = new SequenceHandler<TValue>(values);
            handler.Reverse = Options.HasFlag(ORegexOptions.ReverseSequence);
            
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
                        result.Add(handler[j]);
                    }
                    result.AddRange(transform);
                    i = m.Index + m.Length;
                }
                for (int j = i; j < handler.Count; j++)
                {
                    result.Add(handler[j]);
                }
                return result.ToArray();
            }
            return null;
        }

        private static void CheckInput(TValue[] values, int startIndex)
        {
            if (startIndex < 0)
            {
                throw new ArgumentOutOfRangeException("startIndex");
            }

            if (values == null)
            {
                throw new ArgumentNullException("values");
            }
        }
        public override string ToString()
        {
            return Pattern;
        }
    }
}
