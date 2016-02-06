using System;
using System.Collections.Generic;
using System.Linq;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

namespace ORegex.Core.Parse
{
    public sealed class ORegexAstFactoryArgs<TValue>
    {
        private readonly Dictionary<string, Func<TValue, bool>> _predicateTable;
        private static readonly Func<TValue, bool> AlwaysTruePredicate = x => true; 
        private readonly RegexGrammarParser _parser;

        public ORegexAstFactoryArgs(Dictionary<string, Func<TValue, bool>> predicateTable, RegexGrammarParser parser)
        {
            _predicateTable = predicateTable.ThrowIfNull();
            _parser = parser.ThrowIfNull();
            _predicateTable.Add(".", AlwaysTruePredicate);
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
            if (!_predicateTable.ContainsKey(atomName))
            {
                throw new ArgumentException("No predicate with such name: " + atomName);
            }
            return _predicateTable[atomName];
        }

        public void GetInvertedPredicate(IEnumerable<string> names, out Func<TValue, bool> predicate, out string invertedName)
        {
            invertedName = string.Format("inverted: {0}",string.Join(",", names));
            if (!_predicateTable.ContainsKey(invertedName))
            {
                var predicates = names.Select(GetPredicate).ToArray();

                Func<TValue, bool> invertedPredicate = x =>
                {
                    return !predicates.Any(p => p(x));
                };
                _predicateTable[invertedName] = invertedPredicate;
            }

            predicate = GetPredicate(invertedName);
        }
    }
}
