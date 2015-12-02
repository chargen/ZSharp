using System.Collections.Generic;
using System.Linq;
using Sprache;

namespace ZSharp.Grammar
{
    public abstract class Type
    {
        private static readonly Parser<Type> _tupleType =
            from lb in Parse.Char('(')
            from t in Parse.Ref(() => Parser).DelimitedBy(Parse.Char(',').Contained(Parse.WhiteSpace.Many(), Parse.WhiteSpace.Many()))
            from rb in Parse.Char(')')
            select new TupleType(t);

        private static readonly Parser<Type> _unitType =
            from t in Parse.String("()")
            select new NamedType("Unit");

        private static readonly Parser<Type> _plainType =
            from t in Core.Identifier
            select new NamedType(t);

        private static readonly Parser<Type> _listType =
            from lb in Parse.Char('[')
            from t in Parse.Ref(() => Parser)
            from rb in Parse.Char(']')
            select new ListType(t);

        private static readonly Parser<FuncType> _funcType =
             from lb in Parse.Char('{')
             from i in Parse.Ref(() => Parser)
             from arrow in Parse.String("->").Contained(Core.InsignificantWhitespace.Many(), Core.InsignificantWhitespace.Many())
             from o in Parse.Ref(() => Parser)
             from rb in Parse.Char('}')
             select new FuncType(i, o);

        internal static readonly Parser<Type> Parser =
            from type in _funcType.Or(_unitType).Or(_tupleType).Or(_plainType).Or(_listType)
            select type;
    }

    public class FuncType
        : Type
    {
        public Type Input { get; }
        public Type Output { get; }

        public FuncType(Type input, Type output)
        {
            Input = input;
            Output = output;
        }
    }

    public class ListType
        : Type
    {
        public Type Element { get; }

        public ListType(Type element)
        {
            Element = element;
        }
    }

    public class NamedType
        : Type
    {
        public string Name { get; }

        public NamedType(string name)
        {
            Name = name;
        }
    }

    public class TupleType
        : Type
    {
        public IReadOnlyList<Type> Elements { get; }

        public TupleType(IEnumerable<Type> elements)
        {
            Elements = elements.ToArray();
        }
    }
}
