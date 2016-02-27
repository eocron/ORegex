using ORegex;

namespace Tests.Core
{
    public sealed class DebugPredicateTable : PredicateTable<char>
    {
        public DebugPredicateTable()
        {
            for (char c = 'a'; c <= 'z'; c++)
            {
                var tmp = c;
                AddPredicate(c.ToString(), x => x == tmp);
            }

            for (char c = '0'; c <= '9'; c++)
            {
                var tmp = c;
                AddPredicate(c.ToString(), x => x == tmp);
            }
        }
    }
}
