using System;
using System.Diagnostics;

namespace Eocron.Core.FinitieStateAutomaton.Predicates
{
    [DebuggerDisplay("(Predicate, {_condition.GetHashCode()})")]
    public sealed class FuncPredicateEdge<TValue> : PredicateEdgeBase<TValue>
    {
        internal Func<TValue[], int, bool> _condition { get;private set; }

        public FuncPredicateEdge(Func<TValue[], int, bool> condition)
        {
            _condition = condition;
        }

        public override bool IsFuncPredicate
        {
            get { return true; }
        }

        public override bool IsComparePredicate
        {
            get { return false; }
        }

        public override int GetHashCode()
        {
            return _condition.GetHashCode();
        }

        public override bool IsMatch(TValue[] values, int index)
        {
            return _condition(values, index);
        }

        public override string ToString()
        {
            return _condition.Target.ToString();
        }
    }
}
