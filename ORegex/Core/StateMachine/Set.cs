using System.Collections.Generic;

namespace ORegex.Core.StateMachine
{
    public sealed class Set<TValue> : HashSet<TValue>
    {
        public Set():base()
        {
        }

        public Set(IEnumerable<TValue> t) : base(t)
        {
            
        }
        public override int GetHashCode()
        {
            int hash = 0;
            foreach (var v in this)
            {
                hash ^= v.GetHashCode();
            }
            return hash;
        }

        public override bool Equals(object obj)
        {
            var hs = (Set<TValue>) obj;
            return this.SetEquals(hs);
        }
    }
}
