using Eocron;

namespace Tests.Core
{
    public class DebugORegex : ORegex<char>
    {
        public DebugORegex(string pattern, ORegexOptions options = ORegexOptions.None) : base(pattern,options, new DebugPredicateTable())
        {

        }
    }
}
