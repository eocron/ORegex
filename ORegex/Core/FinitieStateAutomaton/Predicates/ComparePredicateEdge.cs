using System.Collections.Generic;
using System.Diagnostics;
using Eocron.Core.Ast;

namespace Eocron.Core.FinitieStateAutomaton.Predicates
{
    [DebuggerDisplay("(Compare, {_value.ToString()})")]
    public sealed class ComparePredicateEdge<TValue> : PredicateEdgeBase<TValue>
    {
        internal readonly IEqualityComparer<TValue> _comparer;
        internal readonly TValue _value;
        
        public ComparePredicateEdge(TValue value, IEqualityComparer<TValue> comparer = null)
        {
            _comparer = comparer ?? EqualityComparer<TValue>.Default;
            _value = value;
        }

        public override bool IsFuncPredicate
        {
            get { return false; }
        }

        public override bool IsComparePredicate
        {
            get { return true; }
        }

        public override bool IsMatch(TValue[] values, int index)
        {
            return _comparer.Equals(values[index], _value);
        }

        public override int GetHashCode()
        {
            return _comparer.GetHashCode() ^ _value.GetHashCode();
        }

        public override string ToString()
        {
            return _value.ToString();
        }
    }
}
