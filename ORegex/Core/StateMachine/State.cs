using System;
using System.Collections.Generic;
using System.Linq;

namespace ORegex.Core.StateMachine
{
    public sealed class State<TValue>
    {
        public static readonly Func<TValue, bool> Epsilon = x => true;

        public bool IsFinal;

        public readonly List<Tuple<Func<TValue, bool>, State<TValue>>> Transitions = new List<Tuple<Func<TValue, bool>, State<TValue>>>();

        public void AddTransition(Func<TValue, bool> transition, State<TValue> state)
        {
            if (!Transitions.Any(x => x.Item1 == transition && x.Item2 == state))
            {
                Transitions.Add(Tuple.Create(transition, state));
            }
        }

        public void AddEpsilonTransition(State<TValue> state)
        {
            AddTransition(Epsilon, state);
        }

        public bool TryEvaluate(TValue value, out State<TValue> nextState)
        {
            nextState = null;
            var transition = Transitions.FirstOrDefault(x => x.Item1(value));
            if(transition != null)
            {
                nextState = transition.Item2;
                return true;
            }
            return false;
        }
    }
}
