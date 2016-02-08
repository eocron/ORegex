using System.Collections.Generic;

namespace ORegex.Core.StateMachine
{
    public sealed class StateToDFA<TValue>
    {
        private readonly MinimizeDFA<TValue> _minimizer = new MinimizeDFA<TValue>(); 
        public DFA<TValue> Convert(State<TValue> start, State<TValue> end)
        {
            var gen = new IdGenerator();
            var edges = new List<Edge<TValue>>();

            GetAllEdges(start, gen, edges, new HashSet<State<TValue>>());
            var nfa = new NFA<TValue>(gen.Count, gen.GetId(start), gen.GetId(end));
            foreach (var e in edges)
            {
                nfa.AddTrans(e);
            }
            var dfa = SubsetMachine<TValue>.SubsetConstruct(nfa);
           // dfa = _minimizer.MinimizeDFSM(dfa);
            return dfa;
        }

        private void GetAllEdges(State<TValue> state, IdGenerator gen, List<Edge<TValue>> edges,
            HashSet<State<TValue>> visited)
        {
            if (visited.Contains(state))
            {
                return;
            }
            visited.Add(state);

            foreach (var t in state.Transitions)
            {
                edges.Add(new Edge<TValue>()
                {
                    Condition = t.Item1,
                    StartState = gen.GetId(state),
                    EndState = gen.GetId(t.Item2)
                });
                GetAllEdges(t.Item2, gen, edges, visited);
            }
        }
    }
}
