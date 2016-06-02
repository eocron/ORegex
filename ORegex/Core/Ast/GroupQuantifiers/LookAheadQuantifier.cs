namespace Eocron.Core.Ast.GroupQuantifiers
{
    public sealed class LookAheadQuantifier : QuantifierBase
    {
        public const string LookAhead = @"(?=";
        public const string NegativeLookAhead = @"(?!";
        public const string LookBehind = @"(?<=";
        public const string NegativeLookBehind = @"(?<!";

        public readonly bool IsNegative;

        public readonly bool IsBehind;

        public LookAheadQuantifier(string originalString) : base(originalString)
        {
            IsNegative = originalString.StartsWith(NegativeLookAhead) || originalString.StartsWith(NegativeLookBehind);
            IsBehind = originalString.StartsWith(LookBehind) || originalString.StartsWith(NegativeLookBehind);
        }

        public static bool IsLook(string str)
        {
            return str.StartsWith("(?");
        }
    }
}
