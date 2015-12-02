using System;
using System.Collections.Generic;
using System.Linq;
using Sprache;

namespace ZSharp.Grammar
{
    internal class Core
    {
        public static readonly Parser<string> Identifier =
            from first in Parse.Letter.Once().Or(Parse.Char('_').Once())
            from rest in Parse.LetterOrDigit.XOr(Parse.Char('_')).Many()
            select new string(first.Concat(rest).ToArray());

        public static readonly Parser<uint> SignificantWhitespace =
            from ws in Parse.String("    ").Or(Parse.String("\t"))
            select checked((uint)1);

        private static readonly char[] _insignificantWhitespace = {
            ' ', '\t'
        };
        public static readonly Parser<char> InsignificantWhitespace =
            from ws in Parse.Chars(_insignificantWhitespace)
            select ws;

        public static readonly Parser<IEnumerable<char>> Newline =
            from ws in Parse.String("\r\n").Or(
                       Parse.String("\n\r")).Or(
                       Parse.String("\n")).Or(
                       Parse.String("\r")).Or(
                       Parse.String(Environment.NewLine))
            select ws;
    }
}
