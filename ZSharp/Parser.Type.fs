namespace ZSharp.Parser
open System;
open FParsec;

module Type =
    let typeSignature a = pstring "ttt" a;
    let tuple a = (skipString "(" >>. (many typeSignature .>> Core.ws .>> skipString ",") .>> skipString ")") a;