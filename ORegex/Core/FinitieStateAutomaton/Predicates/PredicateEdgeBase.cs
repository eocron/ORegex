using Eocron.Core.Ast;

namespace Eocron.Core.FinitieStateAutomaton.Predicates
{
    public abstract class PredicateEdgeBase<TValue>
    {
        public int Priority { get; set; }

        public abstract bool IsFuncPredicate { get; }

        public abstract bool IsComparePredicate { get; }

        public abstract bool IsComplexPredicate { get; }

        public abstract Range Match(TValue[] sequence, int startIndex, out OCaptureTable<TValue> table);

        public abstract bool IsMatch(TValue value);

        public override bool Equals(object obj)
        {
            return IsEqual((PredicateEdgeBase<TValue>) obj, this);
        }

        public static bool IsEqual(PredicateEdgeBase<TValue> a, PredicateEdgeBase<TValue> b)
        {
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            if (a.Priority != b.Priority)
            {
                return false;
            }

            if (a.IsComparePredicate && b.IsComparePredicate)
            {
                var aa = (ComparePredicateEdge<TValue>) a;
                var bb = (ComparePredicateEdge<TValue>) b;
                return ReferenceEquals(aa._comparer, bb._comparer) && aa._comparer.Equals(aa._value, bb._value);
            }
            if (a.IsComplexPredicate && b.IsComplexPredicate)
            {
                var aa = (ComplexPredicateEdge<TValue>) a;
                var bb = (ComplexPredicateEdge<TValue>) b;
                return ReferenceEquals(aa._fsa, bb._fsa) || aa._fsa.DeepEquals(bb._fsa);
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
