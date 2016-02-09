using System.Collections.Generic;

namespace ORegex.Core.StateMachine
{
    public sealed class StateToDFA<TValue>
    {
        public FA<TValue> Convert(State<TValue> start, State<TValue> end)
        {
            var gen = new IdGenerator();
            var edges = new List<FATrans<TValue>>();

            GetAllEdges(start, gen, edges, new HashSet<State<TValue>>());
            var nfa = new FA<TValue>(edges, new[]{gen.GetId(start)}, new[]{gen.GetId(end)});
            var dfa = FASubsetConverter<TValue>.NfaToDfa(nfa);
            dfa = FAMinimizer<TValue>.Minimize(dfa);
            return dfa;
        }

        private void GetAllEdges(State<TValue> state, IdGenerator gen, List<FATrans<TValue>> edges,
            HashSet<State<TValue>> visited)
        {
            if (visited.Contains(state))
            {
                return;
            }
            visited.Add(state);

            foreach (var t in state.Transitions)
            {
                edges.Add(new FATrans<TValue>(gen.GetId(state), t.Item1, gen.GetId(t.Item2)));
                GetAllEdges(t.Item2, gen, edges, visited);
            }
        }
    }
}
