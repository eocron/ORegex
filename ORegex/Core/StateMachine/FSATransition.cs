using System;
namespace ORegex.Core.StateMachine
{
    public sealed class FSATransition<TValue>
    {
        public readonly int StartState;

        public readonly int EndState;

        public readonly Func<TValue, bool> Condition;

        public FSATransition(int from, Func<TValue, bool> condition, int to)
        {
            StartState = from;
            Condition = condition;
            EndState = to;
        }

        public override bool Equals(object obj)
        {
            var other = (FSATransition<TValue>) obj;
            return other.Condition == Condition && other.StartState == StartState && other.EndState == EndState;
        }

        public override int GetHashCode()
        {
            const int prime = 123;
            int hash = prime;
            hash += StartState.GetHashCode();
            hash *= prime;
            hash += EndState.GetHashCode();
            hash *= prime;
            hash += Condition.GetHashCode();
            return hash;
        }
    }
}
