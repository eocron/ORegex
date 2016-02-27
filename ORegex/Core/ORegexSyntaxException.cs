using Antlr4.Runtime;

namespace Eocron.Core
{
    public sealed class ORegexSyntaxException : ORegexException
    {
        public readonly IRecognizer Recognizer;
        public readonly IToken OffendingSymbol;
        public readonly int Line;
        public readonly int CharPositionInLine;
        public ORegexSyntaxException(IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine,
            string msg,
            RecognitionException e) : base(msg, e)
        {
            Recognizer = recognizer;
            OffendingSymbol = offendingSymbol;
            Line = line;
            CharPositionInLine = charPositionInLine;
        }
    }
}
