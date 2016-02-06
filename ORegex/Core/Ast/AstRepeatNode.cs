using System.Diagnostics;
namespace ORegex.Core.Ast
{
    [DebuggerDisplay("Repeat:{MinCount}-{MaxCount}")]
    public sealed class AstRepeatNode : AstCollectionNodeBase
    {
        public readonly int MinCount;

        public readonly int MaxCount;

        public readonly bool IsGreedy;

        public AstRepeatNode(AstNodeBase arg, int minCount, int maxCount, bool isGreedy) : base(new[] { arg })
        {
            MinCount = minCount;
            MaxCount = maxCount;
            IsGreedy = isGreedy;
        }
    }
}
