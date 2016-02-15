
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
        private static readonly PredicateTable<char> _table = new PredicateTable<char>();

        private static IEnumerable<SingleFileTest> GetTests()
        {
            return SingleFileTestFactory.GetTests("Legacy", "004");
        } 

        static RegexLegacyTests()
        {
            _table.AddPredicate("a", x => x == 'a');
            _table.AddPredicate("b", x => x == 'b');
            _table.AddPredicate("c", x => x == 'c');
            _table.AddPredicate("d", x => x == 'd');
            _table.AddPredicate("e", x => x == 'e');
            _table.AddPredicate("f", x => x == 'f');

            _table.AddPredicate("0", x => x == '0');
            _table.AddPredicate("1", x => x == '1');
            _table.AddPredicate("2", x => x == '2');
            _table.AddPredicate("3", x => x == '3');
        }

        [Test, TestCaseSource(typeof(RegexLegacyTests), "GetTests")]
        public void LegasyTest(SingleFileTest test)
        {
            var regexPattern = test.GetRoot().Element("REGEX").Value;
            var oregexPattern = test.GetRoot().Element("OREGEX").Value;

            var text = test.GetRoot().Element("TEXT").Value;

            var regex = new Regex(regexPattern);
            var oregex = new ObjectRegex<char>(oregexPattern, ORegexOptions.None, _table);

            var regexMatches = regex.Matches(text).Cast<Match>().ToArray();
            var oregexMatches = oregex.Matches(text.ToCharArray()).ToArray();

            Trace.TraceInformation("Text length: "+ text.Length);
            Assert.AreEqual(regexMatches.Length, oregexMatches.Length);

            for (int i = 0; i < regexMatches.Length; i++)
            {
                var rm = regexMatches[i];
                var orm = oregexMatches[i];
                Compare(rm, orm);
            }
        }

        private static void Compare(Match expected, ObjectMatch<char> actual)
        {
            Assert.AreEqual(expected.Index, actual.Index);
            Assert.AreEqual(expected.Length, actual.Length);
        }
    }
}
