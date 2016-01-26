using ORegex.Core.Ast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ORegex.Core.Parse
{
    public abstract class AstVisitorBase
    {
        public void Evaluate(AstNodeBase node)
        {
            var astNonTerminal = node as AstNonTerminalNodeBase;
            if (astNonTerminal != null)
            {
                foreach (var t in astNonTerminal.Cast<AstNodeBase>())
                {
                    Evaluate(t);
                }
            }
            OnVisit(node);
        }

        protected abstract void OnVisit(AstNodeBase node);
    }
}
