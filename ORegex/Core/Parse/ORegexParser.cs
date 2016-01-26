using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using ORegex.Core.Ast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ORegex.Core.Parse
{
    public sealed class ORegexParser
    {

        public ORegexParser()
        {

        }

        public AstNodeBase Parse(string input)
        {
            var lexer = new RegexGrammarLexer(new AntlrInputStream(input));
            var tokenStream = new CommonTokenStream(lexer);
            var parser = new RegexGrammarParser(tokenStream);

            var context = parser.expr();

            return Evaluate(context);
        }

        private AstNodeBase Evaluate(IParseTree node)
        {
            throw new NotImplementedException();
        }
    }
}
