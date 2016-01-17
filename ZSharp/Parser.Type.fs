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

    let rec parser a = ((named_parser |>> N) <|> (list_parser |>> L) <|> (attempt (func_parser |>> F) <|> (tuple_parser |>> T))) a;

    and named_parser a = (Identifier.parser |>> new_Named) a;

    and list_parser a = (parse { do! Whitespace.skip_str_ws "["
                                 let! t = parser
                                 do! Whitespace.skip_str_ws "]"
                                 return new_List t }) a

    and func_parser a = (parse { do! Whitespace.skip_str_ws "("
                                 let! a = parser
                                 do! Whitespace.skip_str_ws "->"
                                 let! b = parser
                                 do! Whitespace.skip_str_ws ")"
                                 return new_func a b }) a

    and tuple_parser a = (parse { do! Whitespace.skip_str_ws "("
                                  let! types = sepBy parser (Whitespace.skip_str_ws ",")
                                  do! Whitespace.skip_str_ws ")"
                                  return new_Tuple types }) a