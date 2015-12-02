using Sprache;

namespace ZSharp.Grammar.Function
{
    public class Function
    {
        private static readonly Parser<FuncType> _typeSignature =
            from input in Type.Parser.Contained(Core.InsignificantWhitespace.Many(), Core.InsignificantWhitespace.Many())
            from arrow in Parse.String("->")
            from output in Type.Parser.Contained(Core.InsignificantWhitespace.Many(), Core.InsignificantWhitespace.Many())
            select new FuncType(input, output);

        internal static readonly Parser<Function> Parser =
            from fun in Parse.String("fun ")
            from name in Core.Identifier
            from colon in Parse.Char(':').Contained(Core.InsignificantWhitespace.Many(), Core.InsignificantWhitespace.Many())
            from typedef in _typeSignature
            from newline in Core.Newline.Once()
            from body in FunctionBody.Parser
            select new Function(name, typedef);

        public string Name { get; private set; }
        public FuncType Signature { get; private set; }

        private Function(string name, FuncType signature)
        {
            Name = name;
            Signature = signature;
        }
    }
}
