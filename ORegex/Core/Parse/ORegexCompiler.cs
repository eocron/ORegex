using ORegex.Core.StateMachine;

namespace ORegex.Core.Parse
{
    public sealed class ORegexCompiler<TValue>
    {
        private readonly ORegexParser<TValue> _parser;
        private readonly StateMachineBuilder<TValue> _stb;
        private readonly StateToDFA<TValue> _converter;
        public ORegexCompiler()
        {
            _parser = new ORegexParser<TValue>();
            _stb = new StateMachineBuilder<TValue>();
            _converter = new StateToDFA<TValue>();
        }

        public DFA<TValue> Build(string input, PredicateTable<TValue> predicateTable)
        {
            var ast = _parser.Parse(input, predicateTable);
            var start = new State<TValue>();
            var end = new State<TValue>();
            _stb.Evaluate(start,end, ast);
            
            var result = _converter.Convert(start, end);
            return result;
        }
    }
}
