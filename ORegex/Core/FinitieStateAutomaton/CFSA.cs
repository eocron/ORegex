using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ORegex.Core.Ast;

namespace ORegex.Core.FinitieStateAutomaton
{
    /// <summary>
    /// Compiled FSA
    /// </summary>
    public sealed class CFSA<TValue>
    {
        public string[] CaptureGroupNames;
 
        public class CFSATransition
        {
            public int ClassGUID;

            public int StartState;

            public Func<TValue, bool> Condition;

            public int EndState;
        }

        public string Name { get; private set; }

        private readonly CFSATransition[][] _transitionMatrix;

        private readonly int _startState;

        private readonly bool[] _finalsLookup;

        public IEnumerable<CFSATransition> Transitions
        {
            get { return _transitionMatrix.Where(x=> x!= null).SelectMany(x => x); }
        }

        public CFSA(FSA<TValue> fsa, string[] captureGroupNames)
        {
            CaptureGroupNames = captureGroupNames;
            Name = fsa.Name;
            _transitionMatrix = new CFSATransition[fsa.StateCount][];
            foreach (var look in fsa.Transitions.ToLookup(x => x.StartState, x => x))
            {
                _transitionMatrix[look.Key] = CompileTransitions(look).ToArray();
            }

            _startState = fsa.Q0.First();
            _finalsLookup = new bool[_transitionMatrix.Length];
            foreach (var f in fsa.F)
            {
                _finalsLookup[f] = true;
            }
        }

        private static IEnumerable<CFSATransition> CompileTransitions(IEnumerable<FSATransition<TValue>> transitions)
        {
            foreach (var t in transitions)
            {
                yield return new CFSATransition()
                {
                    ClassGUID = t.Info.ClassGUID,
                    StartState = t.StartState,
                    Condition = t.Info.Predicate,
                    EndState = t.EndState
                };
            }
        }

        private sealed class FSMState
        {
            public int CurrentState;
            public int CurrentIndex;
            public int CurrentPredicateIndex;
            public CFSATransition[] Transitions;
            public bool IsFinal;
        }

        public ObjectCapture<TValue> Run(TValue[] values, int index)
        {
            var stack = new Stack<FSMState>();
            stack.Push(CreateState(_startState, index));
            FSMState state = null;
            while (stack.Count > 0)
            {
                state = stack.Pop();

                FSMState next;
                if (TryGetNextState(state, values, out next))
                {
                    stack.Push(state);
                    stack.Push(next);
                }
                else
                {
                    if (state.IsFinal)
                    {
                        break;
                    }
                }
            }

            if (state != null && state.IsFinal)
            {
                return new ObjectCapture<TValue>(values, index, state.CurrentIndex - index);
            }
            return null;
        }

        public bool IsFinal(int state)
        {
            return _finalsLookup[state];
        }

        private CFSATransition[] GetTransitions(int state)
        {
            return _transitionMatrix[state];
        }

        private FSMState CreateState(int state, int index)
        {
            return new FSMState
            {
                CurrentState = state,
                CurrentIndex = index,
                CurrentPredicateIndex = 0,
                Transitions = GetTransitions(state),
                IsFinal = IsFinal(state),
            };
        }

        private bool TryGetNextState(FSMState current, TValue[] values, out FSMState nextState)
        {
            nextState = default(FSMState);
            if (current.Transitions != null && 
                current.Transitions.Length > 0 &&
                current.CurrentPredicateIndex != current.Transitions.Length &&
                current.CurrentIndex != values.Length)
            {
                var nextIndex = current.CurrentIndex + 1;
                for (int i = current.CurrentPredicateIndex; i < current.Transitions.Length; i++)
                {
                    current.CurrentPredicateIndex++;
                    if (current.Transitions[i].Condition(values[current.CurrentIndex]))
                    {
                        nextState = CreateState(current.Transitions[i].EndState, nextIndex);
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
