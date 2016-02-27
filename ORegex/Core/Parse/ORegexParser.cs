using Antlr4.Runtime;
using Eocron.Core.Ast;

namespace Eocron.Core.Parse
{
    public sealed class ORegexParser<TValue>
    {
        public AstRootNode Parse(string input, PredicateTable<TValue> predicateTable)
        {
            var lexer = new RegexGrammarLexer(new AntlrInputStream(input));
            var tokenStream = new CommonTokenStream(lexer);
            var parser = new RegexGrammarParser(tokenStream);
            parser.AddErrorListener(new ORegexErrorListener());

            var context = parser.expr();

            var args = new ORegexAstFactoryArgs<TValue>(predicateTable, parser);
            var result = ORegexAstFactory<TValue>.CreateAstTree(context, args);
#if DEBUG
            AstNodeBase.Print(result);
#endif
            return result;
        }
    }
}
