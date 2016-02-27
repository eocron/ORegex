using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Tests.Core;
using NUnit.Framework.Compatibility;

namespace Tests.Intergal
{
    [TestFixture]
    public sealed class PerformanceTests
    {
        private readonly DebugPredicateTable _table = new DebugPredicateTable();
        
        [Test]
        public void PerformanceTest()
        {
            var str = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";
            var input = str.ToCharArray();
            var Eocron = new DebugORegex("({x}+{x}+){y}+");
            Console.WriteLine("Input string: {0}",str);
            const int iterCount = 10;
            const int repeatCount = 10;
            for (int j = 0; j < iterCount; j++)
            {
                var sw = Stopwatch.StartNew();
                for (int i = 0; i < repeatCount; i++)
                {
                    var array = Eocron.Matches(input).ToArray();
                }
                sw.Stop();
                Console.WriteLine("Done {0} and string length {1} in {2}",repeatCount,input.Length, sw.Elapsed);
            }
        }
    }
}
