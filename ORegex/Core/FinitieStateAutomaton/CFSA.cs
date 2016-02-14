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
    public sealed class CFSA<TValue> : IFSA<TValue>
    {
        public struct CFSATransition
        {
            public int StartState;

            public CFSA<TValue> InnerFSA;

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

        public CFSA(FSA<TValue> fsa)
        {
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
                if (t.Info.IsPredicateEdge)
                {
                    var pInfo = (FSAPredicateEdge<TValue>)t.Info;
                    yield return new CFSATransition()
                        {                            
                            StartState = t.StartState,
                            InnerFSA = null,
                            Condition = pInfo.Predicate,
                            EndState = t.EndState
                        };
                }
                else if (t.Info.IsCaptureEdge)
                {
                    var pInfo = (FSACaptureEdge<TValue>)t.Info;
                    yield return new CFSATransition()
                        {
                            StartState = t.StartState,
                            InnerFSA = new CFSA<TValue>(pInfo.InnerFsa),
                            Condition = null,
                            EndState = t.EndState
                        };
                }
            }
        }

        public Range Run(ObjectStream<TValue> stream)
        {
            int startIndex = stream.CurrentIndex;
            if (RecRun(_startState, stream))
            {
                return new Range(startIndex, stream.CurrentIndex - startIndex);
            }
            else
            {
                stream.CurrentIndex = startIndex;
                return default(Range);
            }
        }

        public bool RecRun(int state, ObjectStream<TValue> stream)
        {
            int streamIndex = stream.CurrentIndex;
            var predicates = _transitionMatrix[state];
            if (predicates != null)
            {
                for (int i = 0; i < predicates.Length; i++)
                {
                    var predic = predicates[i];

                    if (predic.Condition != null)
                    {
                        var cond = predic.Condition;
                        if (cond(stream.CurrentElement))
                        {
                            stream.Step();
                            if (stream.IsEos())
                            {
                                break;
                            }
                            if (RecRun(predic.EndState, stream))
                            {
                                return true;
                            }
                        }
                    }
                    else
                    {
                        var fsa = predic.InnerFSA;
                        var range = fsa.Run(stream);
                        if (range.Length != 0)
                        {
                            //capture range.
                            var name = fsa.Name;

                            stream.Step();
                            if (stream.IsEos())
                            {
                                break;
                            }
                            if (RecRun(predic.EndState, stream))
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            if (IsFinal(state))
            {
                return true;
            }
            else
            {
                stream.CurrentIndex = streamIndex;
                return false;
            }
        }

        public bool IsFinal(int state)
        {
            return _finalsLookup[state];
        }
    }
}
