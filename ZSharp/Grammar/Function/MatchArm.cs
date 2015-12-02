using Sprache;

namespace ZSharp.Grammar.Function
{
    public class MatchArm
    {
        public static readonly Parser<MatchArm> Parser =
            from ws in Core.SignificantWhitespace
            from pattern in MatchPattern.Parser
            from arrow in Parse.String("=>").Contained(Core.InsignificantWhitespace.Many(), Core.InsignificantWhitespace.Many())
            from body in MatchBody.Parser
            select new MatchArm(pattern, body);

        public MatchPattern Pattern { get; }
        public MatchBody Body { get; }

        private MatchArm(MatchPattern pattern, MatchBody body)
        {
            Pattern = pattern;
            Body = body;
        }
    }
}
