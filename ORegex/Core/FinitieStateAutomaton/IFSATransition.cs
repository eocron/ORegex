using Eocron.Core.FinitieStateAutomaton.Predicates;

namespace Eocron.Core.FinitieStateAutomaton
{
    // ReSharper disable once InconsistentNaming
    public interface IFSATransition<TValue>
    {
        int BeginState { get; }

        PredicateEdgeBase<TValue> Condition { get; }

        int EndState { get; }
    }
}
