using System;
using System.Collections.Generic;
using System.Linq;
using Eocron.Core.Ast;
using Eocron.Core.FinitieStateAutomaton.Predicates;

namespace Eocron.Core.FinitieStateAutomaton
{
    // ReSharper disable once InconsistentNaming
    public sealed class FSA<TValue> : IFSA<TValue>
    {
        public bool ExactBegin { get; set; }

        public bool ExactEnd { get; set; }

        public string[] CaptureNames { get; set; }

        #region Speedup

        private readonly OrderedSet<PredicateEdgeBase<TValue>> _sigma = new OrderedSet<PredicateEdgeBase<TValue>>();

        private readonly Dictionary<int, OrderedSet<FSATransition<TValue>>> _lookup = new Dictionary<int, OrderedSet<FSATransition<TValue>>>();

        #endregion

        public string Name { get; }

        public IEnumerable<IFSATransition<TValue>> Transitions
        {
            get { return _lookup.Values.SelectMany(x=>x); }
        }

        public readonly HashSet<int> Q0;

        public readonly HashSet<int> F;


        public IEnumerable<PredicateEdgeBase<TValue>> Sigma
        {
            get { return _sigma; }
        }

        public IEnumerable<int> Q
        {
            get
            {
                return
                    Transitions.Select(x => x.BeginState)
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
            Q0 = q0.ToHashSet();
            F = f.ToHashSet();

            #region Speedup
            foreach(var t in transitions)
            {
                OrderedSet<FSATransition<TValue>> predics;
                if (!_lookup.TryGetValue(t.BeginState, out predics))
                {
                    predics = new OrderedSet<FSATransition<TValue>>();
                    _lookup[t.BeginState] = predics;
                }
                predics.Add(t);

                if (!PredicateEdgeBase<TValue>.IsEpsilon(t.Condition))
                {
                    _sigma.Add(t.Condition);
                }
            }
            StateCount = Q.Count();
            #endregion
        }

        public FSA(string name)
        {
            Name = name.ThrowIfEmpty();
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

        private void AddTransition(FSATransition<TValue> trans)
        {
            if (trans == null)
            {
                throw new ArgumentNullException(nameof(trans));
            }
            #region Speedup
            OrderedSet<FSATransition<TValue>> predics;
            if(!_lookup.TryGetValue(trans.BeginState, out predics))
            {
                predics = new OrderedSet<FSATransition<TValue>>();
                _lookup[trans.BeginState] = predics;
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

        public bool TryGetTransitionsFrom(int state, out IEnumerable<FSATransition<TValue>> transitions)
        {
            transitions = null;
            OrderedSet<FSATransition<TValue>> trans;
            if (_lookup.TryGetValue(state, out trans))
            {
                transitions = trans;
            }
            return transitions != null;
        }

        /// <summary>
        /// Returns a set of NFA states from which there is a transition on input symbol
        /// inp from some state s in states.
        /// </summary>
        /// <param name="states"></param>
        /// <param name="inp"></param>
        /// <returns></returns>
        internal Set<int> Move(Set<int> states, PredicateEdgeBase<TValue> inp)
        {
            var result = new Set<int>();

            // For each state in the set of states
            foreach (var state in states)
            {
                IEnumerable<FSATransition<TValue>> transitions;
                if (TryGetTransitionsFrom(state, out transitions))
                {
                    foreach (var input in transitions)
                    {
                        // If the transition is on input inp, add it to the resulting set
                        if (PredicateEdgeBase<TValue>.IsEqual(input.Condition, inp))
                        {
                            result.Add(input.EndState);
                        }
                    }
                }
            }
            return result;
        }

        public bool TryRun(SequenceHandler<TValue> values, int startIndex, OCaptureTable<TValue> table, out Range range)
        {
            throw new NotImplementedException();
        }


        public bool IsFinal(int state)
        {
            return F.Contains(state);
        }
    }
}
