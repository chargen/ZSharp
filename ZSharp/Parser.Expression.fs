namespace ZSharp.Parser
open System;
open FParsec;
open ZSharp;
open Identifier;
open Parser.Type;

module Expressions =

    type BinaryOperator =
        | Multiply  // *
        | Add       // +
        | Subtract  // -
        | Divide    // /
        | ShiftRight    // >>
        | ShiftLeft     // <<
        | Exponent  // ^
        | And       // &
        | Or        // |
        | Greater   // >
        | Lesser    // <
        | Chain     // ;

    type Expression =
        | Operator of OperatorExpression        //todo: (<Expression> Op <Expression>)
        | Match of MatchExpression              //todo: "match"
        | Conditional of ConditionalExpression  // if <Expression> then <Expression> else <Expression>
        | Constructor of ConstructorExpression  //todo: "[" or "(" or "(" or 1234567890
        | Call of FunctionCallExpression        //todo: (<Expression> <Expression>)
        | Bind of BindExpression                // let <identifier> (: type) = <Expression>
        | Read of ReadExpression                // <identifier>
        | Fun of FunctionExpression             //todo: fun (args => expression\n)*

    and OperatorExpression = {
        Left: Expression;
        Right: Expression;
        Op: BinaryOperator;
    }

    and MatchExpression = {
        Pattern: Unit;
        Arms: List<Tuple<Unit, Expression>>;
    }

    and ConditionalExpression = {
        Condition: Expression;
        True: Expression;
        False: Expression;
    }

    and ConstructorExpression =
        | List of List<Expression>
        | Tuple of List<Expression>
        | Function of Unit
        | Number of decimal

    and FunctionCallExpression = {
        Function: Expression;
        Argument: Expression;
    }

    and BindExpression = {
        Name: string;
        Type: Option<TypeSignature>;
        Value: Expression;
    }

    and ReadExpression = {
        Name: string;
    }

    and FunctionExpression = {
        Foo: string;
    }

    let rec parser a = ((conditional_expression |>> Conditional) <|> (bind_parser |>> Bind) <|> (read_expression |>> Read)) a;

    and bind_parser a = (parse { do! Whitespace.skip_str_ws "let "
                                 let! name = Identifier.parser
                                 let! typ = opt (Whitespace.skip_str_ws ":" >>. Parser.Type.parser) 
                                 do! Whitespace.skip_str_ws "="
                                 let! exp = parser
                                 return { Name = name; Value = exp; Type = typ; }
                               }) a;

    and read_expression a = (parse { let! name = Identifier.parser
                                     return { Name = name }
                                   }) a;

    and conditional_expression a = (parse { do! Whitespace.skip_str_ws "if "
                                            let! cond = parser
                                            do! Whitespace.skip_str_ws "then "
                                            let! tru = parser
                                            do! Whitespace.skip_str_ws "else "
                                            let! fals = parser
                                            return { Condition = cond; True = tru; False = fals; }
                                          }) a;