using System.Collections.Generic;
using System.Diagnostics;

namespace ORegex.Core.Ast
{
    public class AstConcatNode : AstCollectionNodeBase
    {
        public AstConcatNode(IEnumerable<AstNodeBase> children) : base(children)
        {
        }

        public override string ToString()
        {
            return string.Format("Concat[{0}]", Children.Length);
        }
    }
}
