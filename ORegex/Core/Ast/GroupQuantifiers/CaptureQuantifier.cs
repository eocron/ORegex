namespace ORegex.Core.Ast.GroupQuantifiers
{
    public sealed class CaptureQuantifier : QuantifierBase
    {
        public string CaptureName;

        public CaptureQuantifier(string originalString, string name) : base(originalString)
        {
            CaptureName = name.ThrowIfEmpty();
        }
    }
}
