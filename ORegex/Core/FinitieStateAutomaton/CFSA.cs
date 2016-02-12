using System;
using System.Collections.Generic;
using System.Linq;

namespace ORegex.Core.FinitieStateAutomaton
{
    /// <summary>
    /// Compiled FSA
    /// </summary>
    public sealed class CFSA<TValue> : IFSA<TValue>
    {
        private struct CFSATransition
        {
            public CFSA<TValue> InnerFSA;

            public Func<TValue, bool> Condition;

            public int EndState;
        }

        public string Name { get; private set; }

        private readonly CFSATransition[][] _transitionMatrix;

        private readonly int _startState;

        private readonly bool[] _finalsLookup;
        public CFSA(FSA<TValue> fsa)
        {
            Name = fsa.Name;
            _transitionMatrix = fsa.Transitions.ToLookup(x => x.StartState, x => x).OrderBy(x => x.Key).Select(x=>CompileTransitions(x).ToArray()).ToArray();
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
                            InnerFSA = new CFSA<TValue>(pInfo.InnerFsa),
                            Condition = null,
                            EndState = t.EndState
                        };
                }
            }
        }

        public bool Run(ObjectStream<TValue> stream)
        {
            var state = _startState;

            int StartIndex = stream.CurrentIndex;
            var stack = new Stack<Queue<int>>();
            stack.Push(new Queue<int>() { StartIndex });
            while(!stream.IsEos())
            {
                var states = stack.Pop();
                
                int current = states.Dequeue();
                
                var transitions = GetTransitions(state, stream);

                //
                stream.Step();
            }
        }

        public bool IsFinal(int state)
        {
            return _finalsLookup[state];
        }

        public Queue<int> GetTransitions(int state, ObjectStream<TValue> stream)
        {
            var result = new Queue<int>();
            var predicates = _transitionMatrix[state];
            for (int i = 0; i < predicates.Length; i++)
            {
                bool predicateSet = 
                bool success = predicates[i].Condition != null && predicates[i].Condition(stream.CurrentElement) ||
                    predicates[i].InnerFSA != null && predicates[i].InnerFSA.Run(stream);

                if(success)
                {
                    result.Enqueue(predicates[i].EndState);
                }
            }
            return result;
        }
    }
}
