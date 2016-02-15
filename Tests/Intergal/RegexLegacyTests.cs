
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using NUnit.Framework;
using ORegex;

namespace Tests.Intergal
{
    [TestFixture]
    public sealed class RegexLegacyTests : SingleFileSuitBase
    {
        private static readonly PredicateTable<char> _table = new PredicateTable<char>();

        private static string FolderPath = "Legacy";

        private static IEnumerable<SingleFileTest> GetSFTests()
        {
            return GetTests(FolderPath);
        } 

        public RegexLegacyTests()
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

        [Test, TestCaseSource(typeof (RegexLegacyTests), nameof(GetSFTests))]
        public void LegasyTest(SingleFileTest test)
        {
            var regexPattern = test.LazyRoot.Value.Element("REGEX").Value;
            var oregexPattern = test.LazyRoot.Value.Element("OREGEX").Value;

            var text = test.LazyRoot.Value.Element("TEXT").Value;

            var regex = new Regex(regexPattern);
            var oregex = new ObjectRegex<char>(oregexPattern, ORegexOptions.None, _table);

            var regexMatches = regex.Matches(text).Cast<Match>().ToArray();
            var oregexMatches = oregex.Matches(text.ToCharArray()).ToArray();

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
