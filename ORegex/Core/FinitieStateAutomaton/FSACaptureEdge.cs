namespace ORegex.Core.FinitieStateAutomaton
{
    public sealed class FSACaptureEdge<TValue> : FSAEdgeInfoBase<TValue>
    {
        public override bool IsCaptureEdge { get { return true; } }
        public override bool IsPredicateEdge { get { return false; } }

        public readonly FSA<TValue> InnerFsa;

        public FSACaptureEdge(FSA<TValue> fsaCondition)
        {
            InnerFsa = fsaCondition;
        }

        public override bool Equals(object obj)
        {
            var capt = obj as FSACaptureEdge<TValue>;

            if (capt != null)
            {
                return capt.InnerFsa.Equals(InnerFsa);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return InnerFsa.GetHashCode();
        }
    }
}
