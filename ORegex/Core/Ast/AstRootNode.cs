using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ORegex.Core.Ast
{
    [DebuggerDisplay("REGEX")]
    public sealed class AstRootNode : AstNonTerminalNodeBase
    {
        public AstRootNode(IEnumerable<AstNodeBase> values) : base(values) { }
    }
}
