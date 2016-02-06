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

        public override string ToString()
        {
            return string.Format("Or[{0}]",Children.Length);
        }
    }
}
