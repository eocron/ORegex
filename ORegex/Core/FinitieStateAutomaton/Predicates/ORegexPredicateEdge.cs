namespace Eocron.Core.FinitieStateAutomaton.Predicates
{
    public sealed class ORegexPredicateEdge<TValue> : SystemPredicateEdge<TValue>
    {
        private readonly bool _isNegative;
        private readonly bool _isBehind;
        private readonly ORegex<TValue> _oregex;

        public ORegexPredicateEdge(string name, ORegex<TValue> oregex, bool isNegative, bool isBehind) : base(name, true)
        {
            _isNegative = isNegative;
            _oregex = oregex;
            _isBehind = isBehind;
        }

        public override bool IsMatch(SequenceHandler<TValue> values, int index)
        {
            index = _isBehind ? index - 1 : index;
            index = values.Invert(index);
            if (index < 0)
            {
                return false;
            }
            var isMatch = _oregex.IsMatch(values.Collection, index);
            return _isNegative ^ isMatch;
        }
    }
}
