using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ORegex.Core.Ast.GroupQuantifiers
{
    public sealed class LookAheadQuantifier : QuantifierBase
    {
        public LookAheadQuantifier(string originalString) : base(originalString)
        {
        }
    }
}
