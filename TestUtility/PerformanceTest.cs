using System;
using System.Linq;
using Eocron.Core.Parse;
using Tests.Core;

namespace TestUtility
{
    public sealed class PerformanceTest
    {
        private readonly ORegexCompiler<char> _compiler = new ORegexCompiler<char>();
        private readonly DebugPredicateTable _table = new DebugPredicateTable();

        public void BuildTest(int iterCount)
        {
            const string input =
                @"  ^
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
            for (int i = 0; i < iterCount; i++)
            {
                _compiler.Build(input, _table);
            }
        }

        public void RunTest(int iterCount)
        {
            var str = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";
            var input = str.ToCharArray();
            var oregex = new DebugORegex("({x}+{x}+){y}+");

            for (int i = 0; i < iterCount; i++)
            {
                var array = oregex.Matches(input).ToArray();
            }
        }
    }
}
