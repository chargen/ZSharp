namespace ZSharp.Parser
open System;
open FParsec;
open ZSharp;
open Identifier;
open Parser.Type;

module FunctionRestrictions =

    type Restriction =
        | D of DirectTypeRestriction
        | U of Unit

    // A restriction of the type A:B meaning A is a B
    and DirectTypeRestriction = {
        Left: Type.NamedType
        Right: TypeSignature
    }

    //Parses the restrictions on types in a function. e.g.
    //	  A : X
    let rec parser a = ((direct_type_restriction |>> D)) a;

    // A restriction meaning the left type is a more derived type than the right e.g. A : B
    and direct_type_restriction a = (parse { let! left = Parser.Type.named_parser
                                             do! Whitespace.skip_str_ws ":"
                                             let! right = Parser.Type.parser
                                             return {
                                                 Left = left;
                                                 Right = right;
                                             }}) a;