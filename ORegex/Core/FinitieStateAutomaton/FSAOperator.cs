using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Eocron.Core.FinitieStateAutomaton.Predicates;

namespace Eocron.Core.FinitieStateAutomaton
{
    /// <summary>
    /// Defines operations available on finite state automaton graphs.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public sealed class FSAOperator<TValue>
    {
        /// <summary>
        /// Convert any FSA to minimized DFA.
        /// Warning: elminates any epsilon transition.
        /// </summary>
        /// <param name="fsa"></param>
        /// <returns></returns>
        public FSA<TValue> MinimizeFsa(FSA<TValue> fsa)
        {
            fsa = RotateFsa(fsa);
            fsa = RotateFsa(fsa);
            return fsa;
        }

        /// <summary>
        /// Convert any FSA to reversed DFA.
        /// Warning: elminates any epsilon transition.
        /// </summary>
        /// <param name="fsa"></param>
        /// <returns></returns>
        public FSA<TValue> RotateFsa(FSA<TValue> fsa)
        {
            return ToDfa(ReverseFsa(fsa));
        }

        /// <summary>
        /// Reverse any FSA.
        /// Warning: Type of automaton can change from NFA to DFA or DFA to NFA.
        /// </summary>
        /// <param name="fsa"></param>
        /// <returns></returns>
        public FSA<TValue> ReverseFsa(FSA<TValue> fsa)
        {
            return
                new FSA<TValue>(fsa.Name,
                    fsa.Transitions.Select(x => new FSATransition<TValue>(x.To, x.Condition, x.From)),
                    fsa.F, fsa.Q0)
                {
                    ExactBegin = fsa.ExactBegin,
                    ExactEnd = fsa.ExactEnd,
                    CaptureNames = fsa.CaptureNames
                };
        }

        /// <summary>
        /// Subset machine that employs the powerset construction or subset construction algorithm.
        /// It creates a DFA that recognizes the same language as the given NFA.
        /// </summary>
        private static FSA<TValue> ToDfa(FSA<TValue> fsa)
        {
            FSA<TValue> dfa = new FSA<TValue>(fsa.Name)
            {
                ExactBegin = fsa.ExactBegin, 
                ExactEnd = fsa.ExactEnd,
                CaptureNames = fsa.CaptureNames,
            };

            // Sets of NFA states which is represented by some DFA state
            var markedStates = new HashSet<Set<int>>();
            var unmarkedStates = new HashSet<Set<int>>();

            // Gives a number to each state in the DFA
            var dfaStateNum = new Dictionary<Set<int>, int>();

            var nfaInitial = fsa.Q0.ToSet();

            // Initially, EpsilonClosure(nfa.initial) is the only state in the DFAs states
            // and it's unmarked.
            var first = EpsilonClosure(fsa, nfaInitial);
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
                if (fsa.F.Any(x => aState.Contains(x)))
                    dfa.AddFinal(dfaStateNum[aState]);

                // For each input symbol the NFA knows...
                
                foreach (var current in fsa.Sigma)
                {
                    // Next state
                    var next = EpsilonClosure(fsa, fsa.Move(aState, current));

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
                        var condition = current;

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
        private static Set<int> EpsilonClosure(FSA<TValue> nfa, Set<int> states)
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
                    if (PredicateEdgeBase<TValue>.IsEpsilon(input.Condition))
                    {
                        int u = input.To;

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
