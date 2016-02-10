using System.Collections.Generic;
using System.Diagnostics;

namespace ORegex.Core.Ast
{
    public sealed class AstRootNode : AstNodeBase
    {
        public bool MatchBegin;

        public bool MatchEnd;

        public AstNodeBase Regex;

        public AstRootNode(AstNodeBase innerExpression, bool matchBegin, bool matchEnd, Range range)
            : base(range)
        {
            MatchBegin = matchBegin;
            MatchEnd = matchEnd;
            Regex = innerExpression;
        }

        public override string ToString()
        {
            return string.Format("REGEX[b:{0}][e:{1}]",MatchBegin,MatchEnd);
        }

        public override IEnumerable<AstNodeBase> GetChildren()
        {
            yield return Regex;
        }
    }
}
