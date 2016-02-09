using System;
namespace ORegex.Core.StateMachine
{
    public sealed class FATrans<TValue>
    {
        public int StartState;

        public int EndState;

        public Func<TValue, bool> Condition;

        public FATrans(int from, Func<TValue, bool> condition, int to)
        {
            StartState = from;
            Condition = condition;
            EndState = to;
        }

        public override bool Equals(object obj)
        {
            var other = (FATrans<TValue>) obj;
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
