using Antlr4.Runtime;

namespace ORegex.Core.Parse
{
    public class ORegexErrorListener : BaseErrorListener
    {
        public override void SyntaxError(IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg,
            RecognitionException e)
        {
            throw new ORegexException(string.Format("SyntaxError: {0} [token: {1},line: {2}, pos: {3}]",msg, offendingSymbol.Text, line,charPositionInLine));
        }
    }
}
