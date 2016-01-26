using ORegex.Core.Ast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ORegex.Core.Parse
{
    public sealed class AstAtomConditionVisitior<TValue> : AstVisitorBase
    {
        private readonly Dictionary<string, Func<TValue, bool>> _table;
        public AstAtomConditionVisitior(Dictionary<string, Func<TValue, bool>> predicateTable)
        {
            _table = predicateTable;
        }

        protected override void OnVisit(AstNodeBase node)
        {
            var atom = node as AstAtomNode<TValue>;
            if (atom != null && _table != null)
            {
                atom.Condition = _table[atom.Name];
            }
        }
    }
}
