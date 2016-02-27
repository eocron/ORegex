
using System;
using System.Diagnostics;
using System.Linq;
using Eocron;
using NUnit.Framework;

namespace Tests.Intergal
{
    [TestFixture]
    public sealed class ApplicableTests
    {
        [Test]
        public void PrimeTest()
        {
            var oregex = new ORegex<int>("{0}(.{0})*", IsPrime);

            var input = new int[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13};
            foreach (var match in oregex.Matches(input))
            {
                Trace.WriteLine(string.Join(",", match.Values));
            }
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
