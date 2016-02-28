using System;
using System.Diagnostics;
using Eocron.Core.Ast;

namespace Eocron.Core.FinitieStateAutomaton.Predicates
{
    [DebuggerDisplay("(Complex, {_fsa.GetHashCode()})")]
    public sealed class ComplexPredicateEdge<TValue> : PredicateEdgeBase<TValue>
    {
        internal readonly IFSA<TValue> _fsa;

        public bool IsCapturePredicate { get; set; }

        public ComplexPredicateEdge(IFSA<TValue> fsa)
        {
            _fsa = fsa;
        }

        public ComplexPredicateEdge(IFSA<TValue> fsa, ComplexPredicateEdge<TValue> other) : this(fsa)
        {
            IsCapturePredicate = other.IsCapturePredicate;
            Priority = other.Priority;
        }

        public override bool IsFuncPredicate
        {
            get { return false; }
        }

        public override bool IsComparePredicate
        {
            get { return false; }
        }

        public override bool IsComplexPredicate
        {
            get { return true; }
        }

        public override Range Match(TValue[] sequence, int startIndex, out OCaptureTable<TValue> table)
        {
            table = new OCaptureTable<TValue>();
            var result = _fsa.Run(sequence, startIndex, table, IsCapturePredicate);
            if (table.Count == 0)
            {
                table = null;
            }
            return result;
        }

        public override int GetHashCode()
        {
            return _fsa.GetHashCode();
        }

        public override bool IsMatch(TValue value)
        {
            throw new NotImplementedException();
        }
    }
}
