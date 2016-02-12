namespace ORegex.Core.FinitieStateAutomaton
{
    public abstract class FSAEdgeInfoBase<TValue>
    {
        public abstract bool IsCaptureEdge { get; }
        public abstract bool IsPredicateEdge { get; }

        public static bool IsEqualFast(FSAEdgeInfoBase<TValue> a, FSAEdgeInfoBase<TValue> b)
        {
            if(a.IsCaptureEdge && b.IsCaptureEdge)
            {
                var aa = (FSACaptureEdge<TValue>)a;
                var bb = (FSACaptureEdge<TValue>)b;
                return ReferenceEquals(aa.InnerFsa, bb.InnerFsa);
            }

            if (a.IsPredicateEdge && b.IsPredicateEdge)
            {
                var aa = (FSAPredicateEdge<TValue>)a;
                var bb = (FSAPredicateEdge<TValue>)b;
                return ReferenceEquals(aa.Predicate, bb.Predicate);
            }

            return false;
        }
    }
}
