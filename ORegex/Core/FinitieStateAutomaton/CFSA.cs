using System.Collections.Generic;
using System.Linq;
using Eocron.Core.Ast;
using Eocron.Core.FinitieStateAutomaton.Predicates;

namespace Eocron.Core.FinitieStateAutomaton
{
    /// <summary>
    /// Compiled FSA
    /// </summary>
    public sealed class CFSA<TValue> : IFSA<TValue>
    {
        private readonly FixedSizeStack<CaptureEdge> _captureStack = new FixedSizeStack<CaptureEdge>(ORegex<TValue>.MaxMatchSize);
        private readonly FixedSizeStack<FSAState> _instanceStack = new FixedSizeStack<FSAState>(ORegex<TValue>.MaxMatchSize);
        private readonly FixedSizeStack<int> _piStack = new FixedSizeStack<int>(ORegex<TValue>.MaxMatchSize);

        public bool ExactBegin { get; set; }

        public bool ExactEnd { get; set; }

        public string Name { get; private set; }

        private readonly IFSATransition<TValue>[][] _transitionMatrix;

        private readonly int _startState;

        private readonly bool[] _finalsLookup;

        public IEnumerable<IFSATransition<TValue>> Transitions
        {
            get { return _transitionMatrix.Where(x => x != null).SelectMany(x => x); }
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
        }



        public bool TryRun(TValue[] values, int startIndex, OCaptureTable<TValue> table, out Range range)
        {
            range = default(Range);
            var piStack = _piStack;
            piStack.Clear();
            var stack = _instanceStack;
            stack.Clear();
            bool hasSubCaptures = false;
            FSAState state = CreateState(_startState, startIndex);
            stack.Push(state);
            var pi = 0;
            piStack.Push(pi);
            while (stack.Count > 0)
            {
                state = stack.Peek();
                pi = piStack.Peek();

                bool retrieved = false;
                var transitions = _transitionMatrix[state.StateId];
                if (transitions != null &&
                    transitions.Length > 0)
                {
                    for (int i = pi; i < transitions.Length; i++)
                    {
                        var trans = transitions[i];
                        piStack.Push(piStack.Pop() + 1);

                        var isMatch = trans.Condition.IsMatch(values, state.CurrentIndex);
                        if (isMatch)
                        {
                            var isEps = PredicateEdgeBase<TValue>.IsEpsilon(trans.Condition);
                            hasSubCaptures |= trans.Condition.IsSystemPredicate && ((SystemPredicateEdge<TValue>)trans.Condition).IsCapture;
                            stack.Push(CreateState(trans.To, state.CurrentIndex + (isEps ? 0 : 1)));
                            piStack.Push(0);
                            retrieved = true;
                            break;
                        }
                    }
                }

                if (!retrieved)
                {
                    if (_finalsLookup[state.StateId])
                    {
                        break;
                    }
                    stack.Pop();
                    piStack.Pop();
                }
            }

            if (stack.Count != 0 && _finalsLookup[state.StateId])
            {
                if (hasSubCaptures && table != null)
                {
                    ManageSubCaptures(table, values, stack, piStack);
                }
                range = new Range(startIndex, state.CurrentIndex - startIndex);
                return true;
            }
            return false;
        }

        private struct CaptureEdge
        {
            public string Name;
            public int Index;
        }

        private void ManageSubCaptures(OCaptureTable<TValue> table, TValue[] collection,
            FixedSizeStack<FSAState> states, FixedSizeStack<int> piStack)
        {
            var stack = _captureStack;
            stack.Clear();

            for (int i = 0; i < piStack.Count; i++)
            {
                var s = states[i];
                int id = piStack[i] - 1;
                if (id >= 0)
                {
                    var cond = _transitionMatrix[s.StateId][id].Condition;
                    if (cond.IsSystemPredicate)
                    {
                        var sys = (SystemPredicateEdge<TValue>)cond;
                        if (sys.IsCapture)
                        {
                            var left = new CaptureEdge
                            {
                                Index = s.CurrentIndex,
                                Name = sys.CaptureName,
                            };
                            if (stack.Count > 0 && stack.Peek().Name == left.Name)
                            {
                                var right = stack.Pop();
                                table.Add(left.Name,
                                    new OCapture<TValue>(collection, new Range(left.Index, right.Index - left.Index)));
                            }
                            else
                            {
                                stack.Push(left);
                            }

                        }
                    }
                }
            }
        }

        public bool IsFinal(int state)
        {
            return _finalsLookup[state];
        }

        private FSAState CreateState(int state, int index)
        {
            return new FSAState
            {
                StateId = state,
                CurrentIndex = index,
            };
        }
    }
}
