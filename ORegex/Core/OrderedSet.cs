using System.Collections.Generic;
using System.Linq;

namespace ORegex.Core
{
    public sealed class OrderedSet<TValue> : List<TValue>
    {
        public OrderedSet()
        {
        }

        public OrderedSet(IEnumerable<TValue> values)
        {
            base.AddRange(values.Distinct());
        } 
        public new void Add(TValue value)
        {
            if (this.IndexOf(value) >= 0)
            {
                return;
            }
            base.Add(value);
        }
    }
}
