namespace ORegex.Core.StateMachine
{
    public sealed class FSACaptureEdge<TValue> : FSAEdgeInfoBase<TValue>
    {
        public readonly IFSA<TValue> InnerFsa;

        public FSACaptureEdge(IFSA<TValue> fsaCondition)
        {
            InnerFsa = fsaCondition;
        }
        public override bool MeetCondition(ObjectStream<TValue> input)
        {
            return InnerFsa.Run(input);
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
