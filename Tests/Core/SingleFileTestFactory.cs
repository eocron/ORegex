﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Tests.Core
{
    public class SingleFileTestFactory
    {
        public static IEnumerable<SingleFileTest> GetTests(string testFolderPath, params string[] expectedToFail)
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData\\", testFolderPath);
            var files = Directory.GetFiles(path, "*.test");
            foreach (var file in files)
            {
                var name = Path.GetFileNameWithoutExtension(file);
                if (expectedToFail.Contains(name))
                {
                    continue;
                }
                yield return new SingleFileTest(file);
            }
        }
    }
}
