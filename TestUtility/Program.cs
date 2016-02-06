using System;
using System.Collections.Generic;
using System.IO;
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
                $";

            var table = new Dictionary<string, Func<int, bool>>();
            table.Add("a", x => x == 2);
            var result = parser.Parse(input, table);
        }
    }
}
