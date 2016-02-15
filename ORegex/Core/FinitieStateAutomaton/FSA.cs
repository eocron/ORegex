using System;
using System.Collections.Generic;
using System.Linq;
using ORegex.Core.Ast;

namespace ORegex.Core.FinitieStateAutomaton
{
    public sealed class FSA<TValue>
    {
        #region Speedup

        private readonly HashSet<FSAEdgeInfoBase<TValue>> _sigma = new HashSet<FSAEdgeInfoBase<TValue>>();

        private readonly Dictionary<int, List<FSATransition<TValue>>> _lookup = new Dictionary<int, List<FSATransition<TValue>>>();

        #endregion

        public string Name { get; private set; }

        public readonly HashSet<FSATransition<TValue>> Transitions;

        public readonly HashSet<int> Q0;

        public readonly HashSet<int> F;


        public IEnumerable<FSAEdgeInfoBase<TValue>> Sigma
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
                    Transitions.Select(x => x.StartState)
                        .Concat(Transitions.Select(x => x.EndState))
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
            Transitions = transitions.ToHashSet();
            Q0 = q0.ToHashSet();
            F = f.ToHashSet();
            StateCount = Q.Count();
            #region Speedup
            foreach(var t in Transitions)
            {
                List<FSATransition<TValue>> predics;
                if (!_lookup.TryGetValue(t.StartState, out predics))
                {
                    predics = new List<FSATransition<TValue>>();
                    _lookup[t.StartState] = predics;
                }
                predics.Add(t);

                if (!FSAPredicateEdge<TValue>.IsEpsilonPredicate(t.Info))
                {
                    _sigma.Add(t.Info);
                }
            }
            

            #endregion
        }

        public FSA(string name)
        {
            Name = name.ThrowIfEmpty();
            Transitions = new HashSet<FSATransition<TValue>>();
            Q0 = new HashSet<int>();
            F = new HashSet<int>();
            StateCount = 0;
        }

        public void AddTransition(int from, FSA<TValue> condition, int to)
        {
            AddTransition(new FSATransition<TValue>(from, condition, to));
        }

        public void AddTransition(int from, FSAEdgeInfoBase<TValue> condition, int to)
        {
            AddTransition(new FSATransition<TValue>(from, condition, to));
        }

        public void AddTransition(int from, Func<TValue, bool> condition, int to)
        {
            AddTransition(new FSATransition<TValue>(from, condition, to));
        }

        public void AddEpsilonTransition(int from, int to)
        {
            AddTransition(from, PredicateConst<TValue>.Epsilon, to);
        }

        public void AddTransition(FSATransition<TValue> trans)
        {
            if (trans == null)
            {
                throw new ArgumentNullException("trans");
            }
            Transitions.Add(trans);
            #region Speedup
            List<FSATransition<TValue>> predics;
            if(!_lookup.TryGetValue(trans.StartState, out predics))
            {
                predics = new List<FSATransition<TValue>>();
                _lookup[trans.StartState] = predics;
            }

            predics.Add(trans);

            if (!FSAPredicateEdge<TValue>.IsEpsilonPredicate(trans.Info))
            {
                _sigma.Add(trans.Info);
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
        public Set<int> Move(Set<int> states, FSAEdgeInfoBase<TValue> inp)
        {
            var result = new Set<int>();

            // For each state in the set of states
            foreach (var state in states)
            {
                foreach (var input in GetTransitionsFrom(state))
                {
                    // If the transition is on input inp, add it to the resulting set
                    if (FSAEdgeInfoBase<TValue>.IsEqualFast(input.Info, inp))
                    {
                        result.Add(input.EndState);
                    }
                }
            }
            return result;
        }

        public Range Run(ObjectStream<TValue> stream)
        {
            throw new NotImplementedException();
        }
    }
}
