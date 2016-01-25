using System;

namespace ORegex.Core.Ast
{
    public sealed class AstAtomNode<TValue> : AstNodeBase
    {
        public readonly Func<TValue, bool> Condition;

        public AstAtomNode(Func<TValue, bool> condition)
        {
            Condition = condition;
        } 
    }
}
