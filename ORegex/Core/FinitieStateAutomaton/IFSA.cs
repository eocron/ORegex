namespace ORegex.Core.FinitieStateAutomaton
{
    public interface IFSA<TValue>
    {
        string Name { get; }
        bool Run(ObjectStream<TValue> stream);
    }
}
