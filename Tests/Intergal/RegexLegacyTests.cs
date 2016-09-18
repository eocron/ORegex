using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Eocron;
using NUnit.Framework;
using Tests.Core;

namespace Tests.Intergal
{
    [TestFixture]
    public sealed class RegexLegacyTests
    {
        private const string TextTag = "TEXT";
        private const string RegexTag = "REGEX";
        private const string OregexTag = "OREGEX";
        private const string OptionsTag = "options";

        // ReSharper disable once UnusedMethodReturnValue.Local
        private static IEnumerable<SingleFileTest> GetTests()
        {
            return SingleFileTestFactory.GetTests("Legacy");
        }

        [Test, TestCaseSource(typeof(RegexLegacyTests), nameof(GetTests))]
        public void LegasyTest(SingleFileTest test)
        {
            var xreg = test.GetRoot().Element(RegexTag);
            var xoreg = test.GetRoot().Element(OregexTag);
            var regexOptions = GetRegexOptions(xreg);
            var oRegexOptions = GetORegexOptions(xoreg);

            // ReSharper disable once PossibleNullReferenceException
            var regexPattern = xreg.Value;
            // ReSharper disable once PossibleNullReferenceException
            var oregexPattern = xoreg.Value;

            // ReSharper disable once PossibleNullReferenceException
            var text = test.GetRoot().Element(TextTag).Value;

            var regex = new Regex(regexPattern, regexOptions);
            var oregex = new DebugORegex(oregexPattern, oRegexOptions);

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
            catch
            {
                Console.WriteLine("##############################################################");
                Console.WriteLine("#                         EXPECTED                           #");
                Console.WriteLine("##############################################################");
                foreach (var m in regexMatches)
                {
                    Console.WriteLine(ExpectedString(m));
                }
                throw;
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

        private static RegexOptions GetRegexOptions(XElement regex)
        {
            const RegexOptions stdOptions = RegexOptions.ExplicitCapture | RegexOptions.Singleline | RegexOptions.Compiled;

            var additionalOptions = RegexOptions.None;
            var xoptions = regex.Attribute(OptionsTag);
            if (xoptions != null)
            {
                additionalOptions = additionalOptions | xoptions.Value.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => Enum.Parse(typeof (RegexOptions), x))
                    .Cast<RegexOptions>()
                    .Aggregate((output, next) => output | next);
            }
            return stdOptions | additionalOptions;
        }

        private static ORegexOptions GetORegexOptions(XElement oregex)
        {
            var additionalOptions = ORegexOptions.None;
            var xoptions = oregex.Attribute(OptionsTag);
            if (xoptions != null)
            {
                additionalOptions = additionalOptions | xoptions.Value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => Enum.Parse(typeof(ORegexOptions), x))
                    .Cast<ORegexOptions>()
                    .Aggregate((output, next) => output | next);
            }
            return additionalOptions;
        }

        private static void Compare(Match expected, OMatch<char> actual)
        {
            Assert.AreEqual(expected.Index, actual.Index);
            Assert.AreEqual(expected.Length, actual.Length);
            CompareCaptures(expected, actual);
        }

        private static void CompareCaptures(Match expected, OMatch<char> actual)
        {
            var groups = expected.Groups.Cast<Group>().Where(x => x.Success).ToArray();
            Assert.AreEqual(groups.Length, actual.Captures.Count);
            foreach (var group in actual.Captures)
            {
                var captured = expected.Groups[group.Key].Captures.Cast<Capture>().ToArray();
                foreach (var capt in group.Value)
                {
                    if (!captured.Any(x => x.Index == capt.Index && x.Length == capt.Length))
                    {
                        Assert.Fail("Captures not equal." + capt);
                    }
                }
            }
        }

        private static string ExpectedString(Match expected)
        {
            return string.Format("Value: {0},\tindex: {1}, length: {2}", expected.Value,
                expected.Index, expected.Length);
        }

        private static string ActualString(OMatch<char> actual)
        {
            return string.Format("Value: {0},\tindex: {1}, length: {2}", new string(actual.Values.ToArray()),
                actual.Index, actual.Length);
        }
    }
}
