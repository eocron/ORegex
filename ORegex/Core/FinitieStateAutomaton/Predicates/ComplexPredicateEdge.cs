using System;
using ORegex.Core.Ast;

namespace ORegex.Core.FinitieStateAutomaton.Predicates
{
    public sealed class ComplexPredicateEdge<TValue> : PredicateEdgeBase<TValue>
    {
        internal readonly IFSA<TValue> _fsa;

        public ComplexPredicateEdge(IFSA<TValue> fsa)
        {
            _fsa = fsa;
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
            return _fsa.Run(sequence, startIndex, table);
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
