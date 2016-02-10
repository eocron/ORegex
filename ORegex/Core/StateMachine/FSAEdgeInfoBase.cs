namespace ORegex.Core.StateMachine
{
    public abstract class FSAEdgeInfoBase<TValue>
    {
        public abstract bool MeetCondition(ObjectStream<TValue> input);
    }
}
