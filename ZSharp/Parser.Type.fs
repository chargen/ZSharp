namespace ZSharp.Parser
open System;
open FParsec;
open ZSharp;
open Identifier;

module Type =
    
    type TypeSignature =
        | Name of string
        | Tuple of list<TypeSignature>
        | List of TypeSignature
        | Function of TypeSignature * TypeSignature

    let rec anyType a = (func <|> named <|> tuple <|> list) a;

    and named a = (identifier |>> Name) a;
    and list a = ((skipString "[") >>. anyType .>> (skipString "]") |>> List) a;
    and func a = ((skipString "{") >>. ((anyType .>> (spaces .>> skipString "->" .>> spaces) .>>. anyType) .>> (skipString "}")) |>> Function) a;
    and tuple a = (((skipString "(") >>. (sepBy anyType (skipString "," .>> spaces)) .>> (skipString ")")) |>> Tuple) a;
    
    

    
