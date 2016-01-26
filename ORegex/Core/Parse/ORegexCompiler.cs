using ORegex.Core.Ast;
using ORegex.Core.Nsm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ORegex.Core.Parse
{
    public sealed class ORegexCompiler<TValue>
    {
        private readonly ORegexParser<TValue> _parser;

        public ORegexCompiler()
        {
            _parser = new ORegexParser<TValue>();
        }

        public NStateMachine Build(string input, Dictionary<string, Func<TValue, bool>> predicateTable)
        {
            var ast = _parser.Parse(input, predicateTable);
            return null;
        }
    }
}
