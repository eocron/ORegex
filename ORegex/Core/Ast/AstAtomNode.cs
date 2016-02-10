using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ORegex.Core.Ast
{
    public sealed class AstAtomNode<TValue> : AstNodeBase
    {
        public readonly string Name;

        public Func<TValue, bool> Condition;

        public AstAtomNode(string name, Func<TValue, bool> condition, Range range)
            : base(range)
        {
            Name = name.ThrowIfEmpty();
            Condition = condition.ThrowIfNull();
        }

        public override IEnumerable<AstNodeBase> GetChildren()
        {
            yield break;
        }

        public override string ToString()
        {
            return string.Format("{{{0}}}",Name);
        }
    }
}
