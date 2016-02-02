namespace ZSharp.Parser
open System;
open FParsec;
open ZSharp;
open Identifier;

module Functions =

    type Restriction =
        | D of DirectTypeRestriction
        | U of Unit

    // A restriction of the type A:B meaning A is a B
    and DirectTypeRestriction = {
        Left: Type.NamedType
        Right: Type.TypeSignature
    }

    type Signature = {
        Name : string;
        Input : Type.TypeSignature;
        Output : Type.TypeSignature;
    }

    type Function = {
        Signature: Signature
        Restrictions: List<Restriction>
        Body: Expressions.Expression
    }

    //Parses the restrictions on types in a function. e.g.
    //	  A : X
    let rec restriction_parser a = ((direct_type_restriction |>> D)) a;

    // A restriction meaning the left type is a more derived type than the right e.g. A : B
    and direct_type_restriction a = (parse { let! left = Parser.Type.named_parser
                                             do! Whitespace.skip_str_ws ":"
                                             let! right = Parser.Type.parser
                                             return {
                                                 Left = left;
                                                 Right = right;
                                             }}) a;

    // Parses the first line of a function, e.g.:
    //   fun qsort2 : (N, [T]) -> [T]
    //        ^ name    ^ input    ^ output
    let signature_parser a = (parse { do! Whitespace.skip_str_ws "fun "
                                      let! name = Identifier.parser
                                      do! Whitespace.skip_str_ws ":"
                                      let! input = Type.parser
                                      do! Whitespace.skip_str_ws "->"
                                      let! output = Type.parser
                                      return {
                                          Name = name;
                                          Input = input;
                                          Output = output
                                      }}) a;

    let restrictions_list a = (parse { do! Whitespace.skip_str_ws "where"
                                       do! Whitespace.skip_str_ws "["
                                       let! restrictions = sepBy restriction_parser (attempt (Whitespace.skip_str_ws ","))
                                       do! Whitespace.skip_str_ws "]"
                                       return restrictions
                                     }) a;
    
    let function_parser a = (parse { let! signature = signature_parser
                                     let! restrictions = restrictions_list
                                     let! body = Expressions.parser
                                     return {
                                        Signature = signature;
                                        Restrictions = restrictions;
                                        Body = body;
                                     }}) a;

    
