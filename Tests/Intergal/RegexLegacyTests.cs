
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using NUnit.Framework;
using ORegex;
using Tests.Core;

namespace Tests.Intergal
{
    [TestFixture]
    public sealed class RegexLegacyTests
    {
        private static readonly DebugPredicateTable _table = new DebugPredicateTable();

        private static IEnumerable<SingleFileTest> GetTests()
        {
            return SingleFileTestFactory.GetTests("Legacy");//, "004");
        } 

        static RegexLegacyTests()
        {
        }

        [Test, TestCaseSource(typeof(RegexLegacyTests), "GetTests")]
        public void LegasyTest(SingleFileTest test)
        {
            var regexPattern = test.GetRoot().Element("REGEX").Value;
            var oregexPattern = test.GetRoot().Element("OREGEX").Value;

            var text = test.GetRoot().Element("TEXT").Value;

            var regex = new Regex(regexPattern, RegexOptions.ExplicitCapture | RegexOptions.Singleline);
            var oregex = new ObjectRegex<char>(oregexPattern, ORegexOptions.None, _table);

            var regexMatches = regex.Matches(text).Cast<Match>().ToArray();
            var oregexMatches = oregex.Matches(text.ToCharArray()).ToArray();

            Trace.TraceInformation("Text length: "+ text.Length + "\n");


            Console.WriteLine("##############################################################");
            try
            {
                Assert.AreEqual(regexMatches.Length, oregexMatches.Length);
                for (int i = 0; i < regexMatches.Length; i++)
                {
                    var rm = regexMatches[i];
                    var orm = oregexMatches[i];
                    Compare(rm, orm);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("##############################################################");
                Console.WriteLine("#                         EXPECTED                           #");
                Console.WriteLine("##############################################################");
                foreach (var m in regexMatches)
                {
                    Console.WriteLine(ExpectedString(m));
                }
                throw e;
            }
            finally
            {
                Console.WriteLine("##############################################################");
                Console.WriteLine("#                         ACTUAL                             #");
                Console.WriteLine("##############################################################");
                foreach (var m in oregexMatches)
                {
                    Console.WriteLine(ActualString(m));
                }
                Console.WriteLine("##############################################################");
            }


        }

        private static void Compare(Match expected, ObjectMatch<char> actual)
        {
            Assert.AreEqual(expected.Index, actual.Index);
            Assert.AreEqual(expected.Length, actual.Length);
        }

        private static string ExpectedString(Match expected)
        {
            return string.Format("Value: {0},\tindex: {1}, length: {2}", expected.Value,
                expected.Index, expected.Length);
        }

        private static string ActualString(ObjectMatch<char> actual)
        {
            return string.Format("Value: {0},\tindex: {1}, length: {2}", new string(actual.Values.ToArray()),
                actual.Index, actual.Length);
        }
    }
}
