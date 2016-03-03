using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Eocron.Core.Ast;
using Eocron.Core.FinitieStateAutomaton.Predicates;

namespace Eocron.Core.FinitieStateAutomaton
{
    /// <summary>
    /// Compiled FSA
    /// </summary>
    public sealed class CFSA<TValue> : IFSA<TValue>
    {
        public bool ExactBegin { get; set; }
        public bool ExactEnd { get; set; }

        public string Name { get; private set; }

        private readonly IFSATransition<TValue>[][] _transitionMatrix;

        private readonly int _startState;

        private readonly bool[] _finalsLookup;

        private readonly int _stateCount;
        public IEnumerable<IFSATransition<TValue>> Transitions
        {
            get { return _transitionMatrix.Where(x=> x!= null).SelectMany(x => x); }
        }

        public CFSA(FSA<TValue> fsa)
        {
            ExactBegin = fsa.ExactBegin;
            ExactEnd = fsa.ExactEnd;
            Name = fsa.Name;
            _transitionMatrix = new IFSATransition<TValue>[fsa.StateCount][];
            foreach (var look in fsa.Transitions.ToLookup(x => x.From, x => x))
            {
                _transitionMatrix[look.Key] = look.ToArray();
            }

            _startState = fsa.Q0.First();
            _finalsLookup = new bool[_transitionMatrix.Length];
            foreach (var f in fsa.F)
            {
                _finalsLookup[f] = true;
            }
            _stateCount = _transitionMatrix.Length;
        }

        private sealed class FSMState
        {
            public int CurrentIndex;
            public int CurrentPredicateIndex;
            public IFSATransition<TValue>[] Transitions;
            public bool IsFinal;
        }

        [ThreadStatic]
        private readonly Stack<FSMState> _globalStack = new Stack<FSMState>(); 

        public Range Run(TValue[] values, int startIndex)
        {
            var stack = _globalStack;
            stack.Push(CreateState(_startState, startIndex));
            FSMState state = null;
            while (stack.Count > 0)
            {
                state = stack.Peek();

                FSMState next;
                if (TryGetNextState(state, values, out next))
                {
                    stack.Push(next);
                }
                else if (state.IsFinal)
                {
                    break;
                }
                else
                {
                    state = stack.Pop();
                }
            }

            stack.Clear();

            if (state != null && state.IsFinal)
            {
                var result = new Range(startIndex, state.CurrentIndex - startIndex);
                return result;
            }
            return Range.Invalid;
        }

        public bool IsFinal(int state)
        {
            return _finalsLookup[state];
        }

        private IFSATransition<TValue>[] GetTransitions(int state)
        {
            return _transitionMatrix[state];
        }

        private FSMState CreateState(int state, int index)
        {
            return new FSMState
            {
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
                current.CurrentIndex <= values.Length)
            {
                for (int i = current.CurrentPredicateIndex; i < current.Transitions.Length; i++)
                {
                    var trans = current.Transitions[i];
                    current.CurrentPredicateIndex++;

                    var isMatch = trans.Condition.IsMatch(values, current.CurrentIndex);
                    if (isMatch)
                    {
                        var isEps = PredicateEdgeBase<TValue>.IsEpsilon(trans.Condition);
                        nextState = CreateState(trans.To, current.CurrentIndex + (isEps ? 0 : 1));
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
