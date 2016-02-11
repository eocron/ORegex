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
        public string Name { get; private set; }

        private readonly FSATransition<TValue>[][] _transitionMatrix;

        private readonly int _startState;

        private readonly bool[] _finalsLookup;
        public CFSA(FSA<TValue> fsa)
        {
            Name = fsa.Name;
            _transitionMatrix = fsa.Transitions.ToLookup(x => x.StartState, x => x).OrderBy(x => x.Key).Select(x=>x.ToArray()).ToArray();
            _startState = fsa.Q0.First();
            _finalsLookup = new bool[_transitionMatrix.Length];
            foreach (var f in fsa.F)
            {
                _finalsLookup[f] = true;
            }
        }

        public bool Run(ObjectStream<TValue> stream)
        {
            var state = _startState;

            throw new NotImplementedException();
        }

        public bool TryRun(int state, ObjectStream<TValue> stream, out int nextState)
        {
            nextState = -1;
            var predicates = _transitionMatrix[state];
            for (int i = 0; i < predicates.Length; i++)
            {
                if (predicates[i].Info.MeetCondition(stream))
                {
                    nextState = predicates[i].EndState;
                    return true;
                }
            }
            return false;
        }
    }
}
