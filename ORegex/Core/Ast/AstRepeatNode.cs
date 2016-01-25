namespace ORegex.Core.Ast
{
    public sealed class AstRepeatNode : AstNodeBase
    {
        public readonly int MinCount;

        public readonly int MaxCount;

        public readonly AstNodeBase Argument;

        public AstRepeatNode(AstNodeBase arg, int minCount, int maxCount)
        {
            Argument = arg;
            MinCount = minCount;
            MaxCount = maxCount;
        }
    }
}
