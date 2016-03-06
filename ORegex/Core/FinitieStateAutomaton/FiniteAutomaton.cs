using System.Collections.Generic;
using Eocron.Core.Ast;

namespace Eocron.Core.FinitieStateAutomaton
{
    public sealed class FiniteAutomaton<TValue> : IFSA<TValue>
    {
        public readonly IFSA<TValue> FastFsa;
        public readonly IFSA<TValue> CmdFsa;


        public string Name
        {
            get { return CmdFsa.Name; }
        }

        public bool ExactEnd
        {
            get { return CmdFsa.ExactEnd; }
        }

        public bool ExactBegin
        {
            get { return CmdFsa.ExactBegin; }
        }

        public string[] CaptureNames 
        {
            get { return CmdFsa.CaptureNames; }
        }

        public FiniteAutomaton(IFSA<TValue> fastFSA, IFSA<TValue> cmdFSA)
        {
            FastFsa = fastFSA.ThrowIfNull();
            CmdFsa = cmdFSA.ThrowIfNull();
        }

        public bool TryRun(TValue[] values, int startIndex, OCaptureTable<TValue> table, out Range range)
        {
            return FastFsa.TryRun(values, startIndex, null, out range) && CmdFsa.TryRun(values, startIndex, table, out range);
        }

        public IEnumerable<IFSATransition<TValue>> Transitions
        {
            get { return CmdFsa.Transitions; }
        }
        public bool IsFinal(int state)
        {
            return CmdFsa.IsFinal(state);
        }
    }
}
