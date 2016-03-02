using System;
using System.Diagnostics;

namespace Eocron.Core.FinitieStateAutomaton.Predicates
{
    [DebuggerDisplay("{Name}")]
    public abstract class PredicateEdgeBase<TValue>
    {
        public static readonly FuncPredicateEdge<TValue> Epsilon = new FuncPredicateEdge<TValue>("#eps",(v,i) => { throw new NotImplementedException("Epsilon condition."); });

        public static readonly FuncPredicateEdge<TValue> AlwaysTrue = new FuncPredicateEdge<TValue>("#any",(v,i) => true);

        public readonly string Name;
        public abstract bool IsFuncPredicate { get; }

        public abstract bool IsComparePredicate { get; }

        public abstract bool IsSystemPredicate { get; }

        protected PredicateEdgeBase(string name)
        {
            Name = name.ThrowIfEmpty();
        }

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

            if (a.IsFuncPredicate && b.IsFuncPredicate)
            {
                var aa = (FuncPredicateEdge<TValue>)a;
                var bb = (FuncPredicateEdge<TValue>)b;
                return ReferenceEquals(aa._condition, bb._condition);
            }

            if (a.IsComparePredicate && b.IsComparePredicate)
            {
                return a.Name == b.Name;
            }

            if (a.IsSystemPredicate && b.IsSystemPredicate)
            {
                var aa = (SystemPredicateEdge<TValue>)a;
                var bb = (SystemPredicateEdge<TValue>)b;
                return !aa.IsUnique && !bb.IsUnique && aa.Name == bb.Name;
            }
            return false;
        }

        public static bool IsEpsilon(PredicateEdgeBase<TValue> a)
        {
            return a.IsFuncPredicate && IsEqual(a, FuncPredicateEdge<TValue>.Epsilon);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
