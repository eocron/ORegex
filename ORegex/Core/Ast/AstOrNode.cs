using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ORegex.Core.Ast
{
    [DebuggerDisplay("Or:{Children.Length}")]
    public sealed class AstOrNode : AstNonTerminalNodeBase
    {
        public AstOrNode(IEnumerable<AstNodeBase> arguments) : base(arguments)
        {
        }
    }
}
