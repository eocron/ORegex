using System;
using System.Collections.Generic;
using ORegex.Core;
using ORegex.Core.StateMachine;

namespace ORegex
{
    public class PredicateTable<TValue>
    {
        protected readonly Dictionary<string, Func<TValue, bool>> _table;
        protected readonly Dictionary<Func<TValue, bool>, string> _tableInverted;

        public PredicateTable()
        {
            _table = new Dictionary<string, Func<TValue, bool>>();
            _tableInverted = new Dictionary<Func<TValue, bool>, string>();
            AddPredicate(".", PredicateConst<TValue>.AlwaysTrue);
        }

        public PredicateTable(PredicateTable<TValue> other) : base()
        {
            _table = new Dictionary<string, Func<TValue, bool>>(other._table);
            _tableInverted = new Dictionary<Func<TValue, bool>, string>(other._tableInverted);
        }
        public virtual void AddPredicate(string name, Func<TValue, bool> predicate)
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

        public virtual string GetName(Func<TValue, bool> predicate)
        {
            if (!_tableInverted.ContainsKey(predicate))
            {
                throw new ArgumentException("No such predicate.");
            }
            return _tableInverted[predicate];
        }

        public virtual Func<TValue, bool> GetPredicate(string name)
        {
            if (!_table.ContainsKey(name))
            {
                throw new ArgumentException("No predicate with such name: " + name);
            }
            return _table[name];
        }

        public virtual bool Contains(string name)
        {
            return _table.ContainsKey(name);
        }
    }
}
