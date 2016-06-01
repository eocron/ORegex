using System.Collections.Generic;
using System.Linq;

namespace Eocron.Core.Ast
{
    public sealed class AstRootNode : AstNodeBase
    {
        public bool MatchBegin;

        public bool MatchEnd;

        public AstNodeBase Regex;

        public string[] CaptureGroupNames;

        public AstRootNode(AstNodeBase innerExpression, bool matchBegin, bool matchEnd, Range range, IEnumerable<string> captureGroupNames)
            : base(range)
        {
            MatchBegin = matchBegin;
            MatchEnd = matchEnd;
            Regex = innerExpression;
            CaptureGroupNames = captureGroupNames == null ? new string[0] : captureGroupNames.ToArray();
        }

        public override string ToString()
        {
            return string.Format("REGEX[b:{0}][e:{1}]",MatchBegin,MatchEnd);
        }

        public override IEnumerable<AstNodeBase> GetChildren()
        {
            yield return Regex;
        }
    }
}
