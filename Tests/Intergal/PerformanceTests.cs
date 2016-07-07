using System;
using System.Data;
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
        }

        [Test]
        public void HtmlTagExtractionTest()
        {
            var str = SingleFileTestFactory.GetTestData("Performance//Page.html");

            PerformanceTest(@"{b1o}{p}{b1c}.*?{b1o}{slash}{p}{b1c}", "<p>.*?</p>", str, 20, true);
        }

        [Test]
        public void RandomSequenceTest()
        {
            var str = SingleFileTestFactory.GetTestData("Performance//random.txt");

            PerformanceTest(@"{a}({b}{a})+", "a(ba)+", str, 20, true);
        }

        private static void PerformanceTest(string oregexPattern, string regexPattern, string inputText, int iterCount,
            bool outputTotal = false)
        {
            var input = inputText.ToCharArray();
            var oregex = new DebugORegex(oregexPattern);
            var regex = new Regex(regexPattern,
                RegexOptions.ExplicitCapture | RegexOptions.Singleline | RegexOptions.Compiled);
            if (input.Length <= 3000)
            {
                Console.WriteLine(string.Format("Input string: {0}", inputText));
            }

            var regexCount = new TimeSpan();
            var oregexCount = new TimeSpan();
            var table = new DataTable();
            table.Columns.Add("№", typeof (int));
            table.Columns.Add("ORegex", typeof (TimeSpan));
            table.Columns.Add("Regex", typeof (TimeSpan));
            table.Columns.Add("Ratio", typeof (double));

            for (int j = 0; j < iterCount; j++)
            {
                var sw1 = Stopwatch.StartNew();
                var array1 = oregex.Matches(input);
                sw1.Stop();
                oregexCount += sw1.Elapsed;

                var sw2 = Stopwatch.StartNew();
                var array2 = regex.Matches(inputText).Cast<Match>().ToArray();
                sw2.Stop();
                regexCount += sw2.Elapsed;

                table.Rows.Add(j + 1, sw1.Elapsed, sw2.Elapsed,
                    Math.Round(sw2.ElapsedTicks/(double) sw1.ElapsedTicks, 2));
            }

            if (outputTotal)
            {
                table.Rows.Add(0, oregexCount, regexCount, Math.Round(oregexCount.Ticks/(double) regexCount.Ticks, 3));
            }

            Console.WriteLine("ORegex pattern: {0}; Regex pattern: {1}.", oregexPattern, regexPattern);
            PrintTable(table);
        }

        private static void PrintTable(DataTable table)
        {
            foreach (DataColumn c in table.Columns)
            {
                Console.Write(c.ColumnName + "\t");
            }
            Console.WriteLine();
            foreach (DataRow row in table.Rows)
            {
                for (int x = 0; x < table.Columns.Count; x++)
                {
                    Console.Write(row[x].ToString() + "\t");
                }
                Console.WriteLine();
            }
        }
    }
}
