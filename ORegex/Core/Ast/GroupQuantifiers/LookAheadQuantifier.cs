namespace Eocron.Core.Ast.GroupQuantifiers
{
    public sealed class LookAheadQuantifier : QuantifierBase
    {
        public readonly bool IsNegative;

        public readonly bool IsBehind;

        public LookAheadQuantifier(string originalString) : base(originalString)
        {
            IsNegative = originalString.StartsWith("(?!") || originalString.StartsWith("(?<!");
            IsBehind = originalString.StartsWith("(?<=") || originalString.StartsWith("(?<!");
        }
    }
}
