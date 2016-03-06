using System.Collections.Generic;

namespace Eocron.Core
{
    internal sealed class OrderedSet<TValue> : HashSet<TValue>
    {
    }
    //internal sealed class OrderedSet<TValue> : List<TValue>
    //{
    //    public OrderedSet()
    //    {
    //    }

    //    public OrderedSet(IEnumerable<TValue> values)
    //    {
    //        base.AddRange(values.Distinct());
    //    } 
    //    public new void Add(TValue value)
    //    {
    //        if (this.IndexOf(value) >= 0)
    //        {
    //            return;
    //        }
    //        base.Add(value);
    //    }
    //}
}
