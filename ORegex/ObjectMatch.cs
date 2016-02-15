using System.Diagnostics;
using System.Text.RegularExpressions;
using ORegex.Core.Ast;
using ORegex.Core.FinitieStateAutomaton;
using ORegex.Core.Parse;

namespace ORegex
{
    [DebuggerDisplay("index={Index}, length={Length};")]
    public sealed class ObjectMatch<TValue> : ObjectGroup<TValue>
    {
        public readonly ObjectGroupCollection<TValue> Groups;

        internal ObjectMatch(TValue[] collection, int index, int length, CFSAContext<TValue> context)
            : base(collection, index, length, context._captures[0])
        {
            Groups = new ObjectGroupCollection<TValue>(collection, context);
        }
    }
}
