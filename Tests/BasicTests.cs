using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ORegex;

namespace Tests
{
    [TestClass]
    public class BasicTests
    {
        [TestMethod]
        public void PrimeSequence()
        {
            var seq = new int[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13};
            ObjectRegex<int> regex = new ObjectRegex<int>(
                "((({0}(?<tail>.{0})+)))", 
                IsPrime                 //0
            );

            var matches = regex.Matches(seq).ToArray();
            foreach (var m in matches)
            {
                Assert.AreEqual(2, m.Groups.Count);
                Assert.AreSame(m.Groups[0], m.Groups["0"]);
                Assert.AreSame(m.Groups[1], m.Groups["tail"]);

                var tail = m.Groups["tail"];

                Assert.AreNotEqual(tail.Value.Count(),0);
            }
        }

        [TestMethod]
        public void PrimeSequenceContent()
        {
            var seq = new int[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13};
            ObjectRegex<int> regex = new ObjectRegex<int>(
                "{0}(.{0})*",
                IsPrime //0
                );

            var matches = regex.Matches(seq).ToArray();
            if (!Enumerable.SequenceEqual(matches[0].Value, new[] {2}))
            {
                Assert.Fail("Incorrect match");
            }
            if (!Enumerable.SequenceEqual(matches[1].Value, new[] {3, 4, 5, 6, 7}))
            {
                Assert.Fail("Incorrect match");
            }
            if (!Enumerable.SequenceEqual(matches[2].Value, new[] {11, 12, 13}))
            {
                Assert.Fail("Incorrect match");
            }
        }

        public static bool IsEven(int number)
        {
            return number%2 == 0;
        }

        public static bool IsPrime(int number)
        {
            int boundary = (int)Math.Floor(Math.Sqrt(number));

            if (number == 1) return false;
            if (number == 2) return true;

            for (int i = 2; i <= boundary; ++i)
            {
                if (number % i == 0) return false;
            }

            return true;
        }
    }
}
