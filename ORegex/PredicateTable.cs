using System;
using System.Collections.Generic;
using Eocron.Core;
using Eocron.Core.FinitieStateAutomaton.Predicates;

namespace Eocron
{
    /// <summary>
    /// Predicate table is key value storage for your predicates. Name it as you wish.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
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

        /// <summary>
        /// Adds lambda predicate with open context.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="predicate"></param>
        public void AddPredicate(string name, Func<TValue[], int, bool> predicate)
        {
            name.ThrowIfEmpty();
            predicate.ThrowIfNull();

            if (_table.ContainsKey(name))
            {
                throw new ArgumentException("Such name already exist: " + name, "name");
            }

            _table.Add(name, new FuncPredicateEdge<TValue>(name, predicate));
        }

        /// <summary>
        /// Adds lamda predicate by value.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="predicate"></param>
        public void AddPredicate(string name, Func<TValue, bool> predicate)
        {
            name.ThrowIfEmpty();
            predicate.ThrowIfNull();
            AddPredicate(name, (v, i) => predicate(v[i]));
        }

        /// <summary>
        /// Add compare predicate by value.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="comparer"></param>
        public void AddCompare(string name, TValue value, IEqualityComparer<TValue> comparer = null)
        {
            name.ThrowIfEmpty();
            if (_table.ContainsKey(name))
            {
                throw new ArgumentException("Such name already exist: " + name, "name");
            }
            _table.Add(name, new ComparePredicateEdge<TValue>(name, value, comparer));
        }

        public PredicateEdgeBase<TValue> GetPredicate(string name)
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
