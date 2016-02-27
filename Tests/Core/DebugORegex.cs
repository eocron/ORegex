using Eocron;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tests.Core
{
    public class DebugORegex : ORegex<char>
    {
        public DebugORegex(string pattern, ORegexOptions options = ORegexOptions.None) : base(pattern,options, new DebugPredicateTable())
        {

        }
    }
}
