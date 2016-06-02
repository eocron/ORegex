namespace Eocron.Core.FinitieStateAutomaton.Predicates
{
    public sealed class ORegexPredicateEdge<TValue> : SystemPredicateEdge<TValue>
    {
        private bool IsNegative { get; set; }

        private bool IsBehind { get; set; }
        private ORegex<TValue> _oregex { get; set; }

        public ORegexPredicateEdge(string name, ORegex<TValue> oregex, bool isNegative, bool isBehind) : base(name, true)
        {
            IsNegative = isNegative;
            _oregex = oregex;
            IsBehind = isBehind;
        }

        public override bool IsMatch(SequenceHandler<TValue> values, int index)
        {
            index = IsBehind ? index - 1 : index;
            index = values.Invert(index);
            if (index < 0)
            {
                return false;
            }
            var isMatch = _oregex.IsMatch(values.Collection, index);
            return IsNegative ^ isMatch;
        }
    }
}
