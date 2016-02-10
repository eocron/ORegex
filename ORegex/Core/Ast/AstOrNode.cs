using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ORegex.Core.Ast
{
    [DebuggerDisplay("Or")]
    public sealed class AstOrNode : AstCollectionNodeBase
    {
        public AstOrNode(IEnumerable<AstNodeBase> arguments, Range range)
            : base(arguments, range)
        {
        }

        public override string ToString()
        {
            return string.Format("Or[{0}]",Children.Length);
        }
    }
}
