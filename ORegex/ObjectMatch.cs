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
        public ObjectGroupCollection<TValue> Groups { get; set; }

        internal ObjectMatch(TValue[] collection, int index, int length)
            : base(collection, index, length)
        {
        }
    }
}
