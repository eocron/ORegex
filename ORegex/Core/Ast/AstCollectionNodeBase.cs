using System.Collections.Generic;
using System.Linq;

namespace Eocron.Core.Ast
{
    public abstract class AstCollectionNodeBase : AstNodeBase
    {
        public readonly AstNodeBase[] Children;

        protected AstCollectionNodeBase(IEnumerable<AstNodeBase> children, Range range) : base(range)
        {
            Children = children.ToArray();
        }

        public override IEnumerable<AstNodeBase> GetChildren()
        {
            return Children;
        }
    }
}
