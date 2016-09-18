using System;
using System.Diagnostics;

namespace Eocron.Core.FinitieStateAutomaton.Predicates
{
    [DebuggerDisplay("(Predicate, {Name})")]
    public sealed class FuncPredicateEdge<TValue> : PredicateEdgeBase<TValue>
    {
        internal Func<SequenceHandler<TValue>, int, bool> Condition { get; }

        public FuncPredicateEdge(string name, Func<SequenceHandler<TValue>, int, bool> condition) : base(name)
        {
            Condition = condition;
        }

        public override bool IsFuncPredicate
        {
            get { return true; }
        }

        public override bool IsComparePredicate
        {
            get { return false; }
        }

        public override bool IsSystemPredicate
        {
            get { return false; }
        }
        public override int GetHashCode()
        {
            return Condition.GetHashCode();
        }

        public override bool IsMatch(SequenceHandler<TValue> values, int index)
        {
            if (index >= values.Count)
            {
                return false;
            }
            return Condition(values, index);
        }
    }
}
