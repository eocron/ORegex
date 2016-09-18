using Eocron;

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
            for (char c = 'A'; c <= 'Z'; c++)
            {
                var tmp = c;
                AddPredicate(c.ToString(), x => x == tmp);
            }
            for (char c = 'а'; c <= 'я'; c++)
            {
                var tmp = c;
                AddPredicate(c.ToString(), x => x == tmp);
            }
            for (char c = 'А'; c <= 'Я'; c++)
            {
                var tmp = c;
                AddPredicate(c.ToString(), x => x == tmp);
            }
            for (char c = '0'; c <= '9'; c++)
            {
                var tmp = c;
                AddPredicate(c.ToString(), x => x == tmp);
            }
            AddPredicate("WS", char.IsWhiteSpace);
            AddPredicate("b1o", x=> x== '<');
            AddPredicate("b1c", x => x == '>');
            AddPredicate("slash", x => x == '/');
        }
    }
}
