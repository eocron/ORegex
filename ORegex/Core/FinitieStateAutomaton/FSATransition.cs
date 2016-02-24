using System;

namespace ORegex.Core.FinitieStateAutomaton
{
    public sealed class FSATransition<TValue> : IFSATransition<TValue>
    {
        public int From { get; private set; }

        public int To { get; private set; }

        public PredicateEdgeBase<TValue> Condition { get; private set; }

        public FSATransition(int from, PredicateEdgeBase<TValue> info, int to)
        {
            From = from;
            Condition = info;
            To = to;
        }

        public override bool Equals(object obj)
        {
            var other = (FSATransition<TValue>) obj;
            return other.Condition == Condition && other.From == From && other.To == To;
        }

        public override int GetHashCode()
        {
            const int prime = 123;
            int hash = prime;
            hash += From.GetHashCode();
            hash *= prime;
            hash += To.GetHashCode();
            hash *= prime;
            hash += Condition.GetHashCode();
            return hash;
        }
    }
}
