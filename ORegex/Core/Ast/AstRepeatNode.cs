namespace ORegex.Core.Ast
{
    public sealed class AstRepeatNode : AstNodeBase
    {
        public readonly int MinCount;

        public readonly int MaxCount;

        public readonly bool IsLazy;

        public AstNodeBase Argument;

        public AstRepeatNode(AstNodeBase arg, int minCount, int maxCount, bool isLazy, Range range)
            : base(range)
        {
            if (minCount > maxCount)
            {
                throw new ORegexException("Invalid expression repeat interval.");
            }
            MinCount = minCount;
            MaxCount = maxCount;
            IsLazy = isLazy;
            Argument = arg.ThrowIfNull();
        }

        public override System.Collections.Generic.IEnumerable<AstNodeBase> GetChildren()
        {
            yield return Argument;
        }

        public override string ToString()
        {
            if (IsLazy)
            {
                return string.Format("Repeat[{0};{1}][lazy]", MinCount, MaxCount);
            }
            return string.Format("Repeat[{0};{1}][greedy]", MinCount, MaxCount);
        }
    }
}
