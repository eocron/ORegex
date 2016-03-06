using System;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
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
            Console.WriteLine(string.Format("Input pattern: {0}", pattern));
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
                Console.WriteLine(string.Format("Done {0} in {1}", repeatCount, sw.Elapsed));
            }
        }

        [Test]
        public void ExhaustingRunTest()
        {
            for (int i = 50; i <= 500; i += 50)
            {
                var str = string.Join("",Enumerable.Repeat("x",i));
                PerformanceTest("{x}+{x}+{y}+", "x+x+y+", str, 1, false);
                Console.WriteLine("############################################");
            }
        }

        [Test]
        public void SimpleRunTest()
        {
            var str = "asdfffffffffffffffffweage" +
                      "rgerxxxxxxxxxxffffffffff" +
                      "aasdfgargaergerg" +
                      "fffffweagergerxxxxxxxxxxxxxxxxx" +
                      "xxxxxxxyyyyxxxxxxxxfffffffyffffffffweagerge" +
                      "rxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx" +
                      "ywfffffffffffffffweagergerxxxxxxxxxxxxxxxxxx" +
                      "xxxxxxefwefwefadxxxxxxxxxxxxffffffff" +
                      "fffffffweagerge" +
                      "rxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";

            PerformanceTest("{x}+{x}+{y}+", "x+x+y+", str, 20, true);
            Console.WriteLine("############################################");
        }

        [Test]
        public void HtmlTagExtractionTest()
        {
            var str = SingleFileTestFactory.GetTestData("Performance//Page.html");

            PerformanceTest(@"{b1o}{p}{b1c}.*?{b1o}{slash}{p}{b1c}", "<p>.*?</p>", str, 20, true);
            Console.WriteLine("############################################");
        }

        [Test]
        public void RandomSequenceTest()
        {
            var str = SingleFileTestFactory.GetTestData("Performance//random.txt");

            PerformanceTest(@"{a}({b}{a})+", "a(ba)+", str, 10, true);
            Console.WriteLine("############################################");
        }

        private static void PerformanceTest(string oregexPattern, string regexPattern, string inputText, int iterCount, bool outputTotal = false)
        {
            var input = inputText.ToCharArray();
            var oregex = new DebugORegex(oregexPattern);
            var regex = new Regex(regexPattern, RegexOptions.ExplicitCapture | RegexOptions.Singleline | RegexOptions.Compiled);
            if (input.Length <= 3000)
            {
                Console.WriteLine(string.Format("Input string: {0}", inputText));
            }

            var regexCount = new TimeSpan();
            var oregexCount = new TimeSpan();
            for (int j = 0; j < iterCount; j++)
            {
                var sw = Stopwatch.StartNew();
                var array1 = oregex.Matches(input);
                sw.Stop();
                oregexCount += sw.Elapsed;
                long last = sw.ElapsedTicks;
                Console.WriteLine(string.Format("ORegex done in\t{0}", sw.Elapsed));

                sw = Stopwatch.StartNew();
                var array2 = regex.Matches(inputText).Cast<Match>().ToArray();
                sw.Stop();
                regexCount += sw.Elapsed;
                Console.WriteLine(string.Format("Regex done in\t{0}", sw.Elapsed));
            }

            if (outputTotal)
            {
                Console.WriteLine("############################################");
                Console.WriteLine(string.Format("ORegex total\t{0}", oregexCount));
                Console.WriteLine(string.Format("Regex total\t{0}", regexCount));
            }
        }
    }
}
