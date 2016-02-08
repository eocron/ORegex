using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace ORegex.Core.StateMachine
{
    internal class MinimizeDFA<TValue>
    {
        private class FSA<TValue>
        {
            public HashSet<Func<TValue, bool>> Sigma;
            public List<Edge<TValue>> Delta;
            public HashSet<int> Q0;
            public HashSet<int> Q;
            public HashSet<int> F;

            public FSA()
            {
            }

            public FSA(DFA<TValue> dfa)
            {
                Sigma = new HashSet<Func<TValue, bool>>(dfa.Edges.Select(x => x.Condition));
                Delta = dfa.Edges.ToList();
                Q0 = new HashSet<int>(new[] {dfa.start});
                Q = new HashSet<int>(dfa.States);
                F = new HashSet<int>(dfa.final);
            }

            public DFA<TValue> ToDFA()
            {
                return new DFA<TValue>()
                {
                    start = Q0.First(),
                    final = new HashSet<int>(F),
                    transTable = Delta.ToLookup(x => x.StartState, x => x).ToDictionary(x => x.Key, x => x.ToList())
                };
            }
        }
        public DFA<TValue> MinimizeDFSM(DFA<TValue> fsm)
        {
            var fsa = new FSA<TValue>(fsm);

            var reversedNDFSM = Reverse(fsa);
            var reversedDFSM = PowersetConstruction(reversedNDFSM);
            var NDFSM = Reverse(reversedDFSM);
            var result = PowersetConstruction(NDFSM);

            return reversedDFSM.ToDFA();
        }

        private static FSA<TValue> Reverse(FSA<TValue> d)
        {
            return new FSA<TValue>()
            {
                Q0 = d.F,
                F = d.Q0,
                Q = d.Q,
                Sigma = d.Sigma,
                Delta = d.Delta.Select(x=> new Edge<TValue>()
                {
                    Condition = x.Condition,
                    EndState = x.StartState,
                    StartState = x.EndState
                }).ToList()
            };
        }

        private static FSA<TValue> PowersetConstruction(FSA<TValue> ndfsm)
        {
            var initialSet = new Set<int>(ndfsm.Q0);
            var gen = new IdGenerator();
            var Q = new HashSet<int>();
            var Sigma = new HashSet<Func<TValue, bool>>(ndfsm.Sigma);
            var Delta = new List<Edge<TValue>>();
            var Q0 = new HashSet<int> { gen.GetId(initialSet) };
            var F = new HashSet<int>();

            var processed = new HashSet<Set<int>>();
            var queue = new Queue<Set<int>>();
            queue.Enqueue(initialSet);

            while (queue.Count > 0)
            {
                var setState = queue.Dequeue();
                processed.Add(setState);
                Q.Add(gen.GetId(setState));

                var statesInCurrentSetState = setState;
                foreach (var state in statesInCurrentSetState)
                {
                    if (ndfsm.F.Contains(state))
                    {
                        F.Add(gen.GetId(setState));
                        break;
                    }
                }
                var symbols = ndfsm.Delta
                   .Where(t => statesInCurrentSetState.Contains(t.StartState))
                   .Select(t => t.Condition)
                   .Distinct();
                foreach (var symbol in symbols)
                {
                    var reachableStates =
                       ndfsm.Delta
                          .Where(t => t.Condition == symbol &&
                                      statesInCurrentSetState.Contains(t.StartState))
                          .OrderBy(t => t.EndState).
                          Select(t => t.EndState);
                    var reachableSetState = new Set<int>(reachableStates);

                    Delta.Add(new Edge<TValue>()
                    {
                        StartState = gen.GetId(setState),
                        Condition = symbol,
                        EndState = gen.GetId(reachableStates)
                    });

                    if (!processed.Contains(reachableSetState))
                    {
                        queue.Enqueue(reachableSetState);
                    }
                }
            }

            return new FSA<TValue>()
            {
                Q0 = Q0,
                F = F,
                Q = Q,
                Sigma = Sigma,
                Delta = Delta
            };
        }

        private static string CleanupState(string state)
        {
            return state.Replace(",", " ");
        }
    }
}
