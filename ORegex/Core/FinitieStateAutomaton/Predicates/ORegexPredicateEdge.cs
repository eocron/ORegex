namespace Eocron.Core.FinitieStateAutomaton.Predicates
{
    public sealed class ORegexPredicateEdge<TValue> : SystemPredicateEdge<TValue>
    {
        private bool IsNegative { get; set; }

        private ORegex<TValue> _oregex { get; set; }

        public ORegexPredicateEdge(string name, ORegex<TValue> regex, bool isNegative) : base(name, true)
        {
            IsNegative = isNegative;
        }

        public override bool IsMatch(SequenceHandler<TValue> values, int index)
        {
            var isMatch = _oregex.IsMatch(values.Collection, index);
            return IsNegative ^ isMatch;
        }
    }
}
