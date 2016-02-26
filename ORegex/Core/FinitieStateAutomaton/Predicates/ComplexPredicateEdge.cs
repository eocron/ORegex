using System;
using ORegex.Core.Ast;

namespace ORegex.Core.FinitieStateAutomaton.Predicates
{
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
            IsLazyPredicate = other.IsLazyPredicate;
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

        public override Range Match(TValue[] sequence, int startIndex, out CaptureTable<TValue> table)
        {
            table = new CaptureTable<TValue>();
            var result = _fsa.Run(sequence, startIndex, table);
            if (!IsCapturePredicate)
            {
                table.Remove(_fsa.Name);
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
