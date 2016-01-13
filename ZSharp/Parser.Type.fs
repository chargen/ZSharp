namespace ZSharp.Parser
open System;
open FParsec;
open ZSharp;
open Identifier;

module Type =
    let typeSignature a = pstring "ttt" a;
    let tuple a = (skipString "(" >>. (many typeSignature .>> identifier .>> skipString ",") .>> skipString ")") a;