using System.Collections.Generic;
using System.Linq;
using state = System.Int32;

namespace ORegex.Core.StateMachine
{
    /// <summary>
    /// Implements a deterministic finite automata
    /// </summary>
    public sealed class DFA<TValue>
    {
        // Start state
        public state start;
        // Set of final states
        public HashSet<state> final;

        public Dictionary<state, List<Edge<TValue>>> transTable;

        public IEnumerable<Edge<TValue>> Edges
        {
            get { return transTable.SelectMany(x => x.Value).Distinct(); }
        }

        public IEnumerable<int> States
        {
            get { return Edges.Select(x => x.StartState).Concat(Edges.Select(x => x.EndState)).Distinct(); }
        }
        public DFA()
        {
            final = new HashSet<state>();

            transTable = new Dictionary<state, List<Edge<TValue>>>();
        }
    }


}