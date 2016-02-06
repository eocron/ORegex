using System.Collections.Generic;
using System.Diagnostics;

namespace ORegex.Core.Ast
{
    [DebuggerDisplay("REGEX")]
    public sealed class AstRootNode : AstCollectionNodeBase
    {
        public bool MatchBegin;

        public bool MatchEnd;

        public AstRootNode(IEnumerable<AstNodeBase> values, bool matchBegin, bool matchEnd) : base(values)
        {
            MatchBegin = matchBegin;
            MatchEnd = matchEnd;
        }
    }
}
