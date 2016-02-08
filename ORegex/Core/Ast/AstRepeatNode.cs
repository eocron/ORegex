using System;
using System.Diagnostics;
namespace ORegex.Core.Ast
{
    public sealed class AstRepeatNode : AstNodeBase
    {
        public readonly int MinCount;

        public readonly int MaxCount;

        public readonly bool IsGreedy;

        public AstNodeBase Argument;

        public AstRepeatNode(AstNodeBase arg, int minCount, int maxCount, bool isGreedy)
        {
            if (minCount > maxCount)
            {
                throw new ORegexException("Invalid expression repeat interval.");
            }
            MinCount = minCount;
            MaxCount = maxCount;
            IsGreedy = isGreedy;
            Argument = arg.ThrowIfNull();
        }

        public override System.Collections.Generic.IEnumerable<AstNodeBase> GetChildren()
        {
            yield return Argument;
        }

        public override string ToString()
        {
            if (IsGreedy)
            {
                return string.Format("Repeat[{0};{1}][greedy]", MinCount, MaxCount);
            }
            else
            {
                return string.Format("Repeat[{0};{1}]", MinCount, MaxCount);
            }
        }
    }
}
