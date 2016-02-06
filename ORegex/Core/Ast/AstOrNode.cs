using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ORegex.Core.Ast
{
    [DebuggerDisplay("Or")]
    public sealed class AstOrNode : AstCollectionNodeBase
    {
        public AstOrNode(IEnumerable<AstNodeBase> arguments) : base(arguments)
        {
        }
    }
}
