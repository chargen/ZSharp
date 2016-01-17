namespace ZSharp.Parser
open System;
open FParsec;
open ZSharp;
open Identifier;

module FunctionSignature =

    type Signature = {
        name : string;
        Input : Type.TypeSignature;
        Output : Type.TypeSignature;
    }

    let rec parser a = (parse { do! Whitespace.skip_str_ws "fun "
                                let! name = Identifier.parser
                                do! Whitespace.skip_str_ws ":"
                                let! input = Type.parser
                                do! Whitespace.skip_str_ws "->"
                                let! output = Type.parser
                                return {
                                    name = name;
                                    Input = input;
                                    Output = output
                                }}) a;
    
    

    
