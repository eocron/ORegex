using System;
using System.Collections.Generic;
using System.Linq;
using Antlr4.Runtime.Atn;

namespace ORegex.Core.StateMachine
{
    public class State<TValue>
    {
        public class Trans
        {
            public State<TValue> StartState;
            public Func<TValue, bool> Condition;
            public State<TValue> EndState;
        }

        public readonly List<Trans> Transitions = new List<Trans>();

        public void AddTransition(Func<TValue, bool> transition, State<TValue> state)
        {
            if (!Transitions.Any(x => x.Condition == transition && x.EndState == state))
            {
                Transitions.Add(new Trans()
                {
                    StartState = this,
                    Condition = transition,
                    EndState = state,
                });
            }
        }

        public void AddEpsilonTransition(State<TValue> state)
        {
            AddTransition(PredicateConst<TValue>.Epsilon, state);
        }

        public bool TryEvaluate(TValue value, out State<TValue> nextState)
        {
            nextState = null;
            var transition = Transitions.FirstOrDefault(x => x.Condition(value));
            if(transition != null)
            {
                nextState = transition.EndState;
                return true;
            }
            return false;
        }
    }
}
