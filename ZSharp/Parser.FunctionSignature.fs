namespace ZSharp.Parser
open System;
open FParsec;
open ZSharp;
open Identifier;

module FunctionSignature =

    type Signature = {
        Name : string;
        Input : Type.TypeSignature;
        Output : Type.TypeSignature;
    }

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

    let restrictions_list a = (parse { do! Whitespace.skip_str_ws "where "
                                       let! restrictions = sepBy Parser.FunctionRestrictions.parser (attempt (Whitespace.skip_str_ws ",") <|> (attempt Whitespace.skip_newline_ws) <|> (Whitespace.skip_str_ws "&"))
                                       return restrictions
                                       }) a;
    
    

    
