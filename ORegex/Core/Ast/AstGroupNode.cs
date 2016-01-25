using System.Collections.Generic;
using System.Linq;

namespace ORegex.Core.Ast
{
    public sealed class AstGroupNode : AstNodeBase
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public readonly AstNodeBase[] Children;

        public readonly bool IsCaptured;

        public AstGroupNode(IEnumerable<AstNodeBase> children,bool isCaptured = false)
        {
            IsCaptured = isCaptured;
            Children = children.ToArray();
        }
        public AstGroupNode(string name, IEnumerable<AstNodeBase> children) : this(children, true)
        {
            Name = name;
        }
    }
}
