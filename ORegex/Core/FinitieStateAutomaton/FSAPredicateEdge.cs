using System;

namespace ORegex.Core.FinitieStateAutomaton
{
    public sealed class FSAPredicateEdge<TValue>
    {
        public readonly int ClassGUID;

        public readonly Func<TValue, bool> Predicate; 
        public FSAPredicateEdge(Func<TValue, bool> predicate, int classGUID)
        {
            Predicate = predicate.ThrowIfNull();
            ClassGUID = classGUID;
        }

        public override bool Equals(object obj)
        {
            var pred = obj as FSAPredicateEdge<TValue>;
            if (pred != null)
            {
                return pred.Predicate.Equals(Predicate) && pred.ClassGUID == ClassGUID;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Predicate.GetHashCode() ^ ClassGUID.GetHashCode();
        }

        public static bool IsEpsilonPredicate(FSAPredicateEdge<TValue> info)
        {
            return ReferenceEquals(info.Predicate, PredicateConst<TValue>.Epsilon);
        }

        public static bool IsEqualFast(FSAPredicateEdge<TValue> a, FSAPredicateEdge<TValue> b)
        {
            return ReferenceEquals(a.Predicate, b.Predicate) && a.ClassGUID == b.ClassGUID;
        }
    }
}
