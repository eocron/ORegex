using System;
using ORegex.Core.Parse;

namespace TestUtility
{
    public sealed class PerformanceTest<TValue>
    {
        private readonly ORegexCompiler<TValue> _compiler = new ORegexCompiler<TValue>();
        private readonly DebugPredicateTable<TValue> _table = new DebugPredicateTable<TValue>();
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
