using System;
using System.Diagnostics;

namespace ORegex.Core.Ast
{
    [DebuggerDisplay("Atom:{Name}")]
    public sealed class AstAtomNode<TValue> : AstNodeBase
    {
        public readonly string Name;

        public Func<TValue, bool> Condition;

        public AstAtomNode(string name)
        {
            Name = name;
            Condition = null;
        } 
    }
}
