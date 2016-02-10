using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

namespace ORegex.Core.Ast
{
    public sealed class Range
    {
        public readonly int Index;
        public readonly int Length;

        public Range(IParseTree tree)
        {
            var context = (ParserRuleContext) tree;
            Index = context.start.StartIndex;
            Length = context.stop.StopIndex - Index + 1;
        }
    }
}
