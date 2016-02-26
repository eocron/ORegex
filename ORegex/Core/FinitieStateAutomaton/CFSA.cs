using System.Collections.Generic;
using System.Linq;
using ORegex.Core.Ast;
using ORegex.Core.FinitieStateAutomaton.Predicates;

namespace ORegex.Core.FinitieStateAutomaton
{
    /// <summary>
    /// Compiled FSA
    /// </summary>
    public sealed class CFSA<TValue> : IFSA<TValue>
    {
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
                    var other = (ComplexPredicateEdge<TValue>) t.Condition;
                    var fsa = new CFSA<TValue>((FSA<TValue>) other._fsa);
                    predicate = new ComplexPredicateEdge<TValue>(fsa, other);
                }

                yield return new FSATransition<TValue>(t.From, predicate?? t.Condition, t.To);
            }
        }

        private sealed class FSMState
        {
            public CaptureTable<TValue> Captures; 
            public int CurrentIndex;
            public int CurrentPredicateIndex;
            public FSATransition<TValue>[] Transitions;
            public bool IsFinal;
        }

        public Range Run(TValue[] values, int startIndex, CaptureTable<TValue> table)
        {
            var stack = new Stack<FSMState>();

            stack.Push(CreateState(_startState, startIndex, null));
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
                        stack.Push(state);
                        break;
                    }
                }
            }

            if (state != null && state.IsFinal)
            {
                foreach (var s in stack.Select(x=>x.Captures).Where(x=> x!= null))
                {
                    table.Add(s);
                }
                var result = new Range(startIndex, state.CurrentIndex - startIndex);
                table.Add(Name, new ObjectCapture<TValue>(values, result));
                return result;
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

        private FSMState CreateState(int state, int index, CaptureTable<TValue> captures)
        {
            return new FSMState
            {
                Captures = captures,
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
                    CaptureTable<TValue> captureTable;
                    var capture = current.Transitions[i].Condition.Match(values, current.CurrentIndex, out captureTable);
                    if (capture.Index >= 0)
                    {
                        nextState = CreateState(current.Transitions[i].To, current.CurrentIndex + capture.Length, captureTable);
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
