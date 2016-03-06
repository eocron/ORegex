namespace Eocron.Core.FinitieStateAutomaton.Predicates
{
    public sealed class SystemPredicateEdge<TValue> : PredicateEdgeBase<TValue>
    {
        public readonly bool IsUnique;
        public SystemPredicateEdge(string cmd, bool isUnique = false) : base(cmd)
        {
            IsUnique = isUnique;
        }

        public override bool IsFuncPredicate
        {
            get { return false; }
        }

        public override bool IsComparePredicate
        {
            get { return false; }
        }

        public override bool IsSystemPredicate
        {
            get { return true; }
        }

        public bool IsCapture { get; set; }

        public string CaptureName { get; set; }

        public int CaptureId { get; set; }

        public override bool IsMatch(TValue[] values, int index)
        {
            return true;
        }
    }
}
