using System.Collections.Generic;
using Eocron.Core.Ast;

namespace Eocron.Core.FinitieStateAutomaton
{
    public interface IFSA<TValue>
    {
        string Name { get; }

        bool ExactEnd { get; }

        bool ExactBegin { get; }

        bool TryRun(TValue[] values, int startIndex, out Range range);

        IEnumerable<IFSATransition<TValue>> Transitions { get; }

        bool IsFinal(int state);
    }
}
