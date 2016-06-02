namespace Eocron.Core.Ast.GroupQuantifiers
{
    public sealed class CaptureQuantifier : QuantifierBase
    {
        public string CaptureName;

        public int CaptureId;

        public CaptureQuantifier(string originalString, int id) : base(originalString)
        {
            CaptureName = originalString.Substring(3, originalString.Length - 4).ThrowIfEmpty();
            CaptureId = id;
        }

        public static bool IsCapture(string str)
        {
            return str.StartsWith("(?<") && str.Length > 4;
        }
    }
}
