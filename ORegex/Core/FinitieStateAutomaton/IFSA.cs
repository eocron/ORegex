using System.Collections.Generic;
using Eocron.Core.Ast;

namespace Eocron.Core.FinitieStateAutomaton
{
    public interface IFSA<TValue>
    {
        string Name { get; }

        Range Run(TValue[] values, int startIndex, OCaptureTable<TValue> table, bool captureSelf);

        IEnumerable<IFSATransition<TValue>> Transitions { get; }

        bool IsFinal(int state);
    }
}
