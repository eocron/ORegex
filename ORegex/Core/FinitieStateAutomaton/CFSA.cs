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
 
        public struct CFSATransition
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
                _transitionMatrix[look.Key] = CompileTransitions(look, captureGroupNames).ToArray();
            }

            _startState = fsa.Q0.First();
            _finalsLookup = new bool[_transitionMatrix.Length];
            foreach (var f in fsa.F)
            {
                _finalsLookup[f] = true;
            }
        }

        private static IEnumerable<CFSATransition> CompileTransitions(IEnumerable<FSATransition<TValue>> transitions, string[] captureGroupNames)
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

        public ObjectCapture<TValue> Run(ObjectStream<TValue> stream, CFSAContext<TValue> context)
        {
            int startIndex = stream.CurrentIndex;
            if (!stream.IsEos())
            {
                if (RecRun(_startState, stream, context))
                {
                    return new ObjectCapture<TValue>(stream.Sequence, startIndex, stream.CurrentIndex - startIndex);
                }
            }
            stream.CurrentIndex = startIndex;
            return null;
        }

        public bool RecRun(int state, ObjectStream<TValue> stream, CFSAContext<TValue> context)
        {            
            int streamIndex = stream.CurrentIndex;
            
            var predicates = _transitionMatrix[state];
            if (predicates != null)
            {
                for (int i = 0; i < predicates.Length; i++)
                {
                    var predic = predicates[i];

                    var cond = predic.Condition;
                    if (cond(stream.CurrentElement))
                    {
                        state = predic.EndState;
                        stream.Step();
                        if (stream.IsEos())
                        {
                            break; //no more elements - need to find out if current state is final.
                        }
                        else
                        {
                            if (RecRun(state, stream, context))
                            {
                                return true;
                            }
                        }
                    }
                    stream.CurrentIndex = streamIndex;
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
