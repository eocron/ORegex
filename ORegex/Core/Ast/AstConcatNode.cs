using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ORegex.Core.Ast
{
    [DebuggerDisplay("Concat:{Children.Length}")]
    public class AstConcatNode : AstNonTerminalNodeBase
    {
        public AstConcatNode(IEnumerable<AstNodeBase> children) : base(children)
        {
        }
    }
}
