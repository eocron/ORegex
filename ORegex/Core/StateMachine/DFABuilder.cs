using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ORegex.Core.StateMachine
{
    public sealed class DFABuilder<TValue>
    {
        public DFA<TValue> Create(State<TValue> start, State<TValue> end)
        {
            List<Transition> transitions = new List<Transition>(); 
            GetAllTransitions(start, transitions, new HashSet<State<TValue>>());
            var conditions = transitions.Where(x => x.Condition != null).Select(x => x.Condition).Distinct().ToArray();
            var states = transitions.Select(x => x.From).Union(transitions.Select(x => x.To)).Distinct().ToArray();
            
            foreach (var state in states)
            {
                transitions.Where(x => x.From == state && x.Condition == null).Concat(new [] {state});
            }
            return null;
        }

        public void GetAllEpsilons(State<TValue> start, List<State<TValue>> epsilons, )
        private void GetAllTransitions(State<TValue> start, List<Transition> transitions, HashSet<State<TValue>> visited)
        {
            if (visited.Contains(start))
            {
                return;
            }
            visited.Add(start);
            foreach (var t in start.Transitions)
            {
                transitions.Add(new Transition()
                {
                    From = start,
                    Condition = t.Item1,
                    To = t.Item2
                });
                GetAllTransitions(t.Item2, transitions);
            }
        }

        private class Transition
        {
            public State<TValue> From;
            public Func<TValue, bool> Condition; 
            public State<TValue> To;
        }
    }
}
