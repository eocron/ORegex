using System.Diagnostics;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

namespace Eocron.Core.Ast
{
    [DebuggerDisplay("({Index},{Length})")]
    public struct Range
    {
        public readonly int Index;
        public readonly int Length;

        public int RightIndex
        {
            get { return Index + Length; }
        }

        public Range(IParseTree tree)
        {
            var context = (ParserRuleContext) tree;
            Index = context.start.StartIndex;
            Length = context.stop.StopIndex - Index + 1;
        }

        public Range(int index, int length)
        {
            Index = index;
            Length = length;
        }

        public override bool Equals(object obj)
        {
            var range = (Range) obj;
            return range.Index == Index && range.Length == Length;
        }

        public override int GetHashCode()
        {
            int hash = Index.GetHashCode();
            hash += 131;
            hash ^= Length.GetHashCode();
            return hash;
        }

        public override string ToString()
        {
            return string.Format("(i:{0}, l:{1})", Index, Length);
        }
    }
}
