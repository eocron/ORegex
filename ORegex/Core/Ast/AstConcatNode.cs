using System.Collections.Generic;
using System.Diagnostics;

namespace ORegex.Core.Ast
{
    [DebuggerDisplay("Concat")]
    public class AstConcatNode : AstCollectionNodeBase
    {
        public AstConcatNode(IEnumerable<AstNodeBase> children) : base(children)
        {
        }
    }
}
