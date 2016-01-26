using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ORegex.Core.Ast
{
    [DebuggerDisplay("VarAtom:{Children.Length} n: {IsNegate}")]
    public sealed class AstVarAtomNode : AstNonTerminalNodeBase
    {
        public readonly bool IsNegate;
        public AstVarAtomNode(IEnumerable<AstNodeBase> children, bool isNegate) : base(children)
        {
            IsNegate = isNegate;
        }
    }
}
