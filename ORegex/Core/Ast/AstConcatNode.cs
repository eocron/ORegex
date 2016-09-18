using System.Collections.Generic;

namespace Eocron.Core.Ast
{
    public class AstConcatNode : AstCollectionNodeBase
    {
        public AstConcatNode(IEnumerable<AstNodeBase> children, Range range)
            : base(children, range)
        {
        }

        public override string ToString()
        {
            return string.Format("Concat[{0}]", Children.Length);
        }
    }
}
