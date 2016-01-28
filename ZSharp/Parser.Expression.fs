namespace ZSharp.Parser
open System;
open FParsec;
open ZSharp;
open Identifier;
open Parser.Type;

module Expressions =

    type Expression =
        | A of Unit
        | B of Unit

    let rec parser a = (parse { do! Whitespace.skip_str_ws "Hello"
                                return A(())
                              }) a;