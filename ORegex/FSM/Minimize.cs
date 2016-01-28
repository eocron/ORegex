using System.Collections.Generic;
using System.Linq;

namespace ORegex.FSM
{
    public class Minimize<TValue>
    {
        public static DFSM<TValue> MinimizeDFSM(DFSM<TValue> fsm)
        {
            var reversedNDFSM = Reverse(fsm);
            var reversedDFSM = PowersetConstruction(reversedNDFSM);
            var NDFSM = Reverse(reversedDFSM);
            return PowersetConstruction(NDFSM);
        }

        private static NDFSM<TValue> Reverse(DFSM<TValue> d)
        {
            var delta = new List<Transition<TValue>>();
            foreach (var transition in d.Delta)
            {
                delta.Add(new Transition<TValue>(transition.EndState, transition.Symbol, transition.StartState));
            }
            return new NDFSM<TValue>(d.Q, d.Sigma, delta, d.F, d.Q0);
        }

        private static DFSM<TValue> PowersetConstruction(NDFSM<TValue> ndfsm)
        {
            var Q = new List<string>();
            var Sigma = ndfsm.Sigma.ToList();
            var Delta = new List<Transition<TValue>>();
            var Q0 = new List<string> {string.Join(" ", ndfsm.Q0)};
            var F = new List<string>();

            var processed = new List<string>();
            var queue = new Queue<string>();
            queue.Enqueue(string.Join(",", ndfsm.Q0));

            while (queue.Count > 0)
            {
                var setState = queue.Dequeue();
                processed.Add(setState);
                Q.Add(CleanupState(setState));

                var statesInCurrentSetState = setState.Split(',').ToList();
                foreach (var state in statesInCurrentSetState)
                {
                    if (ndfsm.F.Contains(state))
                    {
                        F.Add(CleanupState(setState));
                        break;
                    }
                }
                var symbols = ndfsm.Delta
                    .Where(t => statesInCurrentSetState.Contains(t.StartState))
                    .Select(t => t.Symbol)
                    .Distinct();
                foreach (var symbol in symbols)
                {
                    var reachableStates =
                        ndfsm.Delta
                            .Where(t => t.Symbol == symbol &&
                                        statesInCurrentSetState.Contains(t.StartState))
                            .OrderBy(t => t.EndState).
                            Select(t => t.EndState);
                    var reachableSetState = string.Join(",", reachableStates);

                    Delta.Add(new Transition<TValue>(CleanupState(setState), symbol, CleanupState(reachableSetState)));

                    if (!processed.Contains(reachableSetState))
                    {
                        queue.Enqueue(reachableSetState);
                    }
                }
            }
            return new DFSM<TValue>(Q, Sigma, Delta, Q0, F);
        }

        private static string CleanupState(string state)
        {
            return state.Replace(",", " ");
        }
    }
}