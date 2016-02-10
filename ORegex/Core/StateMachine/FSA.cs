﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace ORegex.Core.StateMachine
{
    public sealed class FSA<TValue>
    {
        public readonly string Name;

        public readonly HashSet<FSATransition<TValue>> Transitions;

        public readonly HashSet<int> Q0;

        public readonly HashSet<int> F;

        public IEnumerable<Func<TValue, bool>> Sigma
        {
            get
            {
                return Transitions.Select(x => x.Condition).Where(x=> !ReferenceEquals(x, PredicateConst<TValue>.Epsilon)).Distinct();
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
        }

        public FSA(string name)
        {
            Name = name.ThrowIfEmpty();
            Transitions = new HashSet<FSATransition<TValue>>();
            Q0 = new HashSet<int>();
            F = new HashSet<int>();
            StateCount = 0;
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
        }

        public void AddFinal(int state)
        {
            F.Add(state);
        }

        public void AddStart(int state)
        {
            Q0.Add(state);
        }

        public IEnumerable<FSATransition<TValue>> GetTransitionsFrom(int state)
        {
            return Transitions.Where(x => x.StartState == state);
        }

        /// <summary>
        /// Returns a set of NFA states from which there is a transition on input symbol
        /// inp from some state s in states.
        /// </summary>
        /// <param name="states"></param>
        /// <param name="inp"></param>
        /// <returns></returns>
        public Set<int> Move(Set<int> states, Func<TValue, bool> inp)
        {
            var result = new Set<int>();

            // For each state in the set of states
            foreach (var state in states)
            {
                int i = 0;

                // For each transition from this state
                foreach (var input in GetTransitionsFrom(state))
                {
                    // If the transition is on input inp, add it to the resulting set
                    if (ReferenceEquals(input.Condition, inp))
                    {
                        result.Add(input.EndState);
                    }
                    i = i + 1;
                }
            }
            return result;
        }
    }
}