using System;
using System.Collections.Generic;
using Eocron.Core;
using Eocron.Core.FinitieStateAutomaton.Predicates;

namespace Eocron
{
    public class PredicateTable<TValue>
    {
        protected readonly Dictionary<string, PredicateEdgeBase<TValue>> _table;

        public PredicateTable()
        {
            _table = new Dictionary<string, PredicateEdgeBase<TValue>>();
            _table.Add(".", FuncPredicateEdge<TValue>.AlwaysTrue);
        }

        public PredicateTable(PredicateTable<TValue> other) : base()
        {
            _table = new Dictionary<string, PredicateEdgeBase<TValue>>(other._table);
        }
        public virtual void AddPredicate(string name, Func<TValue, bool> predicate)
        {
            name.ThrowIfEmpty();
            predicate.ThrowIfNull();

            if (_table.ContainsKey(name))
            {
                throw new ArgumentException("Such name already exist: " + name, "name");
            }

            _table.Add(name, new FuncPredicateEdge<TValue>(predicate));
        }

        public virtual void AddCompare(string name, TValue value, IEqualityComparer<TValue> comparer = null)
        {
            name.ThrowIfEmpty();
            if (_table.ContainsKey(name))
            {
                throw new ArgumentException("Such name already exist: " + name, "name");
            }
            _table.Add(name, new ComparePredicateEdge<TValue>(value, comparer));
        }

        public virtual PredicateEdgeBase<TValue> GetPredicate(string name)
        {
            PredicateEdgeBase<TValue> predicate;
            if (!_table.TryGetValue(name, out  predicate))
            {
                throw new ArgumentException("No predicate with such name: " + name);
            }
            return predicate;
        }

        public bool Contains(string name)
        {
            return _table.ContainsKey(name);
        }
    }
}
