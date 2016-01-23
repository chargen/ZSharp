namespace ZSharp.Parser
open System;
open FParsec;
open ZSharp;
open Identifier;

module Type =
    type TypeSignature =
        | N of NamedType
        | T of TupleType
        | L of ListType
        | F of FunctionType

    and NamedType = {
        Name: string
    }

    and ListType = {
        ElementType : TypeSignature
    }

    and TupleType = {
        ElementTypes: List<TypeSignature>
    }

    and FunctionType = {
        Input: TypeSignature
        Output: TypeSignature
    }

    let private new_Named n = { Name = n };
    let private new_List t = { ElementType = t };
    let private new_Tuple ts = { ElementTypes = ts };
    let private new_func i o = { Input = i; Output = o };

    // Parses types e.g.
    //      A
    //      [A]
    //      (A -> B)
    //      (A, B)
    let rec parser a = ((named_parser |>> N) <|> (list_parser |>> L) <|> (attempt (func_parser |>> F) <|> (tuple_parser |>> T))) a;

    // A directly named type e.g. "A"
    and named_parser a = (Identifier.parser |>> new_Named) a;

    // A list of items all of the same type e.g. [A]
    and list_parser a = (parse { do! Whitespace.skip_str_ws "["
                                 let! t = parser
                                 do! Whitespace.skip_str_ws "]"
                                 return new_List t }) a

    // A function mapping from one type to another e.g. (A -> B)
    and func_parser a = (parse { do! Whitespace.skip_str_ws "("
                                 let! a = parser
                                 do! Whitespace.skip_str_ws "->"
                                 let! b = parser
                                 do! Whitespace.skip_str_ws ")"
                                 return new_func a b }) a

    // A collection of items, each with their own type e.g. (A, B)
    and tuple_parser a = (parse { do! Whitespace.skip_str_ws "("
                                  let! types = sepBy parser (Whitespace.skip_str_ws ",")
                                  do! Whitespace.skip_str_ws ")"
                                  return new_Tuple types }) a