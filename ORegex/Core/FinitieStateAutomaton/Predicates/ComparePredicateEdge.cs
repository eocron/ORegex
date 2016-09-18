using System.Collections.Generic;
using System.Diagnostics;

namespace Eocron.Core.FinitieStateAutomaton.Predicates
{
    [DebuggerDisplay("(Compare, {Name})")]
    public sealed class ComparePredicateEdge<TValue> : PredicateEdgeBase<TValue>
    {
        internal readonly IEqualityComparer<TValue> Comparer;
        internal readonly TValue Value;
        
        public ComparePredicateEdge(string name, TValue value, IEqualityComparer<TValue> comparer = null) : base(name)
        {
            Comparer = comparer ?? EqualityComparer<TValue>.Default;
            Value = value;
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

        public override bool IsMatch(SequenceHandler<TValue> values, int index)
        {
            if (index >= values.Count)
            {
                return false;
            }
            return Comparer.Equals(values[index], Value);
        }

        public override int GetHashCode()
        {
            return Comparer.GetHashCode() ^ Value.GetHashCode();
        }
    }
}
