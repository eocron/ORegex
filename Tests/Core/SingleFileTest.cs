using System;
using System.Diagnostics;
using System.IO;
using System.Xml.Linq;
using NUnit.Framework;

namespace Tests.Core
{
    public sealed class SingleFileTest
    {
        public string Name { get; set; }

        public bool Ignored { get; set; }

        private readonly Lazy<XElement> _getRoot; 
        public SingleFileTest(string filePath, bool isIgnored)
        {
            Ignored = isIgnored;
            Name = Path.GetFileNameWithoutExtension(filePath);
            _getRoot = new Lazy<XElement>(() => ReadFile(filePath));
        }

        private static XElement ReadFile(string fileName)
        {
            try
            {
                var text = File.ReadAllText(fileName);
                var root = XElement.Parse(text);
                return root;
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
            }
            return null;
        }

        public XElement GetRoot()
        {
            if (Ignored)
            {
                Assert.Ignore("Ignored test.");
            }
            return _getRoot.Value;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
