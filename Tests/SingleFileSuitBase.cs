using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml.Linq;

namespace Tests
{
    public class SingleFileSuitBase
    {
        public class SingleFileTest
        {
            public readonly Lazy<XElement> LazyRoot;

            public string Name { get; set; }

            public SingleFileTest(string fileName)
            {
                Name = Path.GetFileName(fileName);
                LazyRoot = new Lazy<XElement>(() =>
                {
                    try
                    {
                        return XElement.Parse(File.ReadAllText(fileName));
                    }
                    catch (Exception ex)
                    {
                        Trace.TraceError(ex.Message);
                    }
                    return null;
                });
            }
        }

        protected static string SearchPattern
        {
            get { return "*.test"; }
        }

        public static IEnumerable<SingleFileTest> GetTests(string testFolderPath)
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData\\", testFolderPath);
            var files = Directory.GetFiles(path, SearchPattern);
            foreach (var file in files)
            {
                yield return new SingleFileTest(file);
            }
        }
    }
}
