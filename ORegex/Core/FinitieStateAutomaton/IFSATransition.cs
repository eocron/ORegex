using ORegex.Core.FinitieStateAutomaton.Predicates;

namespace ORegex.Core.FinitieStateAutomaton
{
    public interface IFSATransition<TValue>
    {
        int From { get; }

        PredicateEdgeBase<TValue> Condition { get; }

        int To { get; }
    }
}
