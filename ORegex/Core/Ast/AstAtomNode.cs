using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ORegex.Core.Ast
{
    [DebuggerDisplay("Atom:{Name}")]
    public sealed class AstAtomNode<TValue> : AstNodeBase
    {
        public readonly string Name;

        public Func<TValue, bool> Condition;

        public AstAtomNode(string name, Func<TValue, bool> condition)
        {
            Name = name.ThrowIfEmpty();
            Condition = condition.ThrowIfNull();
        }

        public override IEnumerable<AstNodeBase> GetChildren()
        {
            yield break;
        }
    }
}
