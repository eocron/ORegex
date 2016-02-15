using System.Diagnostics;
using System.Text.RegularExpressions;

namespace ORegex
{
    [DebuggerDisplay("index={Index}, length={Length};")]
    public sealed class ObjectMatch<TValue> : ObjectGroup<TValue>
    {
        //public readonly ObjectGroupCollection<TValue> Groups;

        internal ObjectMatch(TValue[] collection, int index, int length)
            : base(collection, index, length)
        {
        }
    }
}
