using System;
using Eocron.Core.Parse;
using Tests.Core;

namespace TestUtility
{
    public sealed class PerformanceTest
    {
        private readonly EocronCompiler<char> _compiler = new EocronCompiler<char>();
        private readonly DebugPredicateTable _table = new DebugPredicateTable();
        public readonly int IterCount;
        public PerformanceTest(int iterCount = 100)
        {
            IterCount = iterCount;
        } 
        public void Run()
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
            for (int i = 0; i < IterCount; i++)
            {
                _compiler.Build(input, _table);
            }
        }
    }
}
