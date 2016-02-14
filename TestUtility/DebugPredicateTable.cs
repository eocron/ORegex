using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ORegex;

namespace TestUtility
{
    public sealed class DebugPredicateTable : PredicateTable<char>
    {
        public readonly HashSet<string> AvailableNames = new HashSet<string>(); 
        public DebugPredicateTable() : base()
        {
            AddPredicate("a", x => x == 'a');
            AddPredicate("b", x => x == 'b');
            AddPredicate("c", x => x == 'c');
            AddPredicate("1", x => x == '1');
            AddPredicate("2", x => x == '2');
            AddPredicate("3", x => x == '3');
        }

        public override void AddPredicate(string name, Func<char, bool> predicate)
        {
            AvailableNames.Add(name);
            base.AddPredicate(name, predicate);
        }

        public override string GetName(Func<char, bool> predicate)
        {
            if (!_tableInverted.ContainsKey(predicate))
            {
                return "Gen:"+ Regex.Match(predicate.Method.Name, @"\>(.+)").Groups[1].Value;
            }
            return base.GetName(predicate);
        }
    }
}
