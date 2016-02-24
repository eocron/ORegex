using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ORegex;

namespace TestUtility
{
    public sealed class DebugPredicateTable : PredicateTable<char>
    {
        public DebugPredicateTable() : base()
        {
            AddPredicate("a", x => x == 'a');
            AddPredicate("b", x => x == 'b');
            AddPredicate("c", x => x == 'c');
            AddPredicate("1", x => x == '1');
            AddPredicate("2", x => x == '2');
            AddPredicate("3", x => x == '3');
        }
    }
}
