using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ORegex.Core.Ast.GroupQuantifiers;

namespace ORegex.Core.Ast
{
    [DebuggerDisplay("Group")]
    public sealed class AstGroupNode : AstConcatNode
    {
        public QuantifierBase Quantifier;

        public AstGroupNode(IEnumerable<AstNodeBase> values, QuantifierBase quantifier = null)
            : base(values)
        {
            Quantifier = quantifier;
        }
    }
}
