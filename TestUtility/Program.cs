using System;
using System.Collections.Generic;
using ORegex.Core.Parse;

namespace TestUtility
{
    class Program
    {
        static void Main(string[] args)
        {
            var parser = new ORegexParser<int>();
            string input = @"
^
			{a}(?<group1>{a})
| {a}{a}*?
| ({a}{a}({a})?)

///i write some regex
/*asdfsdf*/

| [{a}{a}]
| [^{a}{a}]
| (?<={a})
| .
| {a}{2,}
| {a}{2,3}?
$";

            var table = new Dictionary<string, Func<int, bool>>();
            table.Add("a", x => x == 2);
            var result = parser.Parse(input, table);
        }
    }
}
