using System;
using System.Collections.Generic;
using System.Linq;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using ORegex.Core.StateMachine;

namespace ORegex.Core.Parse
{
    public sealed class ORegexAstFactoryArgs<TValue>
    {
        private readonly PredicateTable<TValue> _predicateTable;
        private readonly RegexGrammarParser _parser;

        public ORegexAstFactoryArgs(PredicateTable<TValue> predicateTable, RegexGrammarParser parser)
        {
            _predicateTable = new PredicateTable<TValue>(predicateTable.ThrowIfNull());
            _parser = parser.ThrowIfNull();
            _predicateTable.AddPredicate(".", PredicateConst<TValue>.AlwaysTrue);
        }

        public bool IsName(IParseTree node, string name)
        {
            return name == GetName(node);
        }

        public string GetName(IParseTree node)
        {
            var rule = (RuleContext)node;
            var name = _parser.RuleNames[rule.RuleIndex];
            return name;
        }

        public Func<TValue, bool> GetPredicate(string atomName)
        {
            return _predicateTable.GetPredicate(atomName);
        }

        public void GetInvertedPredicate(IEnumerable<string> names, out Func<TValue, bool> predicate, out string invertedName)
        {
            invertedName = string.Format("inverted: {0}",string.Join(",", names));
            if (!_predicateTable.Contains(invertedName))
            {
                var predicates = names.Select(GetPredicate).Distinct().ToArray();

                Func<TValue, bool> invertedPredicate = x =>
                {
                    return !predicates.Any(p => p(x));
                };
                _predicateTable.AddPredicate(invertedName, invertedPredicate);
            }

            predicate = GetPredicate(invertedName);
        }
    }
}
