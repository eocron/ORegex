namespace ORegex.Core.StateMachine
{
    public interface IFSA<TValue>
    {
        string Name { get; }
        bool Run(ObjectStream<TValue> stream);
    }
}
