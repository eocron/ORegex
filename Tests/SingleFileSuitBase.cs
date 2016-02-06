using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace Tests
{
    public abstract class SingleFileSuitBase<TInput, TOutput>
    {
        protected class SingleFileTest
        {
            public Func<TInput> LazyInput;

            public Func<TOutput> LazyOutput;
        }
        protected abstract TInput LoadInput(XElement test);

        protected abstract TOutput LoadOutput(XElement test);

        protected abstract string TestFolderPath
        {
            get;
        }

        protected virtual string SearchPattern
        {
            get { return "*.test"; }
        }

        public void GetTests()
        {
            var files = Directory.GetFiles(TestFolderPath, SearchPattern);
            foreach (var file in files)
            {
                try
                {

                }
                catch (Exception)
                {
                    

                    throw;
                }
            }
        }
    }
}
