using System;
using System.Collections.Generic;
using Antlr4.Runtime;
using ORegex.Core.Ast;

namespace ORegex.Core.Parse
{
    public sealed class ORegexParser<TValue>
    {
        public AstNodeBase Parse(string input, Dictionary<string, Func<TValue, bool>> predicateTable)
        {
            var lexer = new RegexGrammarLexer(new AntlrInputStream(input));
            var tokenStream = new CommonTokenStream(lexer);
            var parser = new RegexGrammarParser(tokenStream);

            var context = parser.expr();

            var args = new ORegexAstFactoryArgs<TValue>(predicateTable, parser);
            var result = ORegexAstFactory<TValue>.Create(context, args);
#if DEBUG
            AstNodeBase.Print(result);
#endif
            return result;
        }
    }
}
