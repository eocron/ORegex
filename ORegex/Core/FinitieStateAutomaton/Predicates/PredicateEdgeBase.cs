using System;

namespace Eocron.Core.FinitieStateAutomaton.Predicates
{
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public abstract class PredicateEdgeBase<TValue>
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        public static readonly FuncPredicateEdge<TValue> Epsilon = new FuncPredicateEdge<TValue>((v,i) => { throw new NotImplementedException("Epsilon condition."); });

        public static readonly FuncPredicateEdge<TValue> AlwaysTrue = new FuncPredicateEdge<TValue>((v,i) => true);

        public abstract bool IsFuncPredicate { get; }

        public abstract bool IsComparePredicate { get; }

        public override bool Equals(object obj)
        {
            return IsEqual((PredicateEdgeBase<TValue>) obj, this);
        }

        public abstract bool IsMatch(TValue[] values, int index);

        public static bool IsEqual(PredicateEdgeBase<TValue> a, PredicateEdgeBase<TValue> b)
        {
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            if (a.IsComparePredicate && b.IsComparePredicate)
            {
                var aa = (ComparePredicateEdge<TValue>) a;
                var bb = (ComparePredicateEdge<TValue>) b;
                return ReferenceEquals(aa._comparer, bb._comparer) && aa._comparer.Equals(aa._value, bb._value);
            }
            if (a.IsFuncPredicate && b.IsFuncPredicate)
            {
                var aa = (FuncPredicateEdge<TValue>) a;
                var bb = (FuncPredicateEdge<TValue>) b;
                return ReferenceEquals(aa._condition, bb._condition);
            }
            return false;
        }

        public static bool IsEpsilon(PredicateEdgeBase<TValue> a)
        {
            return a.IsFuncPredicate && IsEqual(a, FuncPredicateEdge<TValue>.Epsilon);
        }
    }
}
