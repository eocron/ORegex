using System;
using System.Collections.Generic;
using System.Linq;
using state = System.Int32;

namespace ORegex.Core.StateMachine
{
    public sealed class SubsetMachine<TValue>
    {
        /// <summary>
        /// Subset machine that employs the powerset construction or subset construction algorithm.
        /// It creates a DFA that recognizes the same language as the given NFA.
        /// </summary>
        public static DFA<TValue> SubsetConstruct(NFA<TValue> nfa)
        {
            int num = 0;
            DFA<TValue> dfa = new DFA<TValue>();

            // Sets of NFA states which is represented by some DFA state
            var markedStates = new HashSet<Set<state>>();
            var unmarkedStates = new HashSet<Set<state>>();

            // Gives a number to each state in the DFA
            var dfaStateNum = new Dictionary<HashSet<state>, state>();

            var nfaInitial = new Set<state>();
            nfaInitial.Add(nfa.initial);

            // Initially, EpsilonClosure(nfa.initial) is the only state in the DFAs states
            // and it's unmarked.
            var first = EpsilonClosure(nfa, nfaInitial);
            unmarkedStates.Add(first);

            // The initial dfa state
            state dfaInitial = GenNewState(ref num);
            dfaStateNum[first] = dfaInitial;
            dfa.start = dfaInitial;

            while (unmarkedStates.Count != 0)
            {
                // Takes out one unmarked state and posteriorly mark it.
                var aState = unmarkedStates.First();

                // Removes from the unmarked set.
                unmarkedStates.Remove(aState);

                // Inserts into the marked set.
                markedStates.Add(aState);

                // If this state contains the NFA's final state, add it to the DFA's set of
                // final states.
                if (aState.Contains(nfa.final))
                    dfa.final.Add(dfaStateNum[aState]);

                IEnumerator<Func<TValue, bool>> iE = nfa.inputs.GetEnumerator();

                // For each input symbol the NFA knows...
                while (iE.MoveNext())
                {
                    // Next state
                    var next = EpsilonClosure(nfa, nfa.Move(aState, iE.Current));

                    if (next.Count > 0)
                    {
                        // If we haven't examined this state before, add it to the unmarkedStates,
                        // and make up a new number for it.
                        if (!unmarkedStates.Contains(next) && !markedStates.Contains(next))
                        {
                            unmarkedStates.Add(next);
                            dfaStateNum.Add(next, GenNewState(ref num));
                        }

                        var from = dfaStateNum[aState];
                        var to = dfaStateNum[next];
                        var condition = iE.Current;

                        if (!dfa.transTable.ContainsKey(from))
                        {
                            dfa.transTable[from] = new List<Edge<TValue>>();
                        }
                        dfa.transTable[from].Add(new Edge<TValue>()
                        {
                            StartState = from,
                            EndState = to,
                            Condition = condition
                        });
                    }
                }
            }

            return dfa;
        }

        /// <summary>
        /// Builds the Epsilon closure of states for the given NFA 
        /// </summary>
        /// <param name="nfa"></param>
        /// <param name="states"></param>
        /// <returns></returns>
        private static Set<state> EpsilonClosure(NFA<TValue> nfa, Set<state> states)
        {
            // Push all states onto a stack
            Stack<state> uncheckedStack = new Stack<state>(states);

            // Initialize EpsilonClosure(states) to states
            Set<state> epsilonClosure = states;

            while (uncheckedStack.Count != 0)
            {
                // Pop state t, the top element, off the stack
                state t = uncheckedStack.Pop();

                int i = 0;

                // For each state u with an edge from t to u labeled Epsilon
                foreach (var input in nfa.transTable[t])
                {
                    if (input == State<TValue>.Epsilon)
                    {
                        state u = Array.IndexOf(nfa.transTable[t], input, i);

                        // If u is not already in epsilonClosure, add it and push it onto stack
                        if (!epsilonClosure.Contains(u))
                        {
                            epsilonClosure.Add(u);
                            uncheckedStack.Push(u);
                        }
                    }

                    i = i + 1;
                }
            }

            return epsilonClosure;
        }

        /// <summary>
        /// Creates unique state numbers for DFA states
        /// </summary>
        /// <returns></returns>
        private static state GenNewState(ref state num)
        {
            return num++;
        }

    }
}