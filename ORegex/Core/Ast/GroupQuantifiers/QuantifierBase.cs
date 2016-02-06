namespace ORegex.Core.Ast.GroupQuantifiers
{
    public abstract class QuantifierBase
    {
        public string OriginalString;

        protected QuantifierBase(string originalString)
        {
            OriginalString = originalString.ThrowIfEmpty();
        }
    }
}
