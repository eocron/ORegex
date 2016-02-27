using Eocron.Core.FinitieStateAutomaton.Predicates;

namespace Eocron.Core.FinitieStateAutomaton
{
    public interface IFSATransition<TValue>
    {
        int From { get; }

        PredicateEdgeBase<TValue> Condition { get; }

        int To { get; }
    }
}
