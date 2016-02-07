using System;
using System.Collections.Generic;
using ORegex.Core;

namespace ORegex
{
    public sealed class PredicateTable<TValue>
    {
        private readonly Dictionary<string, Func<TValue, bool>> _table;
        private readonly Dictionary<Func<TValue, bool>, string> _tableInverted;

        public PredicateTable()
        {
            _table = new Dictionary<string, Func<TValue, bool>>();
            _tableInverted = new Dictionary<Func<TValue, bool>, string>();
        }

        public PredicateTable(PredicateTable<TValue> other)
        {
            _table = new Dictionary<string, Func<TValue, bool>>(other._table);
            _tableInverted = new Dictionary<Func<TValue, bool>, string>(other._tableInverted);
        }
        public void AddPredicate(string name, Func<TValue, bool> predicate)
        {
            name.ThrowIfEmpty();
            predicate.ThrowIfNull();

            if (_table.ContainsKey(name))
            {
                throw new ArgumentException("Such name already exist: " + name, "name");
            }
            if (_tableInverted.ContainsKey(predicate))
            {
                throw new ArgumentException("Such predicate already exist for name: "+ _tableInverted[predicate], "predicate");
            }

            _table[name] = predicate;
            _tableInverted[predicate] = name;
        }

        public string GetName(Func<TValue, bool> predicate)
        {
            if (!_tableInverted.ContainsKey(predicate))
            {
                return "SYSTEM";
            }
            return _tableInverted[predicate];
        }

        public Func<TValue, bool> GetPredicate(string name)
        {
            if (!_table.ContainsKey(name))
            {
                throw new ArgumentException("No predicate with such name: " + name);
            }
            return _table[name];
        }

        public bool Contains(string name)
        {
            return _table.ContainsKey(name);
        }
    }
}
