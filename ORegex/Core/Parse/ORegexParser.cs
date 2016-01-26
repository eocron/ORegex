using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using ORegex.Core.Ast;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ORegex.Core.Parse
{
    public sealed class ORegexParser<TValue>
    {
        public ORegexParser()
        {

        }

        public AstNodeBase Parse(string input, Dictionary<string, Func<TValue, bool>> predicateTable)
        {
            var lexer = new RegexGrammarLexer(new AntlrInputStream(input));
            var tokenStream = new CommonTokenStream(lexer);
            var parser = new RegexGrammarParser(tokenStream);

            var context = parser.expr();

            var result = ORegexAstFactory<TValue>.Create(context, parser);

            var visitior = new AstAtomConditionVisitior<TValue>(predicateTable);
            visitior.Evaluate(result);
#if DEBUG
            AstNodeBase.Print(result);
#endif
            return result;
        }
    }
}
