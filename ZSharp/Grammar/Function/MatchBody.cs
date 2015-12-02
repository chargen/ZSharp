using Sprache;

namespace ZSharp.Grammar.Function
{
    public class MatchBody
    {
        public static readonly Parser<MatchBody> Parser =
            from a in Parse.String("a")
            select new MatchBody();
    }
}
