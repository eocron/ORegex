using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Tests.Core
{
    public class SingleFileTestFactory
    {
        public static string GetTestData(string testPath)
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData\\", testPath);
            return File.ReadAllText(path);
        }
        public static IEnumerable<SingleFileTest> GetTests(string testFolderPath, params string[] expectedToFail)
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData\\", testFolderPath);
            var files = Directory.GetFiles(path, "*.test");
            return from file in files
                   let name = Path.GetFileNameWithoutExtension(file)
                   let isIgnored = expectedToFail != null && expectedToFail.Contains(name)
                   select new SingleFileTest(file, isIgnored);
        }
    }
}
