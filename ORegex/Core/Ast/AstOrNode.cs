using System.Collections.Generic;
using System.Linq;

namespace ORegex.Core.Ast
{
    public sealed class AstOrNode : AstNodeBase
    {
        public readonly AstNodeBase[] Arguments;

        public AstOrNode(IEnumerable<AstNodeBase> arguments)
        {
            Arguments = arguments.ToArray();
        }
    }
}
