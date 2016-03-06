namespace Eocron.Core.Ast.GroupQuantifiers
{
    public sealed class CaptureQuantifier : QuantifierBase
    {
        public string CaptureName;

        public int CaptureId;

        public CaptureQuantifier(string originalString, string name, int id) : base(originalString)
        {
            CaptureName = name.ThrowIfEmpty();
            CaptureId = id;
        }
    }
}
