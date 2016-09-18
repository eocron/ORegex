using System.Collections.Generic;
using System.Linq;

namespace Eocron.Core
{
    /// <summary>
    /// This is hashSet implemantiton with GetHashCode and Equals overriden.
    /// It will remain all functionality but also provide mathematical set equality.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    internal sealed class Set<TValue> : HashSet<TValue>
    {
        public Set()
        {
        }

        public Set(IEnumerable<TValue> t) : base(t)
        {
            
        }
        public override int GetHashCode()
        {
            return this.Aggregate(0, (current, v) => current ^ v.GetHashCode());
        }

        public override bool Equals(object obj)
        {
            var hs = (Set<TValue>) obj;
            return SetEquals(hs);
        }
    }
}
