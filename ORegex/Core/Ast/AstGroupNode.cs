using System.Collections.Generic;
using Eocron.Core.Ast.GroupQuantifiers;

namespace Eocron.Core.Ast
{
    public sealed class AstGroupNode : AstConcatNode
    {
        public QuantifierBase Quantifier;

        public AstGroupNode(IEnumerable<AstNodeBase> values, QuantifierBase quantifier, Range range)
            : base(values, range)
        {
            Quantifier = quantifier;
        }

        public override string ToString()
        {
            if (Quantifier == null)
            {
                return string.Format("Group[{0}]", Children.Length);
            }
            return string.Format("Group[{0}][{1}]", Children.Length, Quantifier.GetType().Name);
        }
    }
}
