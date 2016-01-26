using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ORegex.Core.Nsm
{
    public sealed class Edge<T>
    {
        public int From { get; set; }
        public Func<T, bool> Predicate { get; set; }
        public int To { get; set; }
    }
}
