using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ORegex.Core.Ast
{
    [DebuggerDisplay("Group:<{Name}>{Children[0]}")]
    public sealed class AstGroupNode : AstConcatNode
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public readonly bool IsCaptured;

        public AstGroupNode(IEnumerable<AstNodeBase> values, bool isCaptured = false)
            : base(values)
        {
            IsCaptured = isCaptured;
        }
        public AstGroupNode(string name, IEnumerable<AstNodeBase> values) : this(values, true)
        {
            Name = name;
        }
    }
}
