using System.Collections.Generic;
using System.Linq;
using Sprache;

namespace ZSharp.Grammar.Function
{
    public abstract class MatchPattern
    {
        private static readonly Parser<ListDestructureValue> _namedListItemPart =
            from name in Core.Identifier
            select new ListDestructureValue(name);

        private static readonly Parser<IEnumerable<ListDestructurePart>> _headSlicedList =
            from head in Core.Identifier
            from dots in Parse.String("..")
            from comma in Parse.Char(',').Contained(Core.InsignificantWhitespace.Many(), Core.InsignificantWhitespace.Many())
            from bindings in _namedListItemPart.DelimitedBy(Parse.Char(',').Contained(Core.InsignificantWhitespace.Many(), Core.InsignificantWhitespace.Many()))
            select new ListDestructurePart[] { new ListDestructureSlice(head) }.Concat(bindings).ToArray();

        private static readonly Parser<IEnumerable<ListDestructurePart>> _emptyList =
            from ws in Core.InsignificantWhitespace.Many()
            select new ListDestructurePart[0];

        private static readonly Parser<IEnumerable<ListDestructurePart>> _fixedLengthNamedValues =
            from binding in _namedListItemPart.DelimitedBy(Parse.Char(',').Contained(Core.InsignificantWhitespace.Many(), Core.InsignificantWhitespace.Many()))
            select binding;

        private static readonly Parser<IEnumerable<ListDestructurePart>> _tailSlicedList =
            from bindings in _namedListItemPart.DelimitedBy(Parse.Char(',').Contained(Core.InsignificantWhitespace.Many(), Core.InsignificantWhitespace.Many()))
            from comma in Parse.Char(',').Contained(Core.InsignificantWhitespace.Many(), Core.InsignificantWhitespace.Many())
            from dots in Parse.String("..")
            from tail in Core.Identifier
            select bindings.Concat(new ListDestructurePart[] { new ListDestructureSlice(tail) }).ToArray();

        private static readonly Parser<IEnumerable<ListDestructurePart>> _midSlicedList =
            from bindings1 in _fixedLengthNamedValues
            from comma1 in Parse.Char(',').Contained(Core.InsignificantWhitespace.Many(), Core.InsignificantWhitespace.Many())
            from dots1 in Parse.String("..")
            from mid in Core.Identifier
            from dots2 in Parse.String("..")
            from comma2 in Parse.Char(',').Contained(Core.InsignificantWhitespace.Many(), Core.InsignificantWhitespace.Many())
            from bindings2 in _fixedLengthNamedValues
            select bindings1.Concat(new ListDestructurePart[] { new ListDestructureSlice(mid) }).Concat(bindings2).ToArray();

        // []
        // [head.., a, b, c]
        // [a, ..mid.., b, c]
        // [a, b, c, ..tail]
        // [a, b, c]
        private static readonly Parser<MatchPattern> _listDestructure =
            from lb in Parse.Char('[')
            from ws1 in Core.InsignificantWhitespace.Many()
            from parts in _emptyList.Or(_headSlicedList).Or(_midSlicedList).Or(_tailSlicedList).Or(_fixedLengthNamedValues)
            from ws2 in Core.InsignificantWhitespace.Many()
            from rb in Parse.Char(']')
            select new ListDestructure(parts.ToArray());

        private static readonly Parser<MatchPattern> _tupleDestructure =
            from lb in Parse.Char('(')
            from names in Core.Identifier.Contained(Core.InsignificantWhitespace.Many(), Core.InsignificantWhitespace.Many()).DelimitedBy(Parse.Char(','))
            from rb in Parse.Char(')')
            select new TupleDestructure(names.ToArray()); 

        private static readonly Parser<MatchPattern> _namedValue =
            from name in Core.Identifier.Or(Parse.Number)
            select new NamedValue(name);

        public static readonly Parser<MatchPattern> Parser =
            from pattern in _namedValue.Or(_tupleDestructure).Or(_listDestructure)
            select pattern;
    }

    public class NamedValue
        : MatchPattern
    {
        public string Name { get; }

        public NamedValue(string name)
        {
            Name = name;
        }
    }

    public class TupleDestructure
        : MatchPattern
    {
        public IReadOnlyList<string> Names { get; }

        public TupleDestructure(IReadOnlyList<string> names)
        {
            Names = names;
        }
    }

    public class ListDestructure
        : MatchPattern
    {
        public IReadOnlyList<ListDestructurePart> Parts { get; }

        public ListDestructure(IReadOnlyList<ListDestructurePart> parts)
        {
            Parts = parts;
        }
    }

    public abstract class ListDestructurePart
    {
        public string Name { get; }

        protected ListDestructurePart(string name)
        {
            Name = name;
        }
    }

    public class ListDestructureValue
        : ListDestructurePart
    {
        public ListDestructureValue(string name)
            : base(name)
        {
        }
    }

    public class ListDestructureSlice
       : ListDestructurePart
    {
        public ListDestructureSlice(string name)
            : base(name)
        {
        }
    }
}
