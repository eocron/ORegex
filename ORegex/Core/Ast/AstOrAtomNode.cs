using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ORegex.Core.Ast
{
    [DebuggerDisplay("VarAtom:{Children.Length} n: {IsNegate}")]
    public sealed class AstOrAtomNode<TValue> : AstCollectionNodeBase
    {
        public readonly bool IsNegate;

        public AstOrAtomNode(IEnumerable<AstAtomNode<TValue>> children, bool isNegate) : base(children)
        {
            IsNegate = isNegate;
        }
    }
}
