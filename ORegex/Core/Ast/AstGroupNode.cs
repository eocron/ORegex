using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ORegex.Core.Ast.GroupQuantifiers;

namespace ORegex.Core.Ast
{
    public sealed class AstGroupNode : AstConcatNode
    {
        public QuantifierBase Quantifier;

        public AstGroupNode(IEnumerable<AstNodeBase> values, QuantifierBase quantifier = null)
            : base(values)
        {
            Quantifier = quantifier;
        }

        public override string ToString()
        {
            if (Quantifier == null)
            {
                return string.Format("Group[{0}]", Children.Length);
            }
            else
            {
                return string.Format("Group[{0}][{1}]", Children.Length, Quantifier.GetType().Name);
            }
        }
    }
}
