using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eocron.Core.Ast;

namespace Eocron.Core.FinitieStateAutomaton
{
    public sealed class FiniteAutomaton<TValue> : IFSA<TValue>
    {
        private readonly IFSA<TValue> _fastFSA;
        private readonly IFSA<TValue> _cmdFSA;


        public string Name
        {
            get { return _cmdFSA.Name; }
        }

        public bool ExactEnd
        {
            get { return _cmdFSA.ExactEnd; }
        }

        public bool ExactBegin
        {
            get { return _cmdFSA.ExactBegin; }
        }

        public FiniteAutomaton(IFSA<TValue> fastFSA, IFSA<TValue> cmdFSA)
        {
            _fastFSA = fastFSA.ThrowIfNull();
            _cmdFSA = cmdFSA.ThrowIfNull();
        }

        public bool TryRun(TValue[] values, int startIndex, out Range range)
        {
            return _fastFSA.TryRun(values, startIndex, out range) && _cmdFSA.TryRun(values, startIndex, out range);
        }

        public IEnumerable<IFSATransition<TValue>> Transitions
        {
            get { return _cmdFSA.Transitions; }
        }
        public bool IsFinal(int state)
        {
            return _cmdFSA.IsFinal(state);
        }
    }
}
