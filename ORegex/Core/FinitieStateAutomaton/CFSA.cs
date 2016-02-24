using System.Collections.Generic;
using System.Linq;
using ORegex.Core.Ast;

namespace ORegex.Core.FinitieStateAutomaton
{
    /// <summary>
    /// Compiled FSA
    /// </summary>
    public sealed class CFSA<TValue> : IFSA<TValue>
    {
        public string[] CaptureGroupNames;


        public string Name { get; private set; }

        private readonly FSATransition<TValue>[][] _transitionMatrix;

        private readonly int _startState;

        private readonly bool[] _finalsLookup;

        public IEnumerable<IFSATransition<TValue>> Transitions
        {
            get { return _transitionMatrix.Where(x=> x!= null).SelectMany(x => x); }
        }

        public CFSA(FSA<TValue> fsa)
        {
            Name = fsa.Name;
            _transitionMatrix = new FSATransition<TValue>[fsa.StateCount][];
            foreach (var look in fsa.Transitions.ToLookup(x => x.From, x => x))
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

        private static IEnumerable<FSATransition<TValue>> CompileTransitions(IEnumerable<IFSATransition<TValue>> transitions)
        {
            foreach (var t in transitions)
            {
                PredicateEdgeBase<TValue> predicate = null;
                if (t.Condition.IsComplexPredicate)
                {
                    var fsa = new CFSA<TValue>((FSA<TValue>) ((ComplexPredicateEdge<TValue>) t.Condition)._fsa);
                    predicate = new ComplexPredicateEdge<TValue>(fsa);
                }

                yield return new FSATransition<TValue>(t.From, predicate?? t.Condition, t.To);
            }
        }

        private sealed class FSMState
        {
            public int CurrentState;
            public int CurrentIndex;
            public int CurrentPredicateIndex;
            public FSATransition<TValue>[] Transitions;
            public bool IsFinal;
        }

        public Range Run(TValue[] values, int startIndex)
        {
            var stack = new Stack<FSMState>();
            stack.Push(CreateState(_startState, startIndex));
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
                return new Range(startIndex, state.CurrentIndex - startIndex);
            }
            return Range.Invalid;
        }

        public bool IsFinal(int state)
        {
            return _finalsLookup[state];
        }

        private FSATransition<TValue>[] GetTransitions(int state)
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
                for (int i = current.CurrentPredicateIndex; i < current.Transitions.Length; i++)
                {
                    current.CurrentPredicateIndex++;
                    var capture = current.Transitions[i].Condition.Match(values, current.CurrentIndex);
                    if (capture.Index >= 0)
                    {
                        nextState = CreateState(current.Transitions[i].To, current.CurrentIndex + capture.Length);
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
