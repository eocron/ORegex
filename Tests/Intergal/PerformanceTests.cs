using System.Diagnostics;
using System.Linq;
using NUnit.Framework;
using Tests.Core;
using Stopwatch = NUnit.Framework.Compatibility.Stopwatch;

namespace Tests.Intergal
{
    [TestFixture]
    public sealed class PerformanceTests
    {
        [Test]
        public void BuildTest()
        {
            const string pattern = "({x}+{x}+){y}+{x}|{x}|{x}|{x}|{y}|{x}[^{x}][^{y}]?|(?<g1>{x}+?|(?<g2>{x}?)?)";
            Trace.WriteLine(string.Format("Input pattern: {0}", pattern));
            const int iterCount = 10;
            const int repeatCount = 10;
            for (int j = 0; j < iterCount; j++)
            {
                var sw = Stopwatch.StartNew();
                for (int i = 0; i < repeatCount; i++)
                {
                    var oregex = new DebugORegex(pattern);
                }
                sw.Stop();
                Trace.WriteLine(string.Format("Done {0} in {1}", repeatCount, sw.Elapsed));
            }
        }

        [Test]
        public void RunTest()
        {
            var str = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";
            var input = str.ToCharArray();
            var oregex = new DebugORegex("({x}+{x}+){y}+");
            Trace.WriteLine(string.Format("Input string: {0}",str));
            const int iterCount = 10;
            const int repeatCount = 10;
            for (int j = 0; j < iterCount; j++)
            {
                var sw = Stopwatch.StartNew();
                for (int i = 0; i < repeatCount; i++)
                {
                    var array = oregex.Matches(input).ToArray();
                }
                sw.Stop();
                Trace.WriteLine(string.Format("Done {0} and string length {1} in {2}",repeatCount,input.Length, sw.Elapsed));
            }
        }
    }
}
