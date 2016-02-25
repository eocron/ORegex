using System;
using System.Collections.Generic;
using System.Linq;
using ORegex.Core.Ast;
using ORegex.Core.FinitieStateAutomaton.Predicates;

namespace ORegex.Core.FinitieStateAutomaton
{
    public sealed class FSA<TValue> : IFSA<TValue>
    {
        #region Speedup

        private readonly HashSet<PredicateEdgeBase<TValue>> _sigma = new HashSet<PredicateEdgeBase<TValue>>();

        private readonly Dictionary<int, List<FSATransition<TValue>>> _lookup = new Dictionary<int, List<FSATransition<TValue>>>();

        #endregion

        public string Name { get; private set; }

        private readonly HashSet<FSATransition<TValue>> _transitions;

        public IEnumerable<IFSATransition<TValue>> Transitions
        {
            get { return _transitions; }
        }

        public readonly HashSet<int> Q0;

        public readonly HashSet<int> F;


        public IEnumerable<PredicateEdgeBase<TValue>> Sigma
        {
            get
            {
                return _sigma;
            }
        }

        public IEnumerable<int> Q
        {
            get
            {
                return
                    _transitions.Select(x => x.From)
                        .Concat(_transitions.Select(x => x.To))
                        .Distinct()
                        .OrderBy(x => x);
            }
        }

        public int StateCount { get; private set; }

        public int NewState()
        {
            return StateCount++;
        }

        public FSA(string name, IEnumerable<FSATransition<TValue>> transitions, IEnumerable<int> q0, IEnumerable<int> f)
        {
            Name = name.ThrowIfEmpty();
            _transitions = transitions.ToHashSet();
            Q0 = q0.ToHashSet();
            F = f.ToHashSet();
            StateCount = Q.Count();
            #region Speedup
            foreach(var t in _transitions)
            {
                List<FSATransition<TValue>> predics;
                if (!_lookup.TryGetValue(t.From, out predics))
                {
                    predics = new List<FSATransition<TValue>>();
                    _lookup[t.From] = predics;
                }
                predics.Add(t);

                if (!PredicateEdgeBase<TValue>.IsEpsilon(t.Condition))
                {
                    _sigma.Add(t.Condition);
                }
            }
            

            #endregion
        }

        public FSA(string name)
        {
            Name = name.ThrowIfEmpty();
            _transitions = new HashSet<FSATransition<TValue>>();
            Q0 = new HashSet<int>();
            F = new HashSet<int>();
            StateCount = 0;
        }

        public void AddTransition(int from, PredicateEdgeBase<TValue> condition, int to)
        {
            AddTransition(new FSATransition<TValue>(from, condition, to));
        }

        public void AddEpsilonTransition(int from, int to)
        {
            AddTransition(from, FuncPredicateEdge<TValue>.Epsilon, to);
        }

        public void AddTransition(FSATransition<TValue> trans)
        {
            if (trans == null)
            {
                throw new ArgumentNullException("trans");
            }
            _transitions.Add(trans);
            #region Speedup
            List<FSATransition<TValue>> predics;
            if(!_lookup.TryGetValue(trans.From, out predics))
            {
                predics = new List<FSATransition<TValue>>();
                _lookup[trans.From] = predics;
            }

            predics.Add(trans);

            if (!PredicateEdgeBase<TValue>.IsEpsilon(trans.Condition))
            {
                _sigma.Add(trans.Condition);
            }
            #endregion
        }

        public void AddFinal(int state)
        {
            F.Add(state);
        }

        public void AddStart(int state)
        {
            Q0.Add(state);
        }

        private readonly List<FSATransition<TValue>> EmptyList = new List<FSATransition<TValue>>();
        public IEnumerable<FSATransition<TValue>> GetTransitionsFrom(int state)
        {
            List<FSATransition<TValue>> trans;
            if(_lookup.TryGetValue(state, out trans))
            {
                return trans;
            }
            return EmptyList;
        }

        /// <summary>
        /// Returns a set of NFA states from which there is a transition on input symbol
        /// inp from some state s in states.
        /// </summary>
        /// <param name="states"></param>
        /// <param name="inp"></param>
        /// <returns></returns>
        public Set<int> Move(Set<int> states, PredicateEdgeBase<TValue> inp)
        {
            var result = new Set<int>();

            // For each state in the set of states
            foreach (var state in states)
            {
                foreach (var input in GetTransitionsFrom(state))
                {
                    // If the transition is on input inp, add it to the resulting set
                    if (PredicateEdgeBase<TValue>.IsEqual(input.Condition, inp))
                    {
                        result.Add(input.To);
                    }
                }
            }
            return result;
        }

        public Range Run(TValue[] values, int startIndex, CaptureTable<TValue> table)
        {
            throw new NotImplementedException();
        }


        public bool IsFinal(int state)
        {
            return F.Contains(state);
        }
    }
}
