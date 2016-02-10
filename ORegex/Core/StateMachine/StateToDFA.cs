using System.Collections.Generic;

namespace ORegex.Core.StateMachine
{
    public sealed class StateToDFA<TValue>
    {
        public FA<TValue> Convert(State<TValue> start, State<TValue> end)
        {
            var nfa = GetFiniteAutomaton("main", start, end);
            var dfa = FASubsetConverter<TValue>.NfaToDfa(nfa);
            dfa = FAMinimizer<TValue>.Minimize(dfa);
            return dfa;
        }

        private FA<TValue> GetFiniteAutomaton(string name, State<TValue> start, State<TValue> end)
        {
            var gen = new IdGenerator();
            var edges = new List<FATrans<TValue>>();

            var fsa = new FA<TValue>(name);
            GetAllEdges(start, gen, fsa, new HashSet<State<TValue>>());
            fsa.AddStart(gen.GetId(start));
            fsa.AddFinal(gen.GetId(end));
            return fsa;
        }


        private void GetAllEdges(State<TValue> state, IdGenerator gen, FA<TValue> fsa,
            HashSet<State<TValue>> visited)
        {
            if (visited.Contains(state))
            {
                return;
            }
            visited.Add(state);

            foreach (var t in state.Transitions)
            {
                fsa.AddTransition(gen.GetId(state), t.Condition, gen.GetId(t.EndState));
                GetAllEdges(t.EndState, gen, fsa, visited);
            }
        }
    }
}
