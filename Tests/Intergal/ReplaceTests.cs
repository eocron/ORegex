using System;
using NUnit.Framework;
using Tests.Core;

namespace Tests.Intergal
{
    [TestFixture]
    public sealed class ReplaceTests
    {
        [Test]
        public void ReplaceTest1()
        {
            var oregex = new DebugORegex("{a}+");
            var input = @"1aaa1aaa1aaa111111a1a1".ToCharArray();

            var replace = new string(oregex.Replace(input, x => new[] {'b'}));
            Assert.AreEqual("1b1b1b111111b1b1",replace);
            Console.WriteLine(replace);
        }

        [Test]
        public void ReplaceTest2()
        {
            var oregex = new DebugORegex("{a}+");
            var input = @"1aaa1aaa1aaa111111a1a1".ToCharArray();

            var replace = new string(oregex.Replace(input, x => new char[0]));
            Assert.AreEqual("11111111111",replace);
            Console.WriteLine(replace);
        }

        [Test]
        public void ReplaceTest3()
        {
            var oregex = new DebugORegex("{a}+");
            var input = @"1aaa1aaa1aaa111111a1a1".ToCharArray();

            var replace = new string(oregex.Replace(input, x => new []{'2','3','4'}));
            Assert.AreEqual("12341234123411111123412341",replace);
            Console.WriteLine(replace);
        }

        [Test]
        public void ReplaceTest4()
        {
            var oregex = new DebugORegex("{a}+");
            var input = @"1aaa1aaa1aaa111111a1a1".ToCharArray();

            var replace = new string(oregex.Replace(input, x => new[] { '2', '3', '4', '5' }));
            Assert.AreEqual("1234512345123451111112345123451",replace);
            Console.WriteLine(replace);
        }

        [Test]
        public void ReplaceTest5()
        {
            var oregex = new DebugORegex("{a}*");
            var input = @"11".ToCharArray();

            var replace = new string(oregex.Replace(input, x => new[] { '2', '3', '4', '5' }));
            Assert.AreEqual("23451234512345",replace);
            Console.WriteLine(replace);
        }
    }
}
