using ORegex.Core.Ast;

namespace ORegex.Core.FinitieStateAutomaton
{
    public interface IFSA<TValue>
    {
        string Name { get; }
        Range Run(ObjectStream<TValue> stream);
    }
}
