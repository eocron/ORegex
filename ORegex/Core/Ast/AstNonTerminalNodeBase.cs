using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ORegex.Core.Ast
{
    public abstract class AstNonTerminalNodeBase : AstNodeBase, IEnumerable
    {
        public readonly AstNodeBase[] Children;

        protected AstNonTerminalNodeBase(IEnumerable<AstNodeBase> children)
        {
            Children = children.ToArray();
        }



        public IEnumerator GetEnumerator()
        {
            return Children.GetEnumerator();
        }
    }
}
