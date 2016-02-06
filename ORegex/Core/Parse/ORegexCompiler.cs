using System;
using System.Collections.Generic;

namespace ORegex.Core.Parse
{
    public sealed class ORegexCompiler<TValue>
    {
        private readonly ORegexParser<TValue> _parser;

        public ORegexCompiler()
        {
            _parser = new ORegexParser<TValue>();
        }

        public void Build(string input, Dictionary<string, Func<TValue, bool>> predicateTable)
        {
            var ast = _parser.Parse(input, predicateTable);
        }
    }
}
