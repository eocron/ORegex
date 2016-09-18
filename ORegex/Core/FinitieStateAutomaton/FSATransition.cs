using System.Diagnostics;
using Eocron.Core.FinitieStateAutomaton.Predicates;

namespace Eocron.Core.FinitieStateAutomaton
{
    [DebuggerDisplay("{BeginState}-{Condition}->{EndState}")]
    // ReSharper disable once InconsistentNaming
    public sealed class FSATransition<TValue> : IFSATransition<TValue>
    {
        public int BeginState { get; }

        public int EndState { get; }

        public PredicateEdgeBase<TValue> Condition { get; }

        public FSATransition(int beginState, PredicateEdgeBase<TValue> info, int endState)
        {
            BeginState = beginState;
            Condition = info;
            EndState = endState;
        }

        public override bool Equals(object obj)
        {
            var other = (FSATransition<TValue>) obj;
            return other.Condition == Condition && other.BeginState == BeginState && other.EndState == EndState;
        }

        public override int GetHashCode()
        {
            const int prime = 123;
            int hash = prime;
            hash += BeginState.GetHashCode();
            hash *= prime;
            hash += EndState.GetHashCode();
            hash *= prime;
            hash += Condition.GetHashCode();
            return hash;
        }
    }
}
