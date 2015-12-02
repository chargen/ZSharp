using Sprache;

namespace ZSharp.Grammar.Function
{
    public class FunctionBody
    {
        internal static readonly Parser<FunctionBody> Parser =
            from arms in MatchArm.Parser.AtLeastOnce()
            select new FunctionBody();
    }
}
