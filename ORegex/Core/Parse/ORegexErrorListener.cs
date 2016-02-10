using Antlr4.Runtime;

namespace ORegex.Core.Parse
{
    public class ORegexErrorListener : BaseErrorListener
    {
        public override void SyntaxError(IRecognizer recognizer, IToken offendingSymbol, int line,
            int charPositionInLine, string msg,
            RecognitionException e)
        {
            throw new ORegexSyntaxException(recognizer, offendingSymbol, line, charPositionInLine, msg, e);
        }
    }
}
