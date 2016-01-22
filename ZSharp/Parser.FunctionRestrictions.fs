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

    and DirectTypeRestriction = {
        Left: string
        Right: TypeSignature
    }

    //Parses the restrictions on types in a function. e.g.
    //	  where A : X
    //	  where B : Y
    //	  where C : Z
    let rec parser a = (parse { let! a = skipString "A"
                                return D({Left = "A"; Right = N({ Name = "B" }) })}) a;

    and direct_type_restriction a = (parse { do! Whitespace.skip_str_ws "where "
                                             return {
                                                 Left = "A";
                                                 Right = N({ Name = "X" });
                                             }}) a;