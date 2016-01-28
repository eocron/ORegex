using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ORegex.FSM
{
    public class NDFSM<TValue>
    {
        private readonly List<string> Q = new List<string>();
        public readonly List<Func<TValue, bool>> Sigma = new List<Func<TValue, bool>>();
        public readonly List<Transition<TValue>> Delta = new List<Transition<TValue>>();
        public List<string> Q0 = new List<string>();
        public readonly List<string> F = new List<string>();

        public NDFSM(IEnumerable<string> q, IEnumerable<Func<TValue, bool>> sigma,
            IEnumerable<Transition<TValue>> delta, IEnumerable<string> q0, IEnumerable<string> f)
        {
            Q = q.ToList();
            Sigma = sigma.ToList();
            AddTransitions(delta);
            AddInitialStates(q0);
            AddFinalStates(f);
        }

        private void AddTransitions(IEnumerable<Transition<TValue>> transitions)
        {
            foreach (var transition in transitions.Where(ValidTransition))
            {
                Delta.Add(transition);
            }
        }

        private bool ValidTransition(Transition<TValue> transition)
        {
            return Q.Contains(transition.StartState) &&
                   Q.Contains(transition.EndState) &&
                   Sigma.Contains(transition.Symbol);
        }

        private void AddInitialStates(IEnumerable<string> q0)
        {
            foreach (var startingState in q0.Where(q => q != null && Q.Contains(q)))
            {
                Q0.Add(startingState);
            }
        }

        private void AddFinalStates(IEnumerable<string> finalStates)
        {
            foreach (var finalState in finalStates.Where(finalState => Q.Contains(finalState)))
            {
                F.Add(finalState);
            }
        }

        public void Accepts(TValue[] input)
        {
            ConsoleWriter.Success("Trying to accept: " + input);
            if (Q0.Any(q0 => Accepts(q0, input, new StringBuilder())))
            {
                return;
            }
            ConsoleWriter.Failure("Could not accept the input: " + input);
        }

        private bool Accepts(string currentState, TValue[] input, StringBuilder steps)
        {
            if (input.Length > 0)
            {
                var transitions = GetAllTransitions(currentState, input[0]);
                foreach (var transition in transitions)
                {
                    var currentSteps = new StringBuilder(steps.ToString() + transition);
                    if (Accepts(transition.EndState, input.Skip(1).ToArray(), currentSteps))
                    {
                        return true;
                    }
                }
                return false;
            }
            if (F.Contains(currentState))
            {
                ConsoleWriter.Success("Successfully accepted the input " + input + " " +
                                      "in the final state " + currentState +
                                      " with steps:\n" + steps);
                return true;
            }
            return false;
        }

        private IEnumerable<Transition<TValue>> GetAllTransitions(string currentState, TValue symbol)
        {
            return Delta.FindAll(t => t.StartState == currentState &&
                                      t.Symbol(symbol));
        }
    }
}