using System;
using System.Collections.Generic;
using System.Linq;

namespace ORegex.Core.StateMachine
{
    public sealed class FASubsetConverter<TValue>
    {
        /// <summary>
        /// Subset machine that employs the powerset construction or subset construction algorithm.
        /// It creates a DFA that recognizes the same language as the given NFA.
        /// </summary>
        public static FA<TValue> NfaToDfa(FA<TValue> nfa)
        {
            FA<TValue> dfa = new FA<TValue>();

            // Sets of NFA states which is represented by some DFA state
            var markedStates = new HashSet<Set<int>>();
            var unmarkedStates = new HashSet<Set<int>>();

            // Gives a number to each state in the DFA
            var dfaStateNum = new Dictionary<Set<int>, int>();

            var nfaInitial = nfa.Q0.ToSet();

            // Initially, EpsilonClosure(nfa.initial) is the only state in the DFAs states
            // and it's unmarked.
            var first = EpsilonClosure(nfa, nfaInitial);
            unmarkedStates.Add(first);

            // The initial dfa state
            int dfaInitial = dfa.NewState();
            dfaStateNum[first] = dfaInitial;
            dfa.AddStart(dfaInitial);

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
                if (nfa.F.Any(x=> aState.Contains(x)))
                    dfa.AddFinal(dfaStateNum[aState]);

                var iE = nfa.Sigma.GetEnumerator();

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
                            dfaStateNum.Add(next, dfa.NewState());
                        }

                        var from = dfaStateNum[aState];
                        var to = dfaStateNum[next];
                        var condition = iE.Current;

                        dfa.AddTransition(from, condition, to);
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
        private static Set<int> EpsilonClosure(FA<TValue> nfa, Set<int> states)
        {
            // Push all states onto a stack
            Stack<int> uncheckedStack = new Stack<int>(states);

            // Initialize EpsilonClosure(states) to states
            Set<int> epsilonClosure = states;

            while (uncheckedStack.Count != 0)
            {
                // Pop state t, the top element, off the stack
                var t = uncheckedStack.Pop();

                // For each state u with an edge from t to u labeled Epsilon
                foreach (var input in nfa.GetTransitionsFrom(t))
                {
                    if (input.Condition == State<TValue>.Epsilon)
                    {
                        int u = input.EndState;

                        // If u is not already in epsilonClosure, add it and push it onto stack
                        if (!epsilonClosure.Contains(u))
                        {
                            epsilonClosure.Add(u);
                            uncheckedStack.Push(u);
                        }
                    }
                }
            }

            return epsilonClosure;
        }
    }
}
