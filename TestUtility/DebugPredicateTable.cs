using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ORegex;
using ORegex.Core.StateMachine;

namespace TestUtility
{
    public sealed class DebugPredicateTable<TValue> : PredicateTable<TValue>
    {
        public readonly HashSet<string> AvailableNames = new HashSet<string>(); 
        public DebugPredicateTable()
        {
            AddPredicate("a", x => x == null);
            AddPredicate("b", x => x == null);
            AddPredicate("c", x => x == null);
            AddPredicate("1", x => x == null);
            AddPredicate("2", x => x == null);
            AddPredicate("3", x => x == null);
            AddPredicate("eps", State<TValue>.Epsilon);
        }

        public override void AddPredicate(string name, Func<TValue, bool> predicate)
        {
            AvailableNames.Add(name);
            base.AddPredicate(name, predicate);
        }

        public override string GetName(Func<TValue, bool> predicate)
        {
            if (!_tableInverted.ContainsKey(predicate))
            {
                return "Generated: "+ Regex.Match(predicate.Method.Name, @"\>(.+)").Groups[1].Value;
            }
            return base.GetName(predicate);
        }
    }
}
