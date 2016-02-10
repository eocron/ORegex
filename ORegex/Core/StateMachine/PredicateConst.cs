using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ORegex.Core.StateMachine
{
    public sealed class PredicateConst<TValue>
    {
        public static readonly Func<TValue, bool> Epsilon = x => { throw new NotImplementedException("epsilon condition.");};

        public static readonly Func<TValue, bool> AlwaysTrue = x => true;
    }
}
