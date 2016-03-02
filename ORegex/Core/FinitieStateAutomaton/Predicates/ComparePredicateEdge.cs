using System.Collections.Generic;
using System.Diagnostics;
using Eocron.Core.Ast;

namespace Eocron.Core.FinitieStateAutomaton.Predicates
{
    [DebuggerDisplay("(Compare, {Name})")]
    public sealed class ComparePredicateEdge<TValue> : PredicateEdgeBase<TValue>
    {
        internal readonly IEqualityComparer<TValue> _comparer;
        internal readonly TValue _value;
        
        public ComparePredicateEdge(string name, TValue value, IEqualityComparer<TValue> comparer = null) : base(name)
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
        public override bool IsSystemPredicate
        {
            get { return false; }
        }

        public override bool IsMatch(TValue[] values, int index)
        {
            if (index >= values.Length)
            {
                return false;
            }
            return _comparer.Equals(values[index], _value);
        }

        public override int GetHashCode()
        {
            return _comparer.GetHashCode() ^ _value.GetHashCode();
        }
    }
}
